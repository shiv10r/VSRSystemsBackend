using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Jobs.Interfaces;

public interface IJobService
{
    Task<Result<JobDto>> CreateAsync(CreateJobDto dto, CancellationToken cancellationToken = default);
    Task<Result<JobDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobDto>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobDto>>> GetActiveJobsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobDto>>> GetByCompanyIdAsync(string companyId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobDto>>> GetByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobDto>>> SearchAsync(string searchTerm, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<JobDto>> UpdateAsync(string id, UpdateJobDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobDto>> PublishAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobDto>> CloseAsync(string id, CancellationToken cancellationToken = default);
}

public interface ICompanyService
{
    Task<Result<CompanyDto>> CreateAsync(CreateCompanyDto dto, CancellationToken cancellationToken = default);
    Task<Result<CompanyDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CompanyDto>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CompanyDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CompanyDto>>> GetActiveCompaniesAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<CompanyDto>> UpdateAsync(string id, UpdateCompanyDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IJobApplicationService
{
    Task<Result<JobApplicationDto>> CreateAsync(CreateJobApplicationDto dto, CancellationToken cancellationToken = default);
    Task<Result<JobApplicationDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobApplicationDto>>> GetByJobIdAsync(string jobId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobApplicationDto>>> GetByCandidateIdAsync(string candidateId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<JobApplicationDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<JobApplicationDto>> UpdateAsync(string id, UpdateJobApplicationDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobApplicationDto>> ScreenAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobApplicationDto>> InterviewAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobApplicationDto>> OfferAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobApplicationDto>> HireAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<JobApplicationDto>> RejectAsync(string id, string reason, CancellationToken cancellationToken = default);
}

public interface ICandidateService
{
    Task<Result<CandidateDto>> CreateAsync(CreateCandidateDto dto, CancellationToken cancellationToken = default);
    Task<Result<CandidateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CandidateDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<CandidateDto>> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CandidateDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<CandidateDto>> UpdateAsync(string id, UpdateCandidateDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ISavedJobService
{
    Task<Result<SavedJobDto>> CreateAsync(CreateSavedJobDto dto, CancellationToken cancellationToken = default);
    Task<Result<SavedJobDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SavedJobDto>>> GetByCandidateIdAsync(string candidateId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IScreeningQuestionService
{
    Task<Result<ScreeningQuestionDto>> CreateAsync(CreateScreeningQuestionDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ScreeningQuestionDto>>> GetByJobIdAsync(string jobId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<ScreeningQuestionDto>> UpdateAsync(string id, UpdateScreeningQuestionDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}