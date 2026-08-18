using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;

    public CompanyService(ICompanyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CompanyDto>> CreateAsync(CreateCompanyDto dto, CancellationToken cancellationToken = default)
    {
        var company = _mapper.Map<Company>(dto);
        company.Id = Guid.NewGuid().ToString("N")[..20];
        company.Slug = await SlugHelper.EnsureUniqueSlugAsync(
            SlugHelper.GenerateSlug(dto.Name),
            slug => _repository.ExistsAsync(c => c.Slug == slug, cancellationToken));
        company.IsActive = true;
        company.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(company, cancellationToken);
        return Result<CompanyDto>.Success(_mapper.Map<CompanyDto>(company));
    }

    public async Task<Result<CompanyDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var company = await _repository.GetByIdAsync(id, cancellationToken);
        if (company == null)
            return Result<CompanyDto>.Failure("Company not found");

        return Result<CompanyDto>.Success(_mapper.Map<CompanyDto>(company));
    }

    public async Task<Result<CompanyDto>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var company = await _repository.GetBySlugAsync(slug, cancellationToken);
        if (company == null)
            return Result<CompanyDto>.Failure("Company not found");

        return Result<CompanyDto>.Success(_mapper.Map<CompanyDto>(company));
    }

    public async Task<Result<PagedResult<CompanyDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(c => c.Name.Contains(request.SearchTerm)
                || c.Industry.Contains(request.SearchTerm)
                || c.Location.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "Name" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<CompanyDto>>.Success(
            PagedResult<CompanyDto>.Create(
                _mapper.Map<List<CompanyDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<CompanyDto>>> GetActiveCompaniesAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(c => c.IsActive);

        var totalCount = await query.CountAsync(cancellationToken);

        var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? "Name" : request.SortBy;
        query = request.SortDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : query.OrderBy(e => EF.Property<object>(e, sortBy));

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<CompanyDto>>.Success(
            PagedResult<CompanyDto>.Create(
                _mapper.Map<List<CompanyDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<CompanyDto>> UpdateAsync(string id, UpdateCompanyDto dto, CancellationToken cancellationToken = default)
    {
        var company = await _repository.GetByIdAsync(id, cancellationToken);
        if (company == null)
            return Result<CompanyDto>.Failure("Company not found");

        _mapper.Map(dto, company);
        company.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(company, cancellationToken);
        return Result<CompanyDto>.Success(_mapper.Map<CompanyDto>(company));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var company = await _repository.GetByIdAsync(id, cancellationToken);
        if (company == null)
            return Result.Failure("Company not found");

        await _repository.DeleteAsync(company, cancellationToken);
        return Result.Success();
    }
}
