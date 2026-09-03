using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Configurations;
using VSRSystemsBackend.Api.Platform.Outbox;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using System.Text.Json;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

public sealed class RailwayDbContext(
    DbContextOptions<RailwayDbContext> options,
    IRailwayScopeAccessor scopeAccessor) : DbContext(options)
{
    public DbSet<RailwayDivision> Divisions => Set<RailwayDivision>();
    public DbSet<RailwayCorridor> Corridors => Set<RailwayCorridor>();
    public DbSet<RailwayRoute> Routes => Set<RailwayRoute>();
    public DbSet<TimetableService> TimetableServices => Set<TimetableService>();
    public DbSet<TrackSegment> TrackSegments => Set<TrackSegment>();
    public DbSet<RailwayStation> Stations => Set<RailwayStation>();
    public DbSet<StationZone> StationZones => Set<StationZone>();
    public DbSet<RailwayPlatform> Platforms => Set<RailwayPlatform>();
    public DbSet<RailwayAssetType> AssetTypes => Set<RailwayAssetType>();
    public DbSet<RailwayAsset> Assets => Set<RailwayAsset>();
    public DbSet<RailwayCommandReceipt> CommandReceipts => Set<RailwayCommandReceipt>();
    public DbSet<RailwayEvidence> Evidence => Set<RailwayEvidence>();
    public DbSet<PlatformOutboxMessage> OutboxMessages => Set<PlatformOutboxMessage>();
    public DbSet<InspectionTemplate> InspectionTemplates => Set<InspectionTemplate>();
    public DbSet<InspectionPlan> InspectionPlans => Set<InspectionPlan>();
    public DbSet<InspectionAssignment> InspectionAssignments => Set<InspectionAssignment>();
    public DbSet<InspectionRun> InspectionRuns => Set<InspectionRun>();
    public DbSet<Defect> Defects => Set<Defect>();
    public DbSet<MaintenancePlan> MaintenancePlans => Set<MaintenancePlan>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<MaintenancePart> MaintenanceParts => Set<MaintenancePart>();
    public DbSet<PartReservation> PartReservations => Set<PartReservation>();
    public DbSet<ProcurementRequest> ProcurementRequests => Set<ProcurementRequest>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<GoodsReceipt> GoodsReceipts => Set<GoodsReceipt>();
    public DbSet<CrowdSource> CrowdSources => Set<CrowdSource>();
    public DbSet<CrowdObservation> CrowdObservations => Set<CrowdObservation>();
    public DbSet<CrowdThresholdPolicy> CrowdThresholdPolicies => Set<CrowdThresholdPolicy>();
    public DbSet<CrowdAlert> CrowdAlerts => Set<CrowdAlert>();
    public DbSet<CrowdIncident> CrowdIncidents => Set<CrowdIncident>();
    public DbSet<CrowdIngestionNonce> CrowdIngestionNonces => Set<CrowdIngestionNonce>();
    public DbSet<CrowdQuarantineRecord> CrowdQuarantine => Set<CrowdQuarantineRecord>();
    public DbSet<RailwayAuditRecord> AuditRecords => Set<RailwayAuditRecord>();

    private Guid CurrentOrganizationId => scopeAccessor.GetRequiredScope().OrganizationId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RailwayMasterRecord>(builder =>
        {
            builder.UseTpcMappingStrategy();
            builder.HasKey(record => record.Id);
            builder.Property(record => record.Code).HasMaxLength(64).IsRequired();
            builder.Property(record => record.Name).HasMaxLength(200).IsRequired();
            builder.Property(record => record.Version).IsConcurrencyToken();
            builder.HasIndex(record => new { record.OrganizationId, record.DivisionId, record.Code }).IsUnique();
            builder.HasIndex(record => new { record.OrganizationId, record.RetiredAt });
            builder.HasQueryFilter(record => record.OrganizationId == CurrentOrganizationId);
        });
        modelBuilder.ApplyConfiguration(new RailwayDivisionConfiguration());
        modelBuilder.ApplyConfiguration(new RailwayCorridorConfiguration());
        modelBuilder.ApplyConfiguration(new RailwayRouteConfiguration());
        modelBuilder.ApplyConfiguration(new TimetableServiceConfiguration());
        modelBuilder.ApplyConfiguration(new TrackSegmentConfiguration());
        modelBuilder.ApplyConfiguration(new RailwayStationConfiguration());
        modelBuilder.ApplyConfiguration(new StationZoneConfiguration());
        modelBuilder.ApplyConfiguration(new RailwayPlatformConfiguration());
        modelBuilder.ApplyConfiguration(new RailwayAssetTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RailwayAssetConfiguration());
        modelBuilder.Entity<RailwayCommandReceipt>(builder =>
        {
            builder.ToTable("CommandReceipts", "railway");
            builder.HasKey(receipt => receipt.Id);
            builder.Property(receipt => receipt.IdempotencyKey).HasMaxLength(160).IsRequired();
            builder.Property(receipt => receipt.CommandType).HasMaxLength(120).IsRequired();
            builder.Property(receipt => receipt.Status).HasMaxLength(32).IsRequired();
            builder.Property(receipt => receipt.Code).HasMaxLength(80);
            builder.HasIndex(receipt => new { receipt.OrganizationId, receipt.UserId, receipt.IdempotencyKey }).IsUnique();
            builder.HasQueryFilter(receipt => receipt.OrganizationId == CurrentOrganizationId);
        });
        modelBuilder.Entity<RailwayEvidence>(builder =>
        {
            builder.ToTable("Evidence", "railway");
            builder.HasKey(item => item.Id);
            builder.Property(item => item.Category).HasMaxLength(80).IsRequired();
            builder.Property(item => item.Bucket).HasMaxLength(128).IsRequired();
            builder.Property(item => item.Path).HasMaxLength(512).IsRequired();
            builder.Property(item => item.ContentType).HasMaxLength(100).IsRequired();
            builder.Property(item => item.Sha256).HasMaxLength(64).IsRequired();
            builder.Property(item => item.ScanStatus).HasConversion<string>().HasMaxLength(32);
            builder.Property(item => item.Version).IsConcurrencyToken();
            builder.HasIndex(item => new { item.OrganizationId, item.OwnerRecordId });
            builder.HasIndex(item => new { item.ScanStatus, item.FinalizedAt });
            builder.HasQueryFilter(item => item.OrganizationId == CurrentOrganizationId);
        });
        modelBuilder.Entity<PlatformOutboxMessage>(builder =>
        {
            builder.ToTable("OutboxMessages", "platform");
            builder.HasKey(message => message.Id);
            builder.Property(message => message.EventName).HasMaxLength(180).IsRequired();
            builder.Property(message => message.Payload).HasColumnType("jsonb").IsRequired();
            builder.Property(message => message.CorrelationId).HasMaxLength(128).IsRequired();
            builder.Property(message => message.LastError).HasMaxLength(2000);
            builder.HasIndex(message => new { message.DispatchedAt, message.LeaseUntil, message.OccurredAt });
            builder.HasIndex(message => new { message.OrganizationId, message.EventName });
        });
        modelBuilder.Entity<RailwayAuditRecord>(builder =>
        {
            builder.ToTable("AuditRecords", "railway"); builder.HasKey(item => item.Id);
            builder.Property(item => item.Action).HasMaxLength(32); builder.Property(item => item.ResourceType).HasMaxLength(160);
            builder.Property(item => item.ResourceId).HasMaxLength(80); builder.Property(item => item.CorrelationId).HasMaxLength(128);
            builder.Property(item => item.BeforeJson).HasColumnType("jsonb"); builder.Property(item => item.AfterJson).HasColumnType("jsonb");
            builder.HasIndex(item => new { item.OrganizationId, item.OccurredAt });
            builder.HasQueryFilter(item => item.OrganizationId == CurrentOrganizationId);
        });
        modelBuilder.ApplyConfiguration(new InspectionTemplateConfiguration());
        modelBuilder.ApplyConfiguration(new InspectionPlanConfiguration());
        modelBuilder.ApplyConfiguration(new InspectionAssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new InspectionRunConfiguration());
        modelBuilder.ApplyConfiguration(new DefectConfiguration());
        ConfigureRailwayFilter<InspectionTemplate>(modelBuilder);
        ConfigureRailwayFilter<InspectionPlan>(modelBuilder);
        ConfigureRailwayFilter<InspectionAssignment>(modelBuilder);
        ConfigureRailwayFilter<InspectionRun>(modelBuilder);
        ConfigureRailwayFilter<Defect>(modelBuilder);
        modelBuilder.ApplyConfiguration(new MaintenancePlanConfiguration());
        modelBuilder.ApplyConfiguration(new WorkOrderConfiguration());
        ConfigureRailwayFilter<MaintenancePlan>(modelBuilder);
        ConfigureRailwayFilter<WorkOrder>(modelBuilder);
        modelBuilder.ApplyConfiguration(new MaintenancePartConfiguration()); modelBuilder.ApplyConfiguration(new PartReservationConfiguration());
        modelBuilder.ApplyConfiguration(new ProcurementRequestConfiguration()); modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
        modelBuilder.ApplyConfiguration(new GoodsReceiptConfiguration());
        ConfigureRailwayFilter<MaintenancePart>(modelBuilder); ConfigureRailwayFilter<PartReservation>(modelBuilder);
        ConfigureRailwayFilter<ProcurementRequest>(modelBuilder); ConfigureRailwayFilter<PurchaseOrder>(modelBuilder); ConfigureRailwayFilter<GoodsReceipt>(modelBuilder);
        modelBuilder.ApplyConfiguration(new CrowdSourceConfiguration());
        modelBuilder.ApplyConfiguration(new CrowdObservationConfiguration());
        modelBuilder.ApplyConfiguration(new CrowdThresholdPolicyConfiguration());
        modelBuilder.ApplyConfiguration(new CrowdAlertConfiguration());
        modelBuilder.ApplyConfiguration(new CrowdIncidentConfiguration());
        modelBuilder.ApplyConfiguration(new CrowdIngestionNonceConfiguration());
        modelBuilder.ApplyConfiguration(new CrowdQuarantineRecordConfiguration());
        ConfigureRailwayFilter<CrowdSource>(modelBuilder);
        ConfigureRailwayFilter<CrowdObservation>(modelBuilder);
        ConfigureRailwayFilter<CrowdThresholdPolicy>(modelBuilder);
        ConfigureRailwayFilter<CrowdAlert>(modelBuilder);
        ConfigureRailwayFilter<CrowdIncident>(modelBuilder);
        ConfigureRailwayFilter<CrowdQuarantineRecord>(modelBuilder);

    }

    private void ConfigureRailwayFilter<T>(ModelBuilder modelBuilder) where T : RailwayEntity =>
        modelBuilder.Entity<T>().HasQueryFilter(item => item.OrganizationId == CurrentOrganizationId);

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ValidateOwnership();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ValidateOwnership();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ValidateOwnership()
    {
        CaptureAuditRecords();
        foreach (var entry in ChangeTracker.Entries<RailwayEntity>()
                     .Where(entry => entry.State is EntityState.Added or EntityState.Modified))
        {
            entry.Entity.ValidateOwnership();
        }
    }

    private void CaptureAuditRecords()
    {
        var entries = ChangeTracker.Entries<RailwayEntity>().Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted).ToArray();
        Guid actorId; try { actorId = scopeAccessor.GetRequiredScope().UserId; } catch { actorId = Guid.Empty; }
        foreach (var entry in entries)
        {
            var before = entry.State == EntityState.Added ? null : JsonSerializer.Serialize(entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p]));
            var after = entry.State == EntityState.Deleted ? null : JsonSerializer.Serialize(entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p]));
            AuditRecords.Add(new RailwayAuditRecord(Guid.NewGuid(), entry.Entity.OrganizationId, entry.Entity.DivisionId, actorId,
                entry.State.ToString(), entry.Metadata.ClrType.Name, entry.Entity.Id.ToString(), before, after, entry.Entity.Id.ToString(), DateTimeOffset.UtcNow));
        }
    }
}
