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

public class SalesOrderService : ISalesOrderService
{
    private readonly ISalesOrderRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IMapper _mapper;

    public SalesOrderService(
        ISalesOrderRepository repository,
        ICustomerRepository customerRepository,
        IWarehouseRepository warehouseRepository,
        IInventoryRepository inventoryRepository,
        IMapper mapper)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _warehouseRepository = warehouseRepository;
        _inventoryRepository = inventoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<SalesOrderDto>> CreateAsync(CreateSalesOrderDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, cancellationToken);
        if (customer == null)
            return Result<SalesOrderDto>.Failure("Customer not found");

        var warehouse = await _warehouseRepository.GetByIdAsync(dto.WarehouseId, cancellationToken);
        if (warehouse == null)
            return Result<SalesOrderDto>.Failure("Warehouse not found");

        var order = _mapper.Map<SalesOrder>(dto);
        order.Id = Guid.NewGuid().ToString("N")[..20];
        order.OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        order.CustomerName = customer.Name;
        order.Status = "created";
        order.CreatedAt = DateTime.UtcNow;

        decimal subTotal = 0;
        decimal taxTotal = 0;
        decimal discountTotal = 0;

        foreach (var line in order.Lines)
        {
            line.Id = 0; // Auto-increment
            var item = await _inventoryRepository.GetByIdAsync(line.ItemId, cancellationToken);
            if (item != null)
            {
                line.ItemName = item.Name;
                line.Sku = item.Sku;
            }

            line.Total = line.Qty * line.Price * (1 - (decimal)line.DiscountPct / 100);
            subTotal += line.Qty * line.Price;
            taxTotal += line.Qty * line.Price * (decimal)line.TaxPct / 100;
            discountTotal += line.Qty * line.Price * (decimal)line.DiscountPct / 100;
        }

        order.SubTotal = subTotal;
        order.TaxTotal = taxTotal;
        order.DiscountTotal = discountTotal;
        order.GrandTotal = subTotal + taxTotal - discountTotal;

        await _repository.AddAsync(order, cancellationToken);
        return Result<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(order));
    }

    public async Task<Result<SalesOrderDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        if (order == null)
            return Result<SalesOrderDto>.Failure("Sales order not found");

        return Result<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(order));
    }

    public async Task<Result<SalesOrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByOrderNumberAsync(orderNumber, cancellationToken);
        if (order == null)
            return Result<SalesOrderDto>.Failure("Sales order not found");

        return Result<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(order));
    }

    public async Task<Result<PagedResult<SalesOrderDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(s => s.WarehouseId == warehouseId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(s => s.OrderNumber.Contains(request.SearchTerm) || s.CustomerName.Contains(request.SearchTerm));
        }

        var totalCount = await _repository.CountAsync(s => s.WarehouseId == warehouseId, cancellationToken);
        
        query = query.OrderByDescending(s => s.OrderDate);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<SalesOrderDto>>.Success(
            PagedResult<SalesOrderDto>.Create(
                _mapper.Map<List<SalesOrderDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<SalesOrderDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(s => s.CustomerId == customerId);

        var totalCount = await _repository.CountAsync(s => s.CustomerId == customerId, cancellationToken);
        
        query = query.OrderByDescending(s => s.OrderDate);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<SalesOrderDto>>.Success(
            PagedResult<SalesOrderDto>.Create(
                _mapper.Map<List<SalesOrderDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<SalesOrderDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query().Where(s => s.Status == status);

        var totalCount = await _repository.CountAsync(s => s.Status == status, cancellationToken);
        
        query = query.OrderByDescending(s => s.OrderDate);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<SalesOrderDto>>.Success(
            PagedResult<SalesOrderDto>.Create(
                _mapper.Map<List<SalesOrderDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<SalesOrderDto>> UpdateAsync(string id, UpdateSalesOrderDto dto, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        if (order == null)
            return Result<SalesOrderDto>.Failure("Sales order not found");

        if (order.Status != "created")
            return Result<SalesOrderDto>.Failure("Only created orders can be updated");

        _mapper.Map(dto, order);
        order.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(order, cancellationToken);
        return Result<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(order));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        if (order == null)
            return Result.Failure("Sales order not found");

        if (order.Status != "created")
            return Result.Failure("Only created orders can be deleted");

        await _repository.DeleteAsync(order, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<SalesOrderDto>> ConfirmAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        if (order == null)
            return Result<SalesOrderDto>.Failure("Sales order not found");

        if (order.Status != "created")
            return Result<SalesOrderDto>.Failure("Only created orders can be confirmed");

        order.Status = "confirmed";
        order.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(order, cancellationToken);
        return Result<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(order));
    }

    public async Task<Result<SalesOrderDto>> ReserveAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        if (order == null)
            return Result<SalesOrderDto>.Failure("Sales order not found");

        if (order.Status != "confirmed")
            return Result<SalesOrderDto>.Failure("Only confirmed orders can be reserved");

        // Reserve stock for each line item
        foreach (var line in order.Lines)
        {
            var item = await _inventoryRepository.GetByIdAsync(line.ItemId, cancellationToken);
            if (item == null || item.AvailableQty < line.Qty)
                return Result<SalesOrderDto>.Failure($"Insufficient stock for item {line.ItemName}");
        }

        foreach (var line in order.Lines)
        {
            await _inventoryRepository.ReserveStockAsync(line.ItemId, line.Qty, cancellationToken);
        }

        order.Status = "reserved";
        order.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(order, cancellationToken);
        return Result<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(order));
    }

    public async Task<Result<PagedResult<SalesOrderDto>>> GetOrdersForPickingAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var orders = await _repository.GetOrdersForPickingAsync(warehouseId, cancellationToken);
        var dtoItems = _mapper.Map<List<SalesOrderDto>>(orders);
        
        return Result<PagedResult<SalesOrderDto>>.Success(
            PagedResult<SalesOrderDto>.Create(
                dtoItems,
                dtoItems.Count,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<PagedResult<SalesOrderDto>>> GetOrdersForDispatchAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var orders = await _repository.GetOrdersForDispatchAsync(warehouseId, cancellationToken);
        var dtoItems = _mapper.Map<List<SalesOrderDto>>(orders);
        
        return Result<PagedResult<SalesOrderDto>>.Success(
            PagedResult<SalesOrderDto>.Create(
                dtoItems,
                dtoItems.Count,
                request.PageNumber,
                request.PageSize
            )
        );
    }
}