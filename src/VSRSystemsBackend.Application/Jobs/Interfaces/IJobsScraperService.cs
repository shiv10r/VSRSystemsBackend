using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Jobs.Interfaces;

public interface IJobsScraperService
{
    Task SeedAsync(CancellationToken cancellationToken = default);
    Task<List<JobSourceDto>> GetSourcesAsync(CancellationToken cancellationToken = default);
    Task<JobSourceDto?> GetSourceAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobSourceDto>> CreateSourceAsync(CreateJobSourceDto dto, CancellationToken cancellationToken = default);
    Task<Result<JobSourceDto>> UpdateSourceAsync(string id, UpdateJobSourceDto dto, CancellationToken cancellationToken = default);
    Task<Result<JobSourceDto>> SetSourceEnabledAsync(string id, bool enabled, CancellationToken cancellationToken = default);
    Task<JobSourceConfigDto?> GetSourceConfigAsync(string sourceId, CancellationToken cancellationToken = default);
    Task<List<ScrapeRunDto>> GetRunsAsync(string? sourceId = null, int limit = 50, CancellationToken cancellationToken = default);
    Task<ScrapeRunDto?> GetRunAsync(string runId, CancellationToken cancellationToken = default);
    Task<List<ScrapeLogDto>> GetRunLogsAsync(string runId, CancellationToken cancellationToken = default);
    Task<List<RawExternalJobDto>> GetRawJobsAsync(string? sourceId = null, string? status = null, int limit = 100, CancellationToken cancellationToken = default);
    Task<List<DuplicateCandidateDto>> GetDuplicatesAsync(int limit = 50, CancellationToken cancellationToken = default);
    Task<List<IngestionErrorDto>> GetErrorsAsync(string? sourceId = null, int limit = 50, CancellationToken cancellationToken = default);
    Task<DuplicateCandidateDto?> ResolveDuplicateAsync(string id, string action, string by, CancellationToken cancellationToken = default);
    Task<JobDto?> ReprocessRawJobAsync(string rawId, CancellationToken cancellationToken = default);
    Task<ScrapeRunDto> RunSourceAsync(string sourceId, string triggeredBy = "Manual", CancellationToken cancellationToken = default);
    Task<ScraperDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
    Task<List<JobSourceDto>> GetDueSourcesAsync(CancellationToken cancellationToken = default);
    void ScheduleNextRun(string sourceId, int intervalMinutes);
    Task<(bool Ok, string Error)> ValidateUrlAsync(string url);
}