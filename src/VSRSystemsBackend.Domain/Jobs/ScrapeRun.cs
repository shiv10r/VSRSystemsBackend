using System.ComponentModel.DataAnnotations;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public class ScrapeRun : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobSourceId { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    [MaxLength(30)]
    public string Status { get; set; } = ScrapeRunStatus.Queued;

    [MaxLength(50)]
    public string TriggeredBy { get; set; } = "Scheduler";

    public int JobsDiscovered { get; set; }
    public int JobsFetched { get; set; }
    public int JobsCreated { get; set; }
    public int JobsUpdated { get; set; }
    public int JobsUnchanged { get; set; }
    public int JobsDuplicate { get; set; }
    public int JobsRejected { get; set; }
    public int JobsClosed { get; set; }
    public int HttpRequests { get; set; }
    public int HttpErrors { get; set; }
    public int ParseErrors { get; set; }
    public long DurationMs { get; set; }

    [MaxLength(2000)]
    public string? ErrorSummary { get; set; }

    [MaxLength(100)]
    public string? CorrelationId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class ScrapeLog : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ScrapeRunId { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Level { get; set; } = "Info";

    [MaxLength(50)]
    public string EventType { get; set; } = "Generic";

    public string Message { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Url { get; set; }

    [MaxLength(200)]
    public string? ExternalJobId { get; set; }

    public int? HttpStatusCode { get; set; }

    [MaxLength(200)]
    public string? ExceptionType { get; set; }

    public string? MetadataJson { get; set; }

    [MaxLength(100)]
    public string? CorrelationId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}