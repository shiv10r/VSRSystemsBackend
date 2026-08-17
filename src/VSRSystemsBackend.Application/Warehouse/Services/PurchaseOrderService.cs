using System;
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

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _repository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IMapper _mapper;

    public PurchaseOrderService(
        IPurchaseOrderRepository repository,
        ISupplierRepository supplierRepository,
        IWarehouseRepository warehouseRepository,
        IInventoryRepository inventoryRepository,
        IMapper mapper)
    {
        _repository = repository;
        _supplierRepository = supplierRepository;
        _warehouseRepository = warehouseRepository;
        _inventoryRepository = inventoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<PurchaseOrderDto>> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId, cancellationToken);
        if (supplier == null)
            return Result<PurchaseOrderDto>.Failure("Supplier not found");

        var warehouse = await _warehouseRepository.GetByIdAsync(dto.WarehouseId, cancellationToken);
        if (warehouse == null)
            return Result<PurchaseOrderDto>.Failure("Warehouse not found");

        var po = _mapper.Map<PurchaseOrder>(dto);
        po.Id = Guid.NewGuid().ToString("N")[..20];
        po.PoNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        po.SupplierName = supplier.Name;
        po.Status = "draft";
        po.CreatedAt = DateTime.UtcNow;

        foreach (var line in po.Lines)
        {
            line.Id = 0; // Auto-increment
            var item = await _inventoryRepository.GetByIdAsync(line.ItemId, cancellationToken);
            if (item != null)
            {
                line.ItemName = item.Name;
            }
        }

        po.Total = po.Lines.Sum(l => l.Qty * l.UnitPrice);

        await _repository.AddAsync(po, cancellationToken);
        return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(po));
    }

    public async Task<Result<PurchaseOrderDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var po = await _repository.GetByIdAsync(id, cancellationToken);
        if (po == null)
            return Result<PurchaseOrderDto>.Failure("Purchase order not found");

        return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(po));
    }

    public async Task<Result<PurchaseOrderDto>> GetByPoNumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        var po = await _repository.GetByPoNumberAsync(poNumber, cancellationToken);
        if (po == null)
            return Result<PurchaseOrderDto>.Failure("Purchase order not found");

        return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(po));
    }

    public async Task<Result<PagedResult<PurchaseOrderDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(p => p.WarehouseId == warehouseId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(p => p.PoNumber.Contains(request.SearchTerm) || p.SupplierName.Contains(request.SearchTerm));
        }

        var totalCount = await _repository.CountAsync(p => p.WarehouseId == warehouseId, cancellationToken);
        
        query = query.OrderByDescending(p => p.Date);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<PurchaseOrderDto>>.Success(
            PagedResult<PurchaseOrderDto>.Create(
                _mapper.Map<List<PurchaseOrderDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<PurchaseOrderDto>>> GetBySupplierIdAsync(string supplierId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(p => p.SupplierId == supplierId);

        var totalCount = await _repository.CountAsync(p => p.SupplierId == supplierId, cancellationToken);
        
        query = query.OrderByDescending(p => p.Date);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<PurchaseOrderDto>>.Success(
            PagedResult<PurchaseOrderDto>.Create(
                _mapper.Map<List<PurchaseOrderDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<PurchaseOrderDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(p => p.Status == status);

        var totalCount = await _repository.CountAsync(p => p.Status == status, cancellationToken);
        
        query = query.OrderByDescending(p => p.Date);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<PurchaseOrderDto>>.Success(
            PagedResult<PurchaseOrderDto>.Create(
                _mapper.Map<List<PurchaseOrderDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PurchaseOrderDto>> UpdateAsync(string id, UpdatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var po = await _repository.GetByIdAsync(id, cancellationToken);
        if (po == null)
            return Result<PurchaseOrderDto>.Failure("Purchase order not found");

        if (po.Status != "draft")
            return Result<PurchaseOrderDto>.Failure("Only draft purchase orders can be updated");

        _mapper.Map(dto, po);
        po.UpdatedAt = DateTime.UtcNow;

        po.Total = po.Lines.Sum(l => l.Qty * l.UnitPrice);

        await _repository.UpdateAsync(po, cancellationToken);
        return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(po));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var po = await _repository.GetByIdAsync(id, cancellationToken);
        if (po == null)
            return Result.Failure("Purchase order not found");

        if (po.Status != "draft")
            return Result.Failure("Only draft purchase orders can be deleted");

        await _repository.DeleteAsync(po, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<PurchaseOrderDto>> SubmitAsync(string id, CancellationToken cancellationToken = default)
    {
        var po = await _repository.GetByIdAsync(id, cancellationToken);
        if (po == null)
            return Result<PurchaseOrderDto>.Failure("Purchase order not found");

        if (po.Status != "draft")
            return Result<PurchaseOrderDto>.Failure("Only draft purchase orders can be submitted");

        po.Status = "submitted";
        po.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(po, cancellationToken);
        return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(po));
    }

    public async Task<Result<PurchaseOrderDto>> ApproveAsync(string id, CancellationToken cancellationToken = default)
    {
        var po = await _repository.GetByIdAsync(id, cancellationToken);
        if (po == null)
            return Result<PurchaseOrderDto>.Failure("Purchase order not found");

        if (po.Status != "submitted")
            return Result<PurchaseOrderDto>.Failure("Only submitted purchase orders can be approved");

        po.Status = "approved";
        po.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(po, cancellationToken);
        return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(po));
    }

    public async Task<Result<PurchaseOrderDto>> ReceiveAsync(string id, ReceivePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var po = await _repository.GetByIdAsync(id, cancellationToken);
        if (po == null)
            return Result<PurchaseOrderDto>.Failure("Purchase order not found");

        if (po.Status != "approved" && po.Status != "partial")
            return Result<PurchaseOrderDto>.Failure("Only approved or partially received purchase orders can be received");

        // This would typically create a GRN record - simplified for now
        po.Status = dto.IsComplete ? "received" : "partial";
        po.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(po, cancellationToken);
        return Result<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(po));
    }

    public async Task<Result<PagedResult<PurchaseOrderDto>>> GetPendingReceivingAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var pos = await _repository.GetPendingReceivingAsync(warehouseId, cancellationToken);
        var dtoItems = _mapper.Map<List<PurchaseOrderDto>>(pos);
        
        return Result<PagedResult<PurchaseOrderDto>>.Success(
            PagedResult<PurchaseOrderDto>.Create(
                dtoItems,
                dtoItems.Count,
                request.PageNumber,
                request.PageSize
            )
        );
    }
}