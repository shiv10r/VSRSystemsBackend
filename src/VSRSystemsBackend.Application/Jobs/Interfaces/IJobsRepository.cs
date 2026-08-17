using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Interfaces;

public interface IJobRepository : IRepository<Job>
{
    Task<IReadOnlyList<Job>> GetActiveJobsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Job>> GetByCompanyIdAsync(string companyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Job>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Job>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Job?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Company>> GetActiveCompaniesAsync(CancellationToken cancellationToken = default);
}

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task<IReadOnlyList<JobApplication>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobApplication>> GetByCandidateIdAsync(string candidateId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobApplication>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<JobApplication?> GetByJobAndCandidateAsync(string jobId, string candidateId, CancellationToken cancellationToken = default);
}

public interface ICandidateRepository : IRepository<Candidate>
{
    Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Candidate?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
}

public interface ISavedJobRepository : IRepository<SavedJob>
{
    Task<IReadOnlyList<SavedJob>> GetByCandidateIdAsync(string candidateId, CancellationToken cancellationToken = default);
    Task<SavedJob?> GetByCandidateAndJobAsync(string candidateId, string jobId, CancellationToken cancellationToken = default);
}

public interface IScreeningQuestionRepository : IRepository<ScreeningQuestion>
{
    Task<IReadOnlyList<ScreeningQuestion>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken = default);
}