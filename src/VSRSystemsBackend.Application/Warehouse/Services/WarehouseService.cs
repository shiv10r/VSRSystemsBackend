using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Common;
using DomainWarehouse = VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Application.Warehouse.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _repository;
    private readonly IMapper _mapper;

    public WarehouseService(IWarehouseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsByCodeAsync(dto.Code, cancellationToken))
            return Result<WarehouseDto>.Failure("Warehouse with this code already exists");

        var warehouse = _mapper.Map<DomainWarehouse.Warehouse>(dto);
        warehouse.Id = Guid.NewGuid().ToString("N")[..20];
        warehouse.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(warehouse, cancellationToken);
        return Result<WarehouseDto>.Success(_mapper.Map<WarehouseDto>(warehouse));
    }

    public async Task<Result<WarehouseDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, cancellationToken);
        if (warehouse == null)
            return Result<WarehouseDto>.Failure("Warehouse not found");

        return Result<WarehouseDto>.Success(_mapper.Map<WarehouseDto>(warehouse));
    }

    public async Task<Result<WarehouseDto>> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByCodeAsync(code, cancellationToken);
        if (warehouse == null)
            return Result<WarehouseDto>.Failure("Warehouse not found");

        return Result<WarehouseDto>.Success(_mapper.Map<WarehouseDto>(warehouse));
    }

    public async Task<Result<PagedResult<WarehouseDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(w => w.Name.Contains(request.SearchTerm) || w.Code.Contains(request.SearchTerm));
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
            query = query.OrderBy(w => w.Name);
        }

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<WarehouseDto>>.Success(
            PagedResult<WarehouseDto>.Create(
                _mapper.Map<List<WarehouseDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<WarehouseDto>> UpdateAsync(string id, UpdateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, cancellationToken);
        if (warehouse == null)
            return Result<WarehouseDto>.Failure("Warehouse not found");

        if (dto.Code != warehouse.Code && await _repository.ExistsByCodeAsync(dto.Code, cancellationToken))
            return Result<WarehouseDto>.Failure("Warehouse with this code already exists");

        _mapper.Map(dto, warehouse);
        warehouse.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(warehouse, cancellationToken);
        return Result<WarehouseDto>.Success(_mapper.Map<WarehouseDto>(warehouse));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _repository.GetByIdAsync(id, cancellationToken);
        if (warehouse == null)
            return Result.Failure("Warehouse not found");

        await _repository.DeleteAsync(warehouse, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<WarehouseDto>>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _repository.GetActiveWarehousesAsync(cancellationToken);
        return Result<IReadOnlyList<WarehouseDto>>.Success(_mapper.Map<List<WarehouseDto>>(warehouses));
    }
}