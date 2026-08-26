using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Configurations;

public sealed class CrowdSourceConfiguration : IEntityTypeConfiguration<CrowdSource>
{
    public void Configure(EntityTypeBuilder<CrowdSource> builder)
    {
        builder.ToTable("CrowdSources", "railway"); builder.HasKey(item => item.Id);
        builder.Property(item => item.Name).HasMaxLength(200).IsRequired();
        builder.Property(item => item.AdapterType).HasMaxLength(80).IsRequired();
        builder.Property(item => item.SigningSecretCiphertext).HasMaxLength(2000).IsRequired();
        builder.Property(item => item.PreviousSigningSecretCiphertext).HasMaxLength(2000);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.HasIndex(item => new { item.OrganizationId, item.StationId, item.StationZoneId });
    }
}

public sealed class CrowdObservationConfiguration : IEntityTypeConfiguration<CrowdObservation>
{
    public void Configure(EntityTypeBuilder<CrowdObservation> builder)
    {
        builder.ToTable("CrowdObservations", "railway"); builder.HasKey(item => item.Id);
        builder.Property(item => item.SourceEventId).HasMaxLength(160).IsRequired();
        builder.Property(item => item.Confidence).HasPrecision(5, 4);
        builder.Property(item => item.QualityFlags).HasMaxLength(500);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.HasIndex(item => new { item.OrganizationId, item.SourceId, item.SourceEventId }).IsUnique();
        builder.HasIndex(item => new { item.OrganizationId, item.StationZoneId, item.WindowEnd });
    }
}

public sealed class CrowdThresholdPolicyConfiguration : IEntityTypeConfiguration<CrowdThresholdPolicy>
{
    public void Configure(EntityTypeBuilder<CrowdThresholdPolicy> builder)
    {
        builder.ToTable("CrowdThresholdPolicies", "railway"); builder.HasKey(item => item.Id);
        builder.Property(item => item.OverrideReason).HasMaxLength(1000);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.Ignore(item => item.WarningThreshold); builder.Ignore(item => item.CriticalThreshold);
        builder.HasIndex(item => new { item.OrganizationId, item.StationZoneId, item.EffectiveFrom });
    }
}

public sealed class CrowdAlertConfiguration : IEntityTypeConfiguration<CrowdAlert>
{
    public void Configure(EntityTypeBuilder<CrowdAlert> builder)
    {
        builder.ToTable("CrowdAlerts", "railway"); builder.HasKey(item => item.Id);
        builder.Property(item => item.Level).HasConversion<string>().HasMaxLength(32);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.HasIndex(item => new { item.OrganizationId, item.StationZoneId, item.IsOpen });
    }
}

public sealed class CrowdIncidentConfiguration : IEntityTypeConfiguration<CrowdIncident>
{
    public void Configure(EntityTypeBuilder<CrowdIncident> builder)
    {
        builder.ToTable("CrowdIncidents", "railway"); builder.HasKey(item => item.Id);
        builder.Property(item => item.Title).HasMaxLength(300).IsRequired();
        builder.Property(item => item.Status).HasMaxLength(32).IsRequired();
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.HasIndex(item => new { item.OrganizationId, item.StationId, item.Status });
    }
}

public sealed class CrowdIngestionNonceConfiguration : IEntityTypeConfiguration<CrowdIngestionNonce>
{
    public void Configure(EntityTypeBuilder<CrowdIngestionNonce> builder)
    {
        builder.ToTable("CrowdIngestionNonces", "railway");
        builder.HasKey(item => new { item.SourceId, item.Nonce });
        builder.Property(item => item.Nonce).HasMaxLength(160);
        builder.HasIndex(item => item.AcceptedAt);
    }
}

public sealed class CrowdQuarantineRecordConfiguration : IEntityTypeConfiguration<CrowdQuarantineRecord>
{
    public void Configure(EntityTypeBuilder<CrowdQuarantineRecord> builder)
    {
        builder.ToTable("CrowdQuarantine", "railway"); builder.HasKey(item => item.Id);
        builder.Property(item => item.Reason).HasMaxLength(1000).IsRequired();
        builder.Property(item => item.PayloadHash).HasMaxLength(64).IsRequired();
        builder.HasIndex(item => new { item.OrganizationId, item.SourceId, item.CreatedAt });
    }
}
