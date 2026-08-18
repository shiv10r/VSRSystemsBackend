using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class JobSourceConfiguration : IEntityTypeConfiguration<JobSource>
{
    public void Configure(EntityTypeBuilder<JobSource> builder)
    {
        builder.ToTable("job_sources");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Slug).HasMaxLength(220).IsRequired();
        builder.Property(s => s.CompanyId).HasMaxLength(50);
        builder.Property(s => s.SourceType).HasMaxLength(30).HasDefaultValue("JsonFeed");
        builder.Property(s => s.BaseUrl).HasMaxLength(500);
        builder.Property(s => s.FeedUrl).HasMaxLength(500);
        builder.Property(s => s.CareersUrl).HasMaxLength(500);
        builder.Property(s => s.AdapterKey).HasMaxLength(30).IsRequired();
        builder.Property(s => s.IsEnabled).HasDefaultValue(true);
        builder.Property(s => s.IsAuthorized).HasDefaultValue(false);
        builder.Property(s => s.AuthorizationNotes).HasMaxLength(1000);
        builder.Property(s => s.RequestIntervalMinutes).HasDefaultValue(120);
        builder.Property(s => s.MaxRequestsPerMinute).HasDefaultValue(10);
        builder.Property(s => s.DefaultCountry).HasMaxLength(50).HasDefaultValue("India");
        builder.Property(s => s.DefaultCurrency).HasMaxLength(3).HasDefaultValue("INR");
        builder.Property(s => s.UserAgent).HasMaxLength(500);
        builder.Property(s => s.LastSuccessfulRunAt);
        builder.Property(s => s.LastFailedRunAt);
        builder.Property(s => s.ConsecutiveFailures).HasDefaultValue(0);
        builder.Property(s => s.HealthStatus).HasMaxLength(20).HasDefaultValue("Healthy");
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.IsEnabled);
        builder.HasIndex(s => s.HealthStatus);
        builder.HasIndex(s => s.SourceType);
    }
}

public class JobSourceConfigConfiguration : IEntityTypeConfiguration<JobSourceConfig>
{
    public void Configure(EntityTypeBuilder<JobSourceConfig> builder)
    {
        builder.ToTable("job_source_configs");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.JobSourceId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.ConfigJson).HasColumnType("text");
        builder.Property(c => c.Version).HasDefaultValue(1);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedBy).HasMaxLength(100);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.JobSourceId);
        builder.HasIndex(c => new { c.JobSourceId, c.Version }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class RawExternalJobConfiguration : IEntityTypeConfiguration<RawExternalJob>
{
    public void Configure(EntityTypeBuilder<RawExternalJob> builder)
    {
        builder.ToTable("raw_external_jobs");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(j => j.JobSourceId).HasMaxLength(50).IsRequired();
        builder.Property(j => j.ExternalJobId).HasMaxLength(200).IsRequired();
        builder.Property(j => j.SourceUrl).HasMaxLength(1000);
        builder.Property(j => j.ApplyUrl).HasMaxLength(1000);
        builder.Property(j => j.RawTitle).HasMaxLength(500);
        builder.Property(j => j.RawCompany).HasMaxLength(300);
        builder.Property(j => j.RawLocation).HasMaxLength(300);
        builder.Property(j => j.RawDescription).HasColumnType("text");
        builder.Property(j => j.RawSalary).HasMaxLength(500);
        builder.Property(j => j.RawPostedDate).HasMaxLength(100);
        builder.Property(j => j.RawEmploymentType).HasMaxLength(50);
        builder.Property(j => j.RawWorkMode).HasMaxLength(50);
        builder.Property(j => j.RawSkills).HasMaxLength(2000);
        builder.Property(j => j.RawIndustry).HasMaxLength(200);
        builder.Property(j => j.PayloadHash).HasMaxLength(64).IsRequired();
        builder.Property(j => j.RawPayload).HasColumnType("text");
        builder.Property(j => j.FetchedAt).HasDefaultValueSql("NOW()");
        builder.Property(j => j.FirstSeenAt).HasDefaultValueSql("NOW()");
        builder.Property(j => j.LastSeenAt).HasDefaultValueSql("NOW()");
        builder.Property(j => j.ProcessingStatus).HasMaxLength(20).HasDefaultValue("New");
        builder.Property(j => j.ProcessingError).HasMaxLength(2000);
        builder.Property(j => j.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(j => j.UpdatedAt);
        builder.Property(j => j.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(j => new { j.JobSourceId, j.ExternalJobId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(j => j.JobSourceId);
        builder.HasIndex(j => j.ProcessingStatus);
        builder.HasIndex(j => j.FetchedAt);
        builder.HasIndex(j => j.PayloadHash);
    }
}

public class ScrapeRunConfiguration : IEntityTypeConfiguration<ScrapeRun>
{
    public void Configure(EntityTypeBuilder<ScrapeRun> builder)
    {
        builder.ToTable("scrape_runs");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.JobSourceId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.StartedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.CompletedAt);
        builder.Property(r => r.Status).HasMaxLength(30).HasDefaultValue("Queued");
        builder.Property(r => r.TriggeredBy).HasMaxLength(50).HasDefaultValue("Scheduler");
        builder.Property(r => r.JobsDiscovered).HasDefaultValue(0);
        builder.Property(r => r.JobsFetched).HasDefaultValue(0);
        builder.Property(r => r.JobsCreated).HasDefaultValue(0);
        builder.Property(r => r.JobsUpdated).HasDefaultValue(0);
        builder.Property(r => r.JobsUnchanged).HasDefaultValue(0);
        builder.Property(r => r.JobsDuplicate).HasDefaultValue(0);
        builder.Property(r => r.JobsRejected).HasDefaultValue(0);
        builder.Property(r => r.JobsClosed).HasDefaultValue(0);
        builder.Property(r => r.HttpRequests).HasDefaultValue(0);
        builder.Property(r => r.HttpErrors).HasDefaultValue(0);
        builder.Property(r => r.ParseErrors).HasDefaultValue(0);
        builder.Property(r => r.DurationMs).HasDefaultValue(0);
        builder.Property(r => r.ErrorSummary).HasMaxLength(2000);
        builder.Property(r => r.CorrelationId).HasMaxLength(100);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.JobSourceId);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.StartedAt);
        builder.HasIndex(r => r.CorrelationId);
    }
}

public class ScrapeLogConfiguration : IEntityTypeConfiguration<ScrapeLog>
{
    public void Configure(EntityTypeBuilder<ScrapeLog> builder)
    {
        builder.ToTable("scrape_logs");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(l => l.ScrapeRunId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Level).HasMaxLength(10).HasDefaultValue("Info");
        builder.Property(l => l.EventType).HasMaxLength(50).HasDefaultValue("Generic");
        builder.Property(l => l.Message).HasColumnType("text").IsRequired();
        builder.Property(l => l.Url).HasMaxLength(1000);
        builder.Property(l => l.ExternalJobId).HasMaxLength(200);
        builder.Property(l => l.HttpStatusCode);
        builder.Property(l => l.ExceptionType).HasMaxLength(200);
        builder.Property(l => l.MetadataJson).HasColumnType("text");
        builder.Property(l => l.CorrelationId).HasMaxLength(100);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt);
        builder.Property(l => l.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(l => l.ScrapeRunId);
        builder.HasIndex(l => l.Level);
        builder.HasIndex(l => l.EventType);
        builder.HasIndex(l => l.CreatedAt);
    }
}

public class JobSourceMappingConfiguration : IEntityTypeConfiguration<JobSourceMapping>
{
    public void Configure(EntityTypeBuilder<JobSourceMapping> builder)
    {
        builder.ToTable("job_source_mappings");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(m => m.JobId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.JobSourceId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.ExternalJobId).HasMaxLength(200).IsRequired();
        builder.Property(m => m.SourceUrl).HasMaxLength(1000);
        builder.Property(m => m.ApplyUrl).HasMaxLength(1000);
        builder.Property(m => m.FirstSeenAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.LastSeenAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.IsPrimary).HasDefaultValue(true);
        builder.Property(m => m.IsActive).HasDefaultValue(true);
        builder.Property(m => m.PayloadHash).HasMaxLength(64).IsRequired();
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(m => new { m.JobSourceId, m.ExternalJobId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(m => m.JobId);
        builder.HasIndex(m => new { m.JobId, m.JobSourceId }).HasFilter("\"IsDeleted\" = false");
    }
}

public class DuplicateCandidateConfiguration : IEntityTypeConfiguration<DuplicateCandidate>
{
    public void Configure(EntityTypeBuilder<DuplicateCandidate> builder)
    {
        builder.ToTable("duplicate_candidates");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.JobIdA).HasMaxLength(50).IsRequired();
        builder.Property(d => d.JobIdB).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Score).HasColumnType("double precision");
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("Pending");
        builder.Property(d => d.ResolvedAt);
        builder.Property(d => d.ResolvedBy).HasMaxLength(100);
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => new { d.JobIdA, d.JobIdB }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(d => d.Status);
    }
}

public class IngestionErrorConfiguration : IEntityTypeConfiguration<IngestionError>
{
    public void Configure(EntityTypeBuilder<IngestionError> builder)
    {
        builder.ToTable("ingestion_errors");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(e => e.RawExternalJobId).HasMaxLength(50);
        builder.Property(e => e.JobSourceId).HasMaxLength(50);
        builder.Property(e => e.ErrorCode).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Message).HasColumnType("text").IsRequired();
        builder.Property(e => e.RetryCount).HasDefaultValue(0);
        builder.Property(e => e.NextRetryAt);
        builder.Property(e => e.ResolvedAt);
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(e => e.JobSourceId);
        builder.HasIndex(e => e.ErrorCode);
        builder.HasIndex(e => e.ResolvedAt);
    }
}