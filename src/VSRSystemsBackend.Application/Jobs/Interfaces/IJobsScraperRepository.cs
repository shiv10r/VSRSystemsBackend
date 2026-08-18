using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Interfaces;

public interface IJobSourceRepository : IRepository<JobSource>
{
    Task<JobSource?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobSource>> GetEnabledSourcesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobSource>> GetDueSourcesAsync(CancellationToken cancellationToken = default);
}

public interface IJobSourceConfigRepository : IRepository<JobSourceConfig>
{
    Task<JobSourceConfig?> GetActiveConfigAsync(string jobSourceId, CancellationToken cancellationToken = default);
}

public interface IRawExternalJobRepository : IRepository<RawExternalJob>
{
    Task<RawExternalJob?> GetBySourceAndExternalIdAsync(string jobSourceId, string externalJobId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RawExternalJob>> GetBySourceIdAsync(string jobSourceId, int limit = 100, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RawExternalJob>> GetByProcessingStatusAsync(string status, int limit = 100, CancellationToken cancellationToken = default);
}

public interface IScrapeRunRepository : IRepository<ScrapeRun>
{
    Task<IReadOnlyList<ScrapeRun>> GetBySourceIdAsync(string jobSourceId, int limit = 50, CancellationToken cancellationToken = default);
    Task<ScrapeRun?> GetLatestRunAsync(string jobSourceId, CancellationToken cancellationToken = default);
}

public interface IScrapeLogRepository : IRepository<ScrapeLog>
{
    Task<IReadOnlyList<ScrapeLog>> GetByRunIdAsync(string scrapeRunId, CancellationToken cancellationToken = default);
}

public interface IJobSourceMappingRepository : IRepository<JobSourceMapping>
{
    Task<JobSourceMapping?> GetBySourceAndExternalIdAsync(string jobSourceId, string externalJobId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobSourceMapping>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken = default);
}

public interface IDuplicateCandidateRepository : IRepository<DuplicateCandidate>
{
    Task<IReadOnlyList<DuplicateCandidate>> GetPendingAsync(int limit = 50, CancellationToken cancellationToken = default);
}

public interface IIngestionErrorRepository : IRepository<IngestionError>
{
    Task<IReadOnlyList<IngestionError>> GetBySourceIdAsync(string jobSourceId, int limit = 50, CancellationToken cancellationToken = default);
}