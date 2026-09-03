using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Configurations;

public abstract class RailwayMasterRecordConfiguration<T>(string tableName) : IEntityTypeConfiguration<T>
    where T : RailwayMasterRecord
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(tableName, "railway");
    }
}

public sealed class RailwayDivisionConfiguration() : RailwayMasterRecordConfiguration<RailwayDivision>("Divisions") { }
public sealed class RailwayCorridorConfiguration() : RailwayMasterRecordConfiguration<RailwayCorridor>("Corridors") { }
public sealed class RailwayRouteConfiguration() : RailwayMasterRecordConfiguration<RailwayRoute>("Routes") { }
public sealed class TimetableServiceConfiguration() : RailwayMasterRecordConfiguration<TimetableService>("TimetableServices")
{
    public override void Configure(EntityTypeBuilder<TimetableService> builder)
    {
        base.Configure(builder);
        builder.Property(service => service.OperatingStatus).HasMaxLength(32).IsRequired();
    }
}
public sealed class StationZoneConfiguration() : RailwayMasterRecordConfiguration<StationZone>("StationZones") { }
public sealed class RailwayPlatformConfiguration() : RailwayMasterRecordConfiguration<RailwayPlatform>("Platforms") { }
public sealed class RailwayAssetTypeConfiguration() : RailwayMasterRecordConfiguration<RailwayAssetType>("AssetTypes") { }
public sealed class RailwayAssetConfiguration() : RailwayMasterRecordConfiguration<RailwayAsset>("Assets")
{
    public override void Configure(EntityTypeBuilder<RailwayAsset> builder)
    {
        base.Configure(builder);
        builder.Property(asset => asset.Criticality).HasMaxLength(32).IsRequired();
        builder.Property(asset => asset.Status).HasMaxLength(32).IsRequired();
        builder.Property(asset => asset.Location).HasColumnType("geometry (point, 4326)");
    }
}

public sealed class TrackSegmentConfiguration() : RailwayMasterRecordConfiguration<TrackSegment>("TrackSegments")
{
    public override void Configure(EntityTypeBuilder<TrackSegment> builder)
    {
        base.Configure(builder);
        builder.Property(segment => segment.Geometry).HasColumnType("geometry (linestring, 4326)").IsRequired();
    }
}

public sealed class RailwayStationConfiguration() : RailwayMasterRecordConfiguration<RailwayStation>("Stations")
{
    public override void Configure(EntityTypeBuilder<RailwayStation> builder)
    {
        base.Configure(builder);
        builder.Property(station => station.Location).HasColumnType("geometry (point, 4326)");
    }
}
