using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using VSRSystemsBackend.Api.Modules.Railway.API.Contracts;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests.Modules.Railway;

public sealed class RailwayMasterDataIsolationTests
{
    [Fact]
    public async Task Asset_listing_excludes_other_organizations_and_divisions()
    {
        var organizationId = Guid.NewGuid();
        var otherOrganizationId = Guid.NewGuid();
        var permittedDivisionId = Guid.NewGuid();
        var otherDivisionId = Guid.NewGuid();
        var scope = new RailwayScope(
            Guid.NewGuid(),
            organizationId,
            new HashSet<Guid> { permittedDivisionId },
            new HashSet<string> { "railway.master-data.read" });
        var options = new DbContextOptionsBuilder<RailwayDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var dbContext = new RailwayDbContext(options, new FixedScopeAccessor(scope));
        var assetTypeId = Guid.NewGuid();
        dbContext.Assets.AddRange(
            new RailwayAsset(Guid.NewGuid(), organizationId, permittedDivisionId, assetTypeId, "VISIBLE", "Visible asset", "High"),
            new RailwayAsset(Guid.NewGuid(), organizationId, otherDivisionId, assetTypeId, "OTHER-DIV", "Other division", "Normal"),
            new RailwayAsset(Guid.NewGuid(), otherOrganizationId, permittedDivisionId, assetTypeId, "OTHER-ORG", "Other organization", "Normal"));
        await dbContext.SaveChangesAsync();

        var result = await new MasterDataHandlers(dbContext).ListAssetsAsync(scope, 1, 50, CancellationToken.None);

        var item = Assert.Single(result.Items);
        Assert.Equal("VISIBLE", item.Code);
        Assert.Equal(organizationId, item.OrganizationId);
        Assert.Equal(permittedDivisionId, item.DivisionId);
        Assert.Equal(1, result.Total);
    }

    [Fact]
    public async Task Master_data_listing_requires_read_permission()
    {
        var scope = new RailwayScope(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new HashSet<Guid> { Guid.NewGuid() },
            new HashSet<string>());
        var options = new DbContextOptionsBuilder<RailwayDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var dbContext = new RailwayDbContext(options, new FixedScopeAccessor(scope));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            new MasterDataHandlers(dbContext).ListAssetsAsync(scope, 1, 50, CancellationToken.None));
    }

    [Fact]
    public async Task Retired_asset_is_retained_but_excluded_from_operational_lists()
    {
        var organizationId = Guid.NewGuid();
        var divisionId = Guid.NewGuid();
        var scope = Scope(organizationId, divisionId, "railway.master-data.read", "railway.master-data.manage");
        var options = new DbContextOptionsBuilder<RailwayDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var dbContext = new RailwayDbContext(options, new FixedScopeAccessor(scope));
        var asset = new RailwayAsset(Guid.NewGuid(), organizationId, divisionId, Guid.NewGuid(), "RETIRED", "Retired asset", "Normal");
        dbContext.Assets.Add(asset);
        await dbContext.SaveChangesAsync();

        await new MasterDataHandlers(dbContext).RetireAsync(scope, "assets", asset.Id, 0, DateTimeOffset.UtcNow, CancellationToken.None);

        Assert.Empty((await new MasterDataHandlers(dbContext).ListAssetsAsync(scope, 1, 50, CancellationToken.None)).Items);
        Assert.NotNull(await dbContext.Assets.IgnoreQueryFilters().SingleOrDefaultAsync(item => item.Id == asset.Id));
    }

    [Fact]
    public void Track_segment_requires_a_linestring_with_two_points()
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        Assert.Throws<ArgumentException>(() => new TrackSegment(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "BAD", "Invalid segment",
            geometryFactory.CreateLineString([])));
    }

    [Fact]
    public async Task Asset_creation_rejects_an_asset_type_from_another_organization()
    {
        var organizationId = Guid.NewGuid();
        var divisionId = Guid.NewGuid();
        var foreignAssetTypeId = Guid.NewGuid();
        var scope = Scope(organizationId, divisionId, "railway.master-data.manage");
        var options = new DbContextOptionsBuilder<RailwayDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var dbContext = new RailwayDbContext(options, new FixedScopeAccessor(scope));
        dbContext.AssetTypes.Add(new RailwayAssetType(foreignAssetTypeId, Guid.NewGuid(), null, "FOREIGN", "Foreign type"));
        await dbContext.SaveChangesAsync();
        var request = new CreateRailwayMasterRecordRequest(
            divisionId, "ASSET", "Asset", ParentId: foreignAssetTypeId, Criticality: "High");

        await Assert.ThrowsAsync<ArgumentException>(() =>
            new MasterDataHandlers(dbContext).CreateAsync(scope, "assets", request, CancellationToken.None));
    }

    private static RailwayScope Scope(Guid organizationId, Guid divisionId, params string[] permissions) =>
        new(Guid.NewGuid(), organizationId, new HashSet<Guid> { divisionId }, new HashSet<string>(permissions));

    private sealed class FixedScopeAccessor(RailwayScope scope) : IRailwayScopeAccessor
    {
        public RailwayScope GetRequiredScope() => scope;
    }
}
