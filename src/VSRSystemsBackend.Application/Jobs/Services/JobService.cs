using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _repository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public JobService(IJobRepository repository, ICompanyRepository companyRepository, IMapper mapper)
    {
        _repository = repository;
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<Result<JobDto>> CreateAsync(CreateJobDto dto, CancellationToken cancellationToken = default)
    {
        var company = await _companyRepository.GetByIdAsync(dto.CompanyId, cancellationToken);
        if (company == null)
            return Result<JobDto>.Failure("Company not found");

        var job = _mapper.Map<Job>(dto);
        job.Id = Guid.NewGuid().ToString("N")[..20];
        job.Slug = await SlugHelper.EnsureUniqueSlugAsync(
            SlugHelper.GenerateSlug(dto.Title),
            slug => _repository.ExistsAsync(j => j.Slug == slug, cancellationToken));
        job.Status = "draft";
        job.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(job, cancellationToken);
        return Result<JobDto>.Success(await MapToDtoAsync(job, cancellationToken));
    }

    public async Task<Result<JobDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var job = await _repository.GetByIdAsync(id, cancellationToken);
        if (job == null)
            return Result<JobDto>.Failure("Job not found");

        return Result<JobDto>.Success(await MapToDtoAsync(job, cancellationToken));
    }

    public async Task<Result<JobDto>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var job = await _repository.GetBySlugAsync(slug, cancellationToken);
        if (job == null)
            return Result<JobDto>.Failure("Job not found");

        return Result<JobDto>.Success(await MapToDtoAsync(job, cancellationToken));
    }

    public async Task<Result<PagedResult<JobDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(j => j.Title.Contains(request.SearchTerm)
                || j.Description.Contains(request.SearchTerm)
                || j.Category.Contains(request.SearchTerm)
                || j.Location.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, request);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobDto>>.Success(
            PagedResult<JobDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<JobDto>>> GetActiveJobsAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query()
            .Where(j => j.Status == "published"
                && (j.ExpiresAt == null || j.ExpiresAt >= DateTime.UtcNow));

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(j => j.Title.Contains(request.SearchTerm)
                || j.Description.Contains(request.SearchTerm)
                || j.Category.Contains(request.SearchTerm)
                || j.Location.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, request, defaultSortDescending: true, defaultSortBy: "PublishedAt");

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobDto>>.Success(
            PagedResult<JobDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<JobDto>>> GetByCompanyIdAsync(string companyId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(j => j.CompanyId == companyId);

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, request, defaultSortDescending: true, defaultSortBy: "CreatedAt");

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobDto>>.Success(
            PagedResult<JobDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<JobDto>>> GetByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(j => j.Category == category);

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, request, defaultSortDescending: true, defaultSortBy: "CreatedAt");

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobDto>>.Success(
            PagedResult<JobDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<JobDto>>> SearchAsync(string searchTerm, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(j => j.Title.Contains(searchTerm)
            || j.Description.Contains(searchTerm)
            || j.Requirements.Contains(searchTerm)
            || j.Category.Contains(searchTerm)
            || j.Location.Contains(searchTerm));

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySorting(query, request, defaultSortDescending: true, defaultSortBy: "CreatedAt");

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<JobDto>>.Success(
            PagedResult<JobDto>.Create(
                await MapToDtosAsync(items, cancellationToken),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<JobDto>> UpdateAsync(string id, UpdateJobDto dto, CancellationToken cancellationToken = default)
    {
        var job = await _repository.GetByIdAsync(id, cancellationToken);
        if (job == null)
            return Result<JobDto>.Failure("Job not found");

        if (!string.IsNullOrWhiteSpace(dto.CompanyId) && dto.CompanyId != job.CompanyId)
        {
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId, cancellationToken);
            if (company == null)
                return Result<JobDto>.Failure("Company not found");
        }

        _mapper.Map(dto, job);
        job.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(job, cancellationToken);
        return Result<JobDto>.Success(await MapToDtoAsync(job, cancellationToken));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var job = await _repository.GetByIdAsync(id, cancellationToken);
        if (job == null)
            return Result.Failure("Job not found");

        await _repository.DeleteAsync(job, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<JobDto>> PublishAsync(string id, CancellationToken cancellationToken = default)
    {
        var job = await _repository.GetByIdAsync(id, cancellationToken);
        if (job == null)
            return Result<JobDto>.Failure("Job not found");

        job.Status = "published";
        job.PublishedAt = DateTime.UtcNow;
        job.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(job, cancellationToken);
        return Result<JobDto>.Success(await MapToDtoAsync(job, cancellationToken));
    }

    public async Task<Result<JobDto>> CloseAsync(string id, CancellationToken cancellationToken = default)
    {
        var job = await _repository.GetByIdAsync(id, cancellationToken);
        if (job == null)
            return Result<JobDto>.Failure("Job not found");

        job.Status = "closed";
        job.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(job, cancellationToken);
        return Result<JobDto>.Success(await MapToDtoAsync(job, cancellationToken));
    }

    private async Task<JobDto> MapToDtoAsync(Job job, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<JobDto>(job);
        if (!string.IsNullOrEmpty(job.CompanyId))
        {
            var company = await _companyRepository.GetByIdAsync(job.CompanyId, cancellationToken);
            dto.CompanyName = company?.Name ?? string.Empty;
        }
        return dto;
    }

    private async Task<List<JobDto>> MapToDtosAsync(IEnumerable<Job> jobs, CancellationToken cancellationToken)
    {
        var list = jobs.ToList();
        var companyIds = list.Where(j => !string.IsNullOrEmpty(j.CompanyId))
            .Select(j => j.CompanyId)
            .Distinct()
            .ToList();

        var companies = companyIds.Count > 0
            ? (await _companyRepository.FindAsync(c => companyIds.Contains(c.Id), cancellationToken))
                .ToDictionary(c => c.Id)
            : new Dictionary<string, Company>();

        return list.Select(j =>
        {
            var dto = _mapper.Map<JobDto>(j);
            dto.CompanyName = companies.TryGetValue(j.CompanyId, out var c) ? c.Name : string.Empty;
            return dto;
        }).ToList();
    }

    private static IQueryable<Job> ApplySorting(IQueryable<Job> query, PagedRequest request, bool defaultSortDescending = true, string defaultSortBy = "CreatedAt")
    {
        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? defaultSortBy : request.SortBy;
        var sortDescending = request.SortDescending;
        if (string.IsNullOrWhiteSpace(request.SortBy))
            sortDescending = defaultSortDescending;

        return sortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));
    }
}
