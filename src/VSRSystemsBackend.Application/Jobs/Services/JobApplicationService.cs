using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _repository;
    private readonly IJobRepository _jobRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public JobApplicationService(
        IJobApplicationRepository repository,
        IJobRepository jobRepository,
        ICandidateRepository candidateRepository,
        IMapper mapper)
    {
        _repository = repository;
        _jobRepository = jobRepository;
        _candidateRepository = candidateRepository;
        _mapper = mapper;
    }

    public async Task<Result<JobApplicationDto>> CreateAsync(CreateJobApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var job = await _jobRepository.GetByIdAsync(dto.JobId, cancellationToken);
        if (job == null)
            return Result<JobApplicationDto>.Failure("Job not found");

        var candidate = await _candidateRepository.GetByIdAsync(dto.CandidateId, cancellationToken);
        if (candidate == null)
            return Result<JobApplicationDto>.Failure("Candidate not found");

        var existing = await _repository.GetByJobAndCandidateAsync(dto.JobId, dto.CandidateId, cancellationToken);
        if (existing != null)
            return Result<JobApplicationDto>.Failure("Candidate has already applied for this job");

        var application = _mapper.Map<JobApplication>(dto);
        application.Id = Guid.NewGuid().ToString("N")[..20];
        application.Status = "applied";
        application.AppliedAt = DateTime.UtcNow;
        application.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(application, cancellationToken);
        return Result<JobApplicationDto>.Success(await MapToDtoAsync(application, cancellationToken));
    }

    public async Task<Result<JobApplicationDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var application = await _repository.GetByIdAsync(id, cancellationToken);
        if (application == null)
            return Result<JobApplicationDto>.Failure("Job application not found");

        return Result<JobApplicationDto>.Success(await MapToDtoAsync(application, cancellationToken));
    }

    public async Task<Result<PagedResult<JobApplicationDto>>> GetByJobIdAsync(string jobId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(a => a.JobId == jobId);

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "AppliedAt" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobApplicationDto>>.Success(
            PagedResult<JobApplicationDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<JobApplicationDto>>> GetByCandidateIdAsync(string candidateId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(a => a.CandidateId == candidateId);

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "AppliedAt" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobApplicationDto>>.Success(
            PagedResult<JobApplicationDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<JobApplicationDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(a => a.Status == status);

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "AppliedAt" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobApplicationDto>>.Success(
            PagedResult<JobApplicationDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<JobApplicationDto>> UpdateAsync(string id, UpdateJobApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var application = await _repository.GetByIdAsync(id, cancellationToken);
        if (application == null)
            return Result<JobApplicationDto>.Failure("Job application not found");

        _mapper.Map(dto, application);
        application.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(application, cancellationToken);
        return Result<JobApplicationDto>.Success(await MapToDtoAsync(application, cancellationToken));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var application = await _repository.GetByIdAsync(id, cancellationToken);
        if (application == null)
            return Result.Failure("Job application not found");

        await _repository.DeleteAsync(application, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<JobApplicationDto>> ScreenAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SetStatusAsync(id, "screening", cancellationToken);
    }

    public async Task<Result<JobApplicationDto>> InterviewAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SetStatusAsync(id, "interview", cancellationToken);
    }

    public async Task<Result<JobApplicationDto>> OfferAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SetStatusAsync(id, "offer", cancellationToken);
    }

    public async Task<Result<JobApplicationDto>> HireAsync(string id, CancellationToken cancellationToken = default)
    {
        return await SetStatusAsync(id, "hired", cancellationToken);
    }

    public async Task<Result<JobApplicationDto>> RejectAsync(string id, string reason, CancellationToken cancellationToken = default)
    {
        var application = await _repository.GetByIdAsync(id, cancellationToken);
        if (application == null)
            return Result<JobApplicationDto>.Failure("Job application not found");

        application.Status = "rejected";
        application.UpdatedAt = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(reason) && application.CoverLetter.Length == 0)
            application.CoverLetter = reason;

        await _repository.UpdateAsync(application, cancellationToken);
        return Result<JobApplicationDto>.Success(await MapToDtoAsync(application, cancellationToken));
    }

    private async Task<Result<JobApplicationDto>> SetStatusAsync(string id, string status, CancellationToken cancellationToken)
    {
        var application = await _repository.GetByIdAsync(id, cancellationToken);
        if (application == null)
            return Result<JobApplicationDto>.Failure("Job application not found");

        application.Status = status;
        application.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(application, cancellationToken);
        return Result<JobApplicationDto>.Success(await MapToDtoAsync(application, cancellationToken));
    }

    private async Task<JobApplicationDto> MapToDtoAsync(JobApplication application, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<JobApplicationDto>(application);
        if (!string.IsNullOrEmpty(application.JobId))
        {
            var job = await _jobRepository.GetByIdAsync(application.JobId, cancellationToken);
            dto.JobTitle = job?.Title ?? string.Empty;
        }
        if (!string.IsNullOrEmpty(application.CandidateId))
        {
            var candidate = await _candidateRepository.GetByIdAsync(application.CandidateId, cancellationToken);
            dto.CandidateName = candidate == null
                ? string.Empty
                : $"{candidate.FirstName} {candidate.LastName}".Trim();
        }
        return dto;
    }

    private async Task<List<JobApplicationDto>> MapToDtosAsync(IEnumerable<JobApplication> applications, CancellationToken cancellationToken)
    {
        var list = applications.ToList();

        var jobIds = list.Where(a => !string.IsNullOrEmpty(a.JobId)).Select(a => a.JobId).Distinct().ToList();
        var candidateIds = list.Where(a => !string.IsNullOrEmpty(a.CandidateId)).Select(a => a.CandidateId).Distinct().ToList();

        var jobs = jobIds.Count > 0
            ? (await _jobRepository.FindAsync(j => jobIds.Contains(j.Id), cancellationToken)).ToDictionary(j => j.Id)
            : new Dictionary<string, Job>();
        var candidates = candidateIds.Count > 0
            ? (await _candidateRepository.FindAsync(c => candidateIds.Contains(c.Id), cancellationToken)).ToDictionary(c => c.Id)
            : new Dictionary<string, Candidate>();

        return list.Select(a =>
        {
            var dto = _mapper.Map<JobApplicationDto>(a);
            dto.JobTitle = jobs.TryGetValue(a.JobId, out var j) ? j.Title : string.Empty;
            dto.CandidateName = candidates.TryGetValue(a.CandidateId, out var c)
                ? $"{c.FirstName} {c.LastName}".Trim()
                : string.Empty;
            return dto;
        }).ToList();
    }
}
