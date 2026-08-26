using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using VSRSystemsBackend.Api.Modules.Railway.API.Contracts;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

public sealed record RailwayPage<T>(IReadOnlyList<T> Items, int Page, int PageSize, int Total);

public sealed record AssetSummary(
    Guid Id,
    Guid OrganizationId,
    Guid DivisionId,
    Guid AssetTypeId,
    string Code,
    string Name,
    string Criticality,
    long Version);

public sealed record StationSummary(
    Guid Id,
    Guid OrganizationId,
    Guid DivisionId,
    string Code,
    string Name,
    double? Latitude,
    double? Longitude,
    long Version);

public sealed record RouteSummary(
    Guid Id,
    Guid OrganizationId,
    Guid DivisionId,
    Guid CorridorId,
    string Code,
    string Name,
    long Version);

public sealed record MasterRecordSummary(
    Guid Id,
    Guid OrganizationId,
    Guid? DivisionId,
    string Kind,
    string Code,
    string Name,
    DateTimeOffset? RetiredAt,
    long Version);

public sealed class MasterDataHandlers(RailwayDbContext dbContext)
{
    private static readonly GeometryFactory GeometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

    public async Task<RailwayPage<AssetSummary>> ListAssetsAsync(
        RailwayScope scope,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.master-data.read");
        var query = dbContext.Assets
            .AsNoTracking()
            .Where(asset => asset.RetiredAt == null && asset.DivisionId.HasValue && scope.DivisionIds.Contains(asset.DivisionId.Value))
            .OrderBy(asset => asset.Code);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(asset => new AssetSummary(
                asset.Id,
                asset.OrganizationId,
                asset.DivisionId!.Value,
                asset.AssetTypeId,
                asset.Code,
                asset.Name,
                asset.Criticality,
                asset.Version))
            .ToListAsync(cancellationToken);
        return new RailwayPage<AssetSummary>(items, page, pageSize, total);
    }

    public async Task<RailwayPage<StationSummary>> ListStationsAsync(
        RailwayScope scope,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.master-data.read");
        var query = dbContext.Stations
            .AsNoTracking()
            .Where(station => station.RetiredAt == null && station.DivisionId.HasValue && scope.DivisionIds.Contains(station.DivisionId.Value))
            .OrderBy(station => station.Code);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(station => new StationSummary(
                station.Id,
                station.OrganizationId,
                station.DivisionId!.Value,
                station.Code,
                station.Name,
                station.Location == null ? null : station.Location.Y,
                station.Location == null ? null : station.Location.X,
                station.Version))
            .ToListAsync(cancellationToken);
        return new RailwayPage<StationSummary>(items, page, pageSize, total);
    }

    public async Task<RailwayPage<RouteSummary>> ListRoutesAsync(
        RailwayScope scope,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.master-data.read");
        var query = dbContext.Routes
            .AsNoTracking()
            .Where(route => route.RetiredAt == null && route.DivisionId.HasValue && scope.DivisionIds.Contains(route.DivisionId.Value))
            .OrderBy(route => route.Code);
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(route => new RouteSummary(
                route.Id,
                route.OrganizationId,
                route.DivisionId!.Value,
                route.CorridorId,
                route.Code,
                route.Name,
                route.Version))
            .ToListAsync(cancellationToken);
        return new RailwayPage<RouteSummary>(items, page, pageSize, total);
    }

    public async Task<RailwayPage<MasterRecordSummary>> ListAsync(
        RailwayScope scope,
        string kind,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.master-data.read");
        var normalizedKind = NormalizeKind(kind);
        var query = Query(normalizedKind)
            .AsNoTracking()
            .Where(record => record.RetiredAt == null &&
                (!record.DivisionId.HasValue || scope.DivisionIds.Contains(record.DivisionId.Value)))
            .OrderBy(record => record.Code);
        var total = await query.CountAsync(cancellationToken);
        var records = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new RailwayPage<MasterRecordSummary>(
            records.Select(record => Map(normalizedKind, record)).ToList(),
            page,
            pageSize,
            total);
    }

    public async Task<MasterRecordSummary> CreateAsync(
        RailwayScope scope,
        string kind,
        CreateRailwayMasterRecordRequest request,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.master-data.manage");
        var normalizedKind = NormalizeKind(kind);
        var id = Guid.NewGuid();
        var divisionId = normalizedKind == "divisions" ? id : request.DivisionId;
        if (normalizedKind != "divisions" && normalizedKind != "asset-types")
            scope.RequireDivision(Required(request.DivisionId, nameof(request.DivisionId)));
        else if (divisionId.HasValue && normalizedKind == "asset-types")
            scope.RequireDivision(divisionId.Value);

        var entity = await CreateEntityAsync(scope, normalizedKind, id, divisionId, request, cancellationToken);
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(normalizedKind, entity);
    }

    public async Task<MasterRecordSummary> UpdateAsync(
        RailwayScope scope,
        string kind,
        Guid id,
        UpdateRailwayMasterRecordRequest request,
        long expectedVersion,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.master-data.manage");
        var normalizedKind = NormalizeKind(kind);
        var entity = await FindRequiredAsync(normalizedKind, id, cancellationToken);
        if (entity.DivisionId.HasValue) scope.RequireDivision(entity.DivisionId.Value);
        entity.UpdateIdentity(request.Code, request.Name, expectedVersion);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Map(normalizedKind, entity);
    }

    public async Task RetireAsync(
        RailwayScope scope,
        string kind,
        Guid id,
        long expectedVersion,
        DateTimeOffset retiredAt,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.master-data.manage");
        var entity = await FindRequiredAsync(NormalizeKind(kind), id, cancellationToken);
        if (entity.DivisionId.HasValue) scope.RequireDivision(entity.DivisionId.Value);
        entity.Retire(retiredAt, expectedVersion);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<RailwayMasterRecord> CreateEntityAsync(
        RailwayScope scope,
        string kind,
        Guid id,
        Guid? divisionId,
        CreateRailwayMasterRecordRequest request,
        CancellationToken cancellationToken)
    {
        var organizationId = scope.OrganizationId;
        var scopedDivisionId = divisionId ?? Guid.Empty;
        switch (kind)
        {
            case "divisions":
                return new RailwayDivision(id, organizationId, request.Code, request.Name);
            case "corridors":
                return new RailwayCorridor(id, organizationId, scopedDivisionId, request.Code, request.Name);
            case "routes":
                await EnsureParentAsync("corridors", Required(request.ParentId, nameof(request.ParentId)), scopedDivisionId, false, cancellationToken);
                if (request.SecondaryParentId.HasValue)
                    await EnsureParentAsync("stations", request.SecondaryParentId.Value, scopedDivisionId, false, cancellationToken);
                if (request.TertiaryParentId.HasValue)
                    await EnsureParentAsync("stations", request.TertiaryParentId.Value, scopedDivisionId, false, cancellationToken);
                return new RailwayRoute(id, organizationId, scopedDivisionId, request.ParentId!.Value, request.Code, request.Name, request.SecondaryParentId, request.TertiaryParentId);
            case "timetable-services":
                await EnsureParentAsync("routes", Required(request.ParentId, nameof(request.ParentId)), scopedDivisionId, false, cancellationToken);
                if (request.SecondaryParentId.HasValue)
                    await EnsureParentAsync("platforms", request.SecondaryParentId.Value, scopedDivisionId, false, cancellationToken);
                return new TimetableService(
                    id, organizationId, scopedDivisionId, request.ParentId!.Value, request.Code, request.Name,
                    request.EffectiveFrom ?? DateTimeOffset.UtcNow, request.DepartureWindowStart, request.DepartureWindowEnd,
                    request.SecondaryParentId, request.Status ?? "Active");
            case "track-segments":
                var coordinates = request.Geometry?.Select(point => new Coordinate(point.Longitude, point.Latitude)).ToArray();
                return new TrackSegment(id, organizationId, scopedDivisionId, request.Code, request.Name,
                    GeometryFactory.CreateLineString(coordinates ?? []));
            case "stations":
                return new RailwayStation(id, organizationId, scopedDivisionId, request.Code, request.Name, CreatePoint(request));
            case "zones":
                await EnsureParentAsync("stations", Required(request.ParentId, nameof(request.ParentId)), scopedDivisionId, false, cancellationToken);
                return new StationZone(id, organizationId, scopedDivisionId, request.ParentId!.Value, request.Code, request.Name);
            case "platforms":
                await EnsureParentAsync("stations", Required(request.ParentId, nameof(request.ParentId)), scopedDivisionId, false, cancellationToken);
                return new RailwayPlatform(id, organizationId, scopedDivisionId, request.ParentId!.Value, request.Code, request.Name);
            case "asset-types":
                return new RailwayAssetType(id, organizationId, divisionId, request.Code, request.Name);
            case "assets":
                await EnsureParentAsync("asset-types", Required(request.ParentId, nameof(request.ParentId)), scopedDivisionId, true, cancellationToken);
                return new RailwayAsset(
                    id, organizationId, scopedDivisionId, request.ParentId!.Value, request.Code, request.Name,
                    request.Criticality ?? "Normal", request.Status ?? "Active", CreatePoint(request));
            default:
                throw new ArgumentOutOfRangeException(nameof(kind));
        }
    }

    private async Task EnsureParentAsync(
        string kind,
        Guid id,
        Guid divisionId,
        bool allowOrganizationWide,
        CancellationToken cancellationToken)
    {
        var exists = await Query(kind).AnyAsync(record =>
            record.Id == id && (record.DivisionId == divisionId || (allowOrganizationWide && record.DivisionId == null)),
            cancellationToken);
        if (!exists) throw new ArgumentException("The referenced Railway parent does not exist in the authenticated organization and division.");
    }

    private async Task<RailwayMasterRecord> FindRequiredAsync(string kind, Guid id, CancellationToken cancellationToken) =>
        await Query(kind).SingleOrDefaultAsync(record => record.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException("The Railway master-data record was not found.");

    private IQueryable<RailwayMasterRecord> Query(string kind) => kind switch
    {
        "divisions" => dbContext.Divisions,
        "corridors" => dbContext.Corridors,
        "routes" => dbContext.Routes,
        "timetable-services" => dbContext.TimetableServices,
        "track-segments" => dbContext.TrackSegments,
        "stations" => dbContext.Stations,
        "zones" => dbContext.StationZones,
        "platforms" => dbContext.Platforms,
        "asset-types" => dbContext.AssetTypes,
        "assets" => dbContext.Assets,
        _ => throw new ArgumentException("Unsupported Railway master-data type.", nameof(kind)),
    };

    private static string NormalizeKind(string kind) => kind.Trim().ToLowerInvariant() switch
    {
        "divisions" or "corridors" or "routes" or "timetable-services" or "track-segments" or
        "stations" or "zones" or "platforms" or "asset-types" or "assets" => kind.Trim().ToLowerInvariant(),
        _ => throw new ArgumentException("Unsupported Railway master-data type.", nameof(kind)),
    };

    private static MasterRecordSummary Map(string kind, RailwayMasterRecord record) => new(
        record.Id, record.OrganizationId, record.DivisionId, kind, record.Code, record.Name, record.RetiredAt, record.Version);

    private static Guid Required(Guid? value, string name) =>
        value is { } id && id != Guid.Empty ? id : throw new ArgumentException("A non-empty identifier is required.", name);

    private static Point? CreatePoint(CreateRailwayMasterRecordRequest request) =>
        request.Latitude.HasValue && request.Longitude.HasValue
            ? GeometryFactory.CreatePoint(new Coordinate(request.Longitude.Value, request.Latitude.Value))
            : null;
}
