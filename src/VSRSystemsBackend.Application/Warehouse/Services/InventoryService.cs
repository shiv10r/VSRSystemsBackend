using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Application.Warehouse.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;
    private readonly IStockMovementRepository _movementRepository;
    private readonly IStockAdjustmentRepository _adjustmentRepository;
    private readonly IMapper _mapper;

    public InventoryService(
        IInventoryRepository repository,
        IStockMovementRepository movementRepository,
        IStockAdjustmentRepository adjustmentRepository,
        IMapper mapper)
    {
        _repository = repository;
        _movementRepository = movementRepository;
        _adjustmentRepository = adjustmentRepository;
        _mapper = mapper;
    }

    public async Task<Result<InventoryItemDto>> CreateAsync(CreateInventoryItemDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetBySkuAsync(dto.Sku, dto.WarehouseId, cancellationToken);
        if (existing != null)
            return Result<InventoryItemDto>.Failure("Item with this SKU already exists in this warehouse");

        var item = _mapper.Map<InventoryItem>(dto);
        item.Id = Guid.NewGuid().ToString("N")[..20];
        item.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(item, cancellationToken);
        return Result<InventoryItemDto>.Success(_mapper.Map<InventoryItemDto>(item));
    }

    public async Task<Result<InventoryItemDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return Result<InventoryItemDto>.Failure("Inventory item not found");

        return Result<InventoryItemDto>.Success(_mapper.Map<InventoryItemDto>(item));
    }

    public async Task<Result<PagedResult<InventoryItemDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(i => i.WarehouseId == warehouseId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(i => i.Name.Contains(request.SearchTerm) || i.Sku.Contains(request.SearchTerm));
        }

        var totalCount = await _repository.CountAsync(i => i.WarehouseId == warehouseId, cancellationToken);
        
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortDescending 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, request.SortBy));
        }
        else
        {
            query = query.OrderBy(i => i.Name);
        }

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<InventoryItemDto>>.Success(
            PagedResult<InventoryItemDto>.Create(
                _mapper.Map<List<InventoryItemDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<InventoryItemDto>> GetBySkuAsync(string sku, string warehouseId, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetBySkuAsync(sku, warehouseId, cancellationToken);
        if (item == null)
            return Result<InventoryItemDto>.Failure("Inventory item not found");

        return Result<InventoryItemDto>.Success(_mapper.Map<InventoryItemDto>(item));
    }

    public async Task<Result<InventoryItemDto>> UpdateAsync(string id, UpdateInventoryItemDto dto, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return Result<InventoryItemDto>.Failure("Inventory item not found");

        _mapper.Map(dto, item);
        item.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(item, cancellationToken);
        return Result<InventoryItemDto>.Success(_mapper.Map<InventoryItemDto>(item));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return Result.Failure("Inventory item not found");

        await _repository.DeleteAsync(item, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<PagedResult<InventoryItemDto>>> GetLowStockAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetLowStockAsync(warehouseId, cancellationToken);
        var dtoItems = _mapper.Map<List<InventoryItemDto>>(items);
        
        return Result<PagedResult<InventoryItemDto>>.Success(
            PagedResult<InventoryItemDto>.Create(
                dtoItems,
                dtoItems.Count,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<InventoryItemDto>>> GetOutOfStockAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetOutOfStockAsync(warehouseId, cancellationToken);
        var dtoItems = _mapper.Map<List<InventoryItemDto>>(items);
        
        return Result<PagedResult<InventoryItemDto>>.Success(
            PagedResult<InventoryItemDto>.Create(
                dtoItems,
                dtoItems.Count,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<int>> GetTotalStockValueAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        var value = await _repository.GetTotalStockValueAsync(warehouseId, cancellationToken);
        return Result<int>.Success(value);
    }

    public async Task<Result<Dictionary<string, int>>> GetStockByCategoryAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetStockByCategoryAsync(warehouseId, cancellationToken);
        return Result<Dictionary<string, int>>.Success(result);
    }

    public async Task<Result> AdjustStockAsync(string itemId, int quantity, string reason, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(itemId, cancellationToken);
        if (item == null)
            return Result.Failure("Inventory item not found");

        var oldQty = item.Qty;
        var newQty = item.Qty + quantity;
        
        if (newQty < 0)
            return Result.Failure("Insufficient stock for adjustment");

        item.Qty = newQty;
        item.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(item, cancellationToken);

        // Create stock movement
        var movement = new StockMovement
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            ItemId = item.Id,
            ItemName = item.Name,
            Sku = item.Sku,
            Type = "adjustment",
            Qty = quantity,
            From = "System",
            To = item.WarehouseId,
            Reason = reason,
            RefNumber = $"ADJ-{DateTime.UtcNow:yyyyMMddHHmmss}",
            Date = DateTime.UtcNow,
            Notes = $"Adjusted from {oldQty} to {newQty}"
        };
        await _movementRepository.AddAsync(movement, cancellationToken);

        // Create stock adjustment record
        var adjustment = new StockAdjustment
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            ItemId = item.Id,
            ItemName = item.Name,
            Sku = item.Sku,
            Location = item.Location,
            OldQty = oldQty,
            NewQty = newQty,
            Difference = quantity,
            Reason = reason,
            Remarks = $"Adjusted from {oldQty} to {newQty}",
            Date = DateTime.UtcNow
        };
        await _adjustmentRepository.AddAsync(adjustment, cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ReserveStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(itemId, cancellationToken);
        if (item == null)
            return Result.Failure("Inventory item not found");

        if (item.AvailableQty < quantity)
            return Result.Failure($"Insufficient available stock. Available: {item.AvailableQty}");

        item.Reserved += quantity;
        item.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(item, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> ReleaseReservedStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetByIdAsync(itemId, cancellationToken);
        if (item == null)
            return Result.Failure("Inventory item not found");

        if (item.Reserved < quantity)
            return Result.Failure($"Cannot release more than reserved. Reserved: {item.Reserved}");

        item.Reserved -= quantity;
        item.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(item, cancellationToken);
        return Result.Success();
    }
}