using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _repository;
    private readonly IMapper _mapper;

    public CandidateService(ICandidateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CandidateDto>> CreateAsync(CreateCandidateDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(c => c.Email == dto.Email, cancellationToken))
            return Result<CandidateDto>.Failure("Candidate with this email already exists");

        if (await _repository.ExistsAsync(c => c.Phone == dto.Phone, cancellationToken))
            return Result<CandidateDto>.Failure("Candidate with this phone already exists");

        var candidate = _mapper.Map<Candidate>(dto);
        candidate.Id = Guid.NewGuid().ToString("N")[..20];
        candidate.IsActive = true;
        candidate.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(candidate, cancellationToken);
        return Result<CandidateDto>.Success(_mapper.Map<CandidateDto>(candidate));
    }

    public async Task<Result<CandidateDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var candidate = await _repository.GetByIdAsync(id, cancellationToken);
        if (candidate == null)
            return Result<CandidateDto>.Failure("Candidate not found");

        return Result<CandidateDto>.Success(_mapper.Map<CandidateDto>(candidate));
    }

    public async Task<Result<CandidateDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var candidate = await _repository.GetByEmailAsync(email, cancellationToken);
        if (candidate == null)
            return Result<CandidateDto>.Failure("Candidate not found");

        return Result<CandidateDto>.Success(_mapper.Map<CandidateDto>(candidate));
    }

    public async Task<Result<CandidateDto>> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        var candidate = await _repository.GetByPhoneAsync(phone, cancellationToken);
        if (candidate == null)
            return Result<CandidateDto>.Failure("Candidate not found");

        return Result<CandidateDto>.Success(_mapper.Map<CandidateDto>(candidate));
    }

    public async Task<Result<PagedResult<CandidateDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(c => c.FirstName.Contains(request.SearchTerm)
                || c.LastName.Contains(request.SearchTerm)
                || c.Email.Contains(request.SearchTerm)
                || c.Phone.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "FirstName" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<CandidateDto>>.Success(
            PagedResult<CandidateDto>.Create(
                _mapper.Map<List<CandidateDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<CandidateDto>> UpdateAsync(string id, UpdateCandidateDto dto, CancellationToken cancellationToken = default)
    {
        var candidate = await _repository.GetByIdAsync(id, cancellationToken);
        if (candidate == null)
            return Result<CandidateDto>.Failure("Candidate not found");

        _mapper.Map(dto, candidate);
        candidate.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(candidate, cancellationToken);
        return Result<CandidateDto>.Success(_mapper.Map<CandidateDto>(candidate));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var candidate = await _repository.GetByIdAsync(id, cancellationToken);
        if (candidate == null)
            return Result.Failure("Candidate not found");

        await _repository.DeleteAsync(candidate, cancellationToken);
        return Result.Success();
    }
}
