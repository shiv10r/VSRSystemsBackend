using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Application.Warehouse.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IMapper _mapper;

    public SupplierService(ISupplierRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<SupplierDto>> CreateAsync(CreateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsByGstinAsync(dto.Gstin, cancellationToken))
            return Result<SupplierDto>.Failure("Supplier with this GSTIN already exists");

        var supplier = _mapper.Map<Supplier>(dto);
        supplier.Id = Guid.NewGuid().ToString("N")[..20];
        supplier.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(supplier, cancellationToken);
        return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(supplier));
    }

    public async Task<Result<SupplierDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, cancellationToken);
        if (supplier == null)
            return Result<SupplierDto>.Failure("Supplier not found");

        return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(supplier));
    }

    public async Task<Result<SupplierDto>> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByGstinAsync(gstin, cancellationToken);
        if (supplier == null)
            return Result<SupplierDto>.Failure("Supplier not found");

        return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(supplier));
    }

    public async Task<Result<PagedResult<SupplierDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(s => s.Name.Contains(request.SearchTerm) || s.Company.Contains(request.SearchTerm) || s.Gstin.Contains(request.SearchTerm));
        }

        var totalCount = await _repository.CountAsync(cancellationToken: cancellationToken);
        
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortDescending 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, request.SortBy));
        }
        else
        {
            query = query.OrderBy(s => s.Name);
        }

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<SupplierDto>>.Success(
            PagedResult<SupplierDto>.Create(
                _mapper.Map<List<SupplierDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<SupplierDto>> UpdateAsync(string id, UpdateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, cancellationToken);
        if (supplier == null)
            return Result<SupplierDto>.Failure("Supplier not found");

        if (dto.Gstin != supplier.Gstin && await _repository.ExistsByGstinAsync(dto.Gstin, cancellationToken))
            return Result<SupplierDto>.Failure("Supplier with this GSTIN already exists");

        _mapper.Map(dto, supplier);
        supplier.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(supplier, cancellationToken);
        return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(supplier));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, cancellationToken);
        if (supplier == null)
            return Result.Failure("Supplier not found");

        await _repository.DeleteAsync(supplier, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<SupplierDto>>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default)
    {
        var suppliers = await _repository.GetActiveSuppliersAsync(cancellationToken);
        return Result<IReadOnlyList<SupplierDto>>.Success(_mapper.Map<List<SupplierDto>>(suppliers));
    }
}