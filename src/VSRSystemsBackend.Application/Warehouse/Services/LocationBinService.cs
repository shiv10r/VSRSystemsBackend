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

public class LocationBinService : ILocationBinService
{
    private readonly ILocationBinRepository _repository;
    private readonly IMapper _mapper;

    public LocationBinService(ILocationBinRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<LocationBinDto>> CreateAsync(CreateLocationBinDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByCodeAsync(dto.WarehouseId, dto.Code, cancellationToken);
        if (existing != null)
            return Result<LocationBinDto>.Failure("Location with this code already exists in this warehouse");

        var location = _mapper.Map<DomainWarehouse.LocationBin>(dto);
        location.Id = Guid.NewGuid().ToString("N")[..20];
        location.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(location, cancellationToken);
        return Result<LocationBinDto>.Success(_mapper.Map<LocationBinDto>(location));
    }

    public async Task<Result<LocationBinDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var location = await _repository.GetByIdAsync(id, cancellationToken);
        if (location == null)
            return Result<LocationBinDto>.Failure("Location not found");

        return Result<LocationBinDto>.Success(_mapper.Map<LocationBinDto>(location));
    }

    public async Task<Result<PagedResult<LocationBinDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(l => l.WarehouseId == warehouseId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(l => l.Code.Contains(request.SearchTerm));
        }

        var totalCount = await _repository.CountAsync(l => l.WarehouseId == warehouseId, cancellationToken);
        
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortDescending 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, request.SortBy));
        }
        else
        {
            query = query.OrderBy(l => l.Code);
        }

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<LocationBinDto>>.Success(
            PagedResult<LocationBinDto>.Create(
                _mapper.Map<List<LocationBinDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<LocationBinDto>> UpdateAsync(string id, UpdateLocationBinDto dto, CancellationToken cancellationToken = default)
    {
        var location = await _repository.GetByIdAsync(id, cancellationToken);
        if (location == null)
            return Result<LocationBinDto>.Failure("Location not found");

        _mapper.Map(dto, location);
        location.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(location, cancellationToken);
        return Result<LocationBinDto>.Success(_mapper.Map<LocationBinDto>(location));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var location = await _repository.GetByIdAsync(id, cancellationToken);
        if (location == null)
            return Result.Failure("Location not found");

        await _repository.DeleteAsync(location, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<LocationBinDto>>> GetActiveByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        var locations = await _repository.GetActiveByWarehouseIdAsync(warehouseId, cancellationToken);
        return Result<IReadOnlyList<LocationBinDto>>.Success(_mapper.Map<List<LocationBinDto>>(locations));
    }
}