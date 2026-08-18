using System.ComponentModel.DataAnnotations;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public static class JobSourceHealth
{
    public const string Healthy = "Healthy";
    public const string Warning = "Warning";
    public const string Failing = "Failing";
    public const string Paused = "Paused";
    public const string Disabled = "Disabled";
}

public static class JobProcessingStatus
{
    public const string New = "New";
    public const string Normalized = "Normalized";
    public const string Duplicate = "Duplicate";
    public const string Published = "Published";
    public const string Rejected = "Rejected";
    public const string Error = "Error";
}

public static class ScrapeRunStatus
{
    public const string Queued = "Queued";
    public const string Running = "Running";
    public const string Succeeded = "Succeeded";
    public const string PartiallySucceeded = "PartiallySucceeded";
    public const string Failed = "Failed";
    public const string Cancelled = "Cancelled";
    public const string Suspicious = "Suspicious";
}

public static class JobSourceType
{
    public const string Api = "Api";
    public const string JsonFeed = "JsonFeed";
    public const string XmlFeed = "XmlFeed";
    public const string Rss = "Rss";
    public const string Sitemap = "Sitemap";
    public const string AtsPublicEndpoint = "AtsPublicEndpoint";
    public const string HtmlCareerPage = "HtmlCareerPage";
    public const string ManualImport = "ManualImport";
    public const string CsvImport = "CsvImport";
    public const string RecruiterPosted = "RecruiterPosted";
    public const string PartnerFeed = "PartnerFeed";
}

public class JobSource : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CompanyId { get; set; }

    [Required]
    [MaxLength(30)]
    public string SourceType { get; set; } = JobSourceType.JsonFeed;

    [MaxLength(500)]
    public string? BaseUrl { get; set; }

    [MaxLength(500)]
    public string? FeedUrl { get; set; }

    [MaxLength(500)]
    public string? CareersUrl { get; set; }

    [Required]
    [MaxLength(30)]
    public string AdapterKey { get; set; } = "JsonFeed";

    public bool IsEnabled { get; set; } = true;
    public bool IsAuthorized { get; set; }

    [MaxLength(1000)]
    public string? AuthorizationNotes { get; set; }

    public int RequestIntervalMinutes { get; set; } = 120;
    public int MaxRequestsPerMinute { get; set; } = 10;

    [MaxLength(50)]
    public string DefaultCountry { get; set; } = "India";

    [MaxLength(3)]
    public string DefaultCurrency { get; set; } = "INR";

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    public DateTime? LastSuccessfulRunAt { get; set; }
    public DateTime? LastFailedRunAt { get; set; }
    public int ConsecutiveFailures { get; set; }

    [MaxLength(20)]
    public string HealthStatus { get; set; } = JobSourceHealth.Healthy;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class JobSourceConfig : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobSourceId { get; set; } = string.Empty;

    public string ConfigJson { get; set; } = "{}";
    public int Version { get; set; } = 1;
    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}