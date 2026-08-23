using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/warehouse/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _service;

    public InventoryController(IInventoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<InventoryItemDto>>> Create([FromBody] CreateInventoryItemDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<InventoryItemDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<InventoryItemDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<InventoryItemDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<InventoryItemDto>.Fail(result.Error));

        return Ok(ApiResponse<InventoryItemDto>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<InventoryItemDto>>>> GetByWarehouse(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            SortBy = sortBy,
            SortDescending = sortDescending
        };

        var result = await _service.GetByWarehouseIdAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<InventoryItemDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<InventoryItemDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/sku/{sku}")]
    public async Task<ActionResult<ApiResponse<InventoryItemDto>>> GetBySku(string warehouseId, string sku, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBySkuAsync(sku, warehouseId, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<InventoryItemDto>.Fail(result.Error));

        return Ok(ApiResponse<InventoryItemDto>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/low-stock")]
    public async Task<ActionResult<ApiResponse<PagedResult<InventoryItemDto>>>> GetLowStock(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetLowStockAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<InventoryItemDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<InventoryItemDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/out-of-stock")]
    public async Task<ActionResult<ApiResponse<PagedResult<InventoryItemDto>>>> GetOutOfStock(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetOutOfStockAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<InventoryItemDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<InventoryItemDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/total-value")]
    public async Task<ActionResult<ApiResponse<int>>> GetTotalStockValue(string warehouseId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetTotalStockValueAsync(warehouseId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<int>.Fail(result.Error));

        return Ok(ApiResponse<int>.Ok(result.Value));
    }

    [HttpGet("warehouse/{warehouseId}/by-category")]
    public async Task<ActionResult<ApiResponse<Dictionary<string, int>>>> GetStockByCategory(string warehouseId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetStockByCategoryAsync(warehouseId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<Dictionary<string, int>>.Fail(result.Error));

        return Ok(ApiResponse<Dictionary<string, int>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<InventoryItemDto>>> Update(string id, [FromBody] UpdateInventoryItemDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<InventoryItemDto>.Fail(result.Error));

        return Ok(ApiResponse<InventoryItemDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Inventory item deleted successfully"));
    }

    [HttpPost("{itemId}/adjust")]
    public async Task<ActionResult<ApiResponse>> AdjustStock(string itemId, [FromBody] StockAdjustmentRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AdjustStockAsync(itemId, request.Quantity, request.Reason, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Stock adjusted successfully"));
    }

    [HttpPost("{itemId}/reserve")]
    public async Task<ActionResult<ApiResponse>> ReserveStock(string itemId, [FromBody] StockReservationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.ReserveStockAsync(itemId, request.Quantity, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Stock reserved successfully"));
    }

    [HttpPost("{itemId}/release-reserved")]
    public async Task<ActionResult<ApiResponse>> ReleaseReservedStock(string itemId, [FromBody] StockReservationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.ReleaseReservedStockAsync(itemId, request.Quantity, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Reserved stock released successfully"));
    }
}

public class StockAdjustmentRequest
{
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class StockReservationRequest
{
    public int Quantity { get; set; }
}