using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

public class SavedJobService : ISavedJobService
{
    private readonly ISavedJobRepository _repository;
    private readonly IJobRepository _jobRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public SavedJobService(
        ISavedJobRepository repository,
        IJobRepository jobRepository,
        ICandidateRepository candidateRepository,
        IMapper mapper)
    {
        _repository = repository;
        _jobRepository = jobRepository;
        _candidateRepository = candidateRepository;
        _mapper = mapper;
    }

    public async Task<Result<SavedJobDto>> CreateAsync(CreateSavedJobDto dto, CancellationToken cancellationToken = default)
    {
        var job = await _jobRepository.GetByIdAsync(dto.JobId, cancellationToken);
        if (job == null)
            return Result<SavedJobDto>.Failure("Job not found");

        var candidate = await _candidateRepository.GetByIdAsync(dto.CandidateId, cancellationToken);
        if (candidate == null)
            return Result<SavedJobDto>.Failure("Candidate not found");

        var existing = await _repository.GetByCandidateAndJobAsync(dto.CandidateId, dto.JobId, cancellationToken);
        if (existing != null)
            return Result<SavedJobDto>.Failure("Job already saved by this candidate");

        var savedJob = _mapper.Map<SavedJob>(dto);
        savedJob.Id = Guid.NewGuid().ToString("N")[..20];
        savedJob.SavedAt = DateTime.UtcNow;
        savedJob.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(savedJob, cancellationToken);
        return Result<SavedJobDto>.Success(_mapper.Map<SavedJobDto>(savedJob));
    }

    public async Task<Result<SavedJobDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var savedJob = await _repository.GetByIdAsync(id, cancellationToken);
        if (savedJob == null)
            return Result<SavedJobDto>.Failure("Saved job not found");

        return Result<SavedJobDto>.Success(_mapper.Map<SavedJobDto>(savedJob));
    }

    public async Task<Result<PagedResult<SavedJobDto>>> GetByCandidateIdAsync(string candidateId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(s => s.CandidateId == candidateId);

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "SavedAt" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<SavedJobDto>>.Success(
            PagedResult<SavedJobDto>.Create(
                _mapper.Map<List<SavedJobDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var savedJob = await _repository.GetByIdAsync(id, cancellationToken);
        if (savedJob == null)
            return Result.Failure("Saved job not found");

        await _repository.DeleteAsync(savedJob, cancellationToken);
        return Result.Success();
    }
}
