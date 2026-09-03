using NetTopologySuite.Geometries;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

public abstract class RailwayMasterRecord : RailwayEntity
{
    protected RailwayMasterRecord()
    {
    }

    protected RailwayMasterRecord(Guid id, Guid organizationId, Guid? divisionId, string code, string name)
        : base(id, organizationId, divisionId)
    {
        Code = RequireText(code, nameof(code));
        Name = RequireText(name, nameof(name));
    }

    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public DateTimeOffset? RetiredAt { get; private set; }

    public void UpdateIdentity(string code, string name, long expectedVersion)
    {
        RequireVersion(expectedVersion);
        Code = RequireText(code, nameof(code));
        Name = RequireText(name, nameof(name));
        Version++;
    }

    public void Retire(DateTimeOffset retiredAt, long expectedVersion)
    {
        RequireVersion(expectedVersion);
        if (RetiredAt is not null)
            throw new InvalidOperationException("The Railway master-data record is already retired.");
        RetiredAt = retiredAt;
        Version++;
    }

    private void RequireVersion(long expectedVersion)
    {
        if (Version != expectedVersion)
            throw new InvalidOperationException("The Railway master-data record has changed since it was read.");
    }

    protected static Guid RequireId(Guid value, string parameterName) =>
        value != Guid.Empty ? value : throw new ArgumentException("A non-empty identifier is required.", parameterName);

    protected static string RequireText(string value, string parameterName) =>
        !string.IsNullOrWhiteSpace(value)
            ? value.Trim()
            : throw new ArgumentException("A value is required.", parameterName);
}

public sealed class RailwayDivision : RailwayMasterRecord
{
    private RailwayDivision() { }
    public RailwayDivision(Guid id, Guid organizationId, string code, string name)
        : base(id, organizationId, id, code, name) { }
}

public sealed class RailwayCorridor : RailwayMasterRecord
{
    private RailwayCorridor() { }
    public RailwayCorridor(Guid id, Guid organizationId, Guid divisionId, string code, string name)
        : base(id, organizationId, divisionId, code, name) { }
}

public sealed class RailwayRoute : RailwayMasterRecord
{
    private RailwayRoute() { }
    public RailwayRoute(
        Guid id,
        Guid organizationId,
        Guid divisionId,
        Guid corridorId,
        string code,
        string name,
        Guid? originStationId = null,
        Guid? destinationStationId = null)
        : base(id, organizationId, divisionId, code, name)
    {
        CorridorId = RequireId(corridorId, nameof(corridorId));
        OriginStationId = originStationId;
        DestinationStationId = destinationStationId;
    }
    public Guid CorridorId { get; private set; }
    public Guid? OriginStationId { get; private set; }
    public Guid? DestinationStationId { get; private set; }
}

public sealed class TimetableService : RailwayMasterRecord
{
    private TimetableService() { }
    public TimetableService(
        Guid id,
        Guid organizationId,
        Guid divisionId,
        Guid routeId,
        string code,
        string name,
        DateTimeOffset effectiveFrom,
        TimeOnly? departureWindowStart = null,
        TimeOnly? departureWindowEnd = null,
        Guid? platformId = null,
        string operatingStatus = "Active")
        : base(id, organizationId, divisionId, code, name)
    {
        RouteId = RequireId(routeId, nameof(routeId));
        EffectiveFrom = effectiveFrom;
        DepartureWindowStart = departureWindowStart;
        DepartureWindowEnd = departureWindowEnd;
        PlatformId = platformId;
        OperatingStatus = RequireText(operatingStatus, nameof(operatingStatus));
    }
    public Guid RouteId { get; private set; }
    public DateTimeOffset EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }
    public TimeOnly? DepartureWindowStart { get; private set; }
    public TimeOnly? DepartureWindowEnd { get; private set; }
    public Guid? PlatformId { get; private set; }
    public string OperatingStatus { get; private set; } = "Active";
}

public sealed class TrackSegment : RailwayMasterRecord
{
    private TrackSegment() { }
    public TrackSegment(Guid id, Guid organizationId, Guid divisionId, string code, string name, LineString geometry)
        : base(id, organizationId, divisionId, code, name)
    {
        if (geometry is null || geometry.IsEmpty || geometry.NumPoints < 2)
            throw new ArgumentException("A track segment requires a non-empty LineString with at least two points.", nameof(geometry));
        Geometry = geometry;
    }
    public LineString Geometry { get; private set; } = null!;
}

public sealed class RailwayStation : RailwayMasterRecord
{
    private RailwayStation() { }
    public RailwayStation(Guid id, Guid organizationId, Guid divisionId, string code, string name, Point? location = null)
        : base(id, organizationId, divisionId, code, name) => Location = location;
    public Point? Location { get; private set; }
}

public sealed class StationZone : RailwayMasterRecord
{
    private StationZone() { }
    public StationZone(Guid id, Guid organizationId, Guid divisionId, Guid stationId, string code, string name)
        : base(id, organizationId, divisionId, code, name) => StationId = stationId;
    public Guid StationId { get; private set; }
}

public sealed class RailwayPlatform : RailwayMasterRecord
{
    private RailwayPlatform() { }
    public RailwayPlatform(Guid id, Guid organizationId, Guid divisionId, Guid stationId, string code, string name)
        : base(id, organizationId, divisionId, code, name) => StationId = stationId;
    public Guid StationId { get; private set; }
}

public sealed class RailwayAssetType : RailwayMasterRecord
{
    private RailwayAssetType() { }
    public RailwayAssetType(Guid id, Guid organizationId, Guid? divisionId, string code, string name)
        : base(id, organizationId, divisionId, code, name) { }
}

public sealed class RailwayAsset : RailwayMasterRecord
{
    private RailwayAsset() { }
    public RailwayAsset(
        Guid id,
        Guid organizationId,
        Guid divisionId,
        Guid assetTypeId,
        string code,
        string name,
        string criticality,
        string status = "Active",
        Point? location = null)
        : base(id, organizationId, divisionId, code, name)
    {
        AssetTypeId = RequireId(assetTypeId, nameof(assetTypeId));
        Criticality = RequireText(criticality, nameof(criticality));
        Status = RequireText(status, nameof(status));
        Location = location;
    }
    public Guid AssetTypeId { get; private set; }
    public string Criticality { get; private set; } = "Normal";
    public string Status { get; private set; } = "Active";
    public Point? Location { get; private set; }
}
