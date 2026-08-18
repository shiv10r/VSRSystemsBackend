using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Jobs.DTOs;

public class JobSourceDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? CompanyId { get; set; }
    public string SourceType { get; set; } = "JsonFeed";
    public string? BaseUrl { get; set; }
    public string? FeedUrl { get; set; }
    public string? CareersUrl { get; set; }
    public string AdapterKey { get; set; } = "JsonFeed";
    public bool IsEnabled { get; set; } = true;
    public bool IsAuthorized { get; set; }
    public string? AuthorizationNotes { get; set; }
    public int RequestIntervalMinutes { get; set; } = 120;
    public int MaxRequestsPerMinute { get; set; } = 10;
    public string DefaultCountry { get; set; } = "India";
    public string DefaultCurrency { get; set; } = "INR";
    public string? UserAgent { get; set; }
    public DateTime? LastSuccessfulRunAt { get; set; }
    public DateTime? LastFailedRunAt { get; set; }
    public int ConsecutiveFailures { get; set; }
    public string HealthStatus { get; set; } = "Healthy";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateJobSourceDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CompanyId { get; set; }

    [MaxLength(30)]
    public string SourceType { get; set; } = "JsonFeed";

    [MaxLength(500)]
    public string? BaseUrl { get; set; }

    [MaxLength(500)]
    public string? FeedUrl { get; set; }

    [MaxLength(500)]
    public string? CareersUrl { get; set; }

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
}

public class UpdateJobSourceDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CompanyId { get; set; }

    [MaxLength(30)]
    public string SourceType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? BaseUrl { get; set; }

    [MaxLength(500)]
    public string? FeedUrl { get; set; }

    [MaxLength(500)]
    public string? CareersUrl { get; set; }

    [MaxLength(30)]
    public string AdapterKey { get; set; } = string.Empty;

    public bool? IsEnabled { get; set; }
    public bool? IsAuthorized { get; set; }

    [MaxLength(1000)]
    public string? AuthorizationNotes { get; set; }

    public int? RequestIntervalMinutes { get; set; }
    public int? MaxRequestsPerMinute { get; set; }

    [MaxLength(50)]
    public string? DefaultCountry { get; set; }

    [MaxLength(3)]
    public string? DefaultCurrency { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }
}

public class JobSourceConfigDto
{
    public string Id { get; set; } = string.Empty;
    public string JobSourceId { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = "{}";
    public int Version { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RawExternalJobDto
{
    public string Id { get; set; } = string.Empty;
    public string JobSourceId { get; set; } = string.Empty;
    public string ExternalJobId { get; set; } = string.Empty;
    public string? SourceUrl { get; set; }
    public string? ApplyUrl { get; set; }
    public string? RawTitle { get; set; }
    public string? RawCompany { get; set; }
    public string? RawLocation { get; set; }
    public string? RawSalary { get; set; }
    public string? RawPostedDate { get; set; }
    public string? RawEmploymentType { get; set; }
    public string? RawWorkMode { get; set; }
    public string? RawSkills { get; set; }
    public string? RawIndustry { get; set; }
    public string PayloadHash { get; set; } = string.Empty;
    public DateTime FetchedAt { get; set; }
    public DateTime FirstSeenAt { get; set; }
    public DateTime LastSeenAt { get; set; }
    public string ProcessingStatus { get; set; } = "New";
    public string? ProcessingError { get; set; }
}

public class ScrapeRunDto
{
    public string Id { get; set; } = string.Empty;
    public string JobSourceId { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = "Queued";
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
    public string? ErrorSummary { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ScrapeLogDto
{
    public string Id { get; set; } = string.Empty;
    public string ScrapeRunId { get; set; } = string.Empty;
    public string Level { get; set; } = "Info";
    public string EventType { get; set; } = "Generic";
    public string Message { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? ExternalJobId { get; set; }
    public int? HttpStatusCode { get; set; }
    public string? ExceptionType { get; set; }
    public string? MetadataJson { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DuplicateCandidateDto
{
    public string Id { get; set; } = string.Empty;
    public string JobIdA { get; set; } = string.Empty;
    public string JobIdB { get; set; } = string.Empty;
    public double Score { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? ResolvedAt { get; set; }
    public string? ResolvedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class IngestionErrorDto
{
    public string Id { get; set; } = string.Empty;
    public string? RawExternalJobId { get; set; }
    public string? JobSourceId { get; set; }
    public string ErrorCode { get; set; } = "Unknown";
    public string Message { get; set; } = string.Empty;
    public int RetryCount { get; set; }
    public DateTime? NextRetryAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SourceHealthDto
{
    public string JobSourceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string HealthStatus { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public int ConsecutiveFailures { get; set; }
    public DateTime? LastSuccessfulRunAt { get; set; }
    public DateTime? LastFailedRunAt { get; set; }
    public string? LastError { get; set; }
}

public class ScraperDashboardDto
{
    public DashboardCardsDto Cards { get; set; } = new();
    public DashboardChartsDto Charts { get; set; } = new();
    public List<JobSourceDto> Sources { get; set; } = new();
}

public class DashboardCardsDto
{
    public int EnabledSources { get; set; }
    public int HealthySources { get; set; }
    public int FailingSources { get; set; }
    public int JobsImportedToday { get; set; }
    public int JobsUpdatedToday { get; set; }
    public int JobsClosedToday { get; set; }
    public int DuplicatesDetected { get; set; }
    public int ParseErrors { get; set; }
    public int HttpErrors { get; set; }
    public long AverageRunMs { get; set; }
    public int TotalJobs { get; set; }
    public int TotalRaw { get; set; }
}

public class DashboardChartsDto
{
    public List<RunsOverTimePointDto> RunsOverTime { get; set; } = new();
    public List<JobsBySourcePointDto> JobsBySource { get; set; } = new();
    public List<ErrorsBySourcePointDto> ErrorsBySource { get; set; } = new();
}

public class RunsOverTimePointDto
{
    public string Date { get; set; } = string.Empty;
    public int Runs { get; set; }
    public int Created { get; set; }
    public int Closed { get; set; }
}

public class JobsBySourcePointDto
{
    public string SourceId { get; set; } = string.Empty;
    public int Discovered { get; set; }
    public int Created { get; set; }
}

public class ErrorsBySourcePointDto
{
    public string SourceId { get; set; } = string.Empty;
    public int Count { get; set; }
}