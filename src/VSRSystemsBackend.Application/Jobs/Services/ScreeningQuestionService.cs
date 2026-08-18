using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

public class ScreeningQuestionService : IScreeningQuestionService
{
    private readonly IScreeningQuestionRepository _repository;
    private readonly IJobRepository _jobRepository;
    private readonly IMapper _mapper;

    public ScreeningQuestionService(
        IScreeningQuestionRepository repository,
        IJobRepository jobRepository,
        IMapper mapper)
    {
        _repository = repository;
        _jobRepository = jobRepository;
        _mapper = mapper;
    }

    public async Task<Result<ScreeningQuestionDto>> CreateAsync(CreateScreeningQuestionDto dto, CancellationToken cancellationToken = default)
    {
        var job = await _jobRepository.GetByIdAsync(dto.JobId, cancellationToken);
        if (job == null)
            return Result<ScreeningQuestionDto>.Failure("Job not found");

        var question = _mapper.Map<ScreeningQuestion>(dto);
        question.Id = Guid.NewGuid().ToString("N")[..20];
        question.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(question, cancellationToken);
        return Result<ScreeningQuestionDto>.Success(_mapper.Map<ScreeningQuestionDto>(question));
    }

    public async Task<Result<PagedResult<ScreeningQuestionDto>>> GetByJobIdAsync(string jobId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(q => q.JobId == jobId);

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "Order" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<ScreeningQuestionDto>>.Success(
            PagedResult<ScreeningQuestionDto>.Create(
                _mapper.Map<List<ScreeningQuestionDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<ScreeningQuestionDto>> UpdateAsync(string id, UpdateScreeningQuestionDto dto, CancellationToken cancellationToken = default)
    {
        var question = await _repository.GetByIdAsync(id, cancellationToken);
        if (question == null)
            return Result<ScreeningQuestionDto>.Failure("Screening question not found");

        _mapper.Map(dto, question);
        question.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(question, cancellationToken);
        return Result<ScreeningQuestionDto>.Success(_mapper.Map<ScreeningQuestionDto>(question));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var question = await _repository.GetByIdAsync(id, cancellationToken);
        if (question == null)
            return Result.Failure("Screening question not found");

        await _repository.DeleteAsync(question, cancellationToken);
        return Result.Success();
    }
}
