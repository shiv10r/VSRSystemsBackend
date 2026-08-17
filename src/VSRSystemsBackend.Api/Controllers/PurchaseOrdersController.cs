using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/warehouse/purchase-orders")]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _service;

    public PurchaseOrdersController(IPurchaseOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> Create([FromBody] CreatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PurchaseOrderDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<PurchaseOrderDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PurchaseOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<PurchaseOrderDto>.Ok(result.Value!));
    }

    [HttpGet("po-number/{poNumber}")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> GetByPoNumber(string poNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByPoNumberAsync(poNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PurchaseOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<PurchaseOrderDto>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<PurchaseOrderDto>>>> GetByWarehouse(
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
            return BadRequest(ApiResponse<PagedResult<PurchaseOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PurchaseOrderDto>>.Ok(result.Value!));
    }

    [HttpGet("supplier/{supplierId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<PurchaseOrderDto>>>> GetBySupplier(
        string supplierId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetBySupplierIdAsync(supplierId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PurchaseOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PurchaseOrderDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<PurchaseOrderDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PurchaseOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PurchaseOrderDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/pending-receiving")]
    public async Task<ActionResult<ApiResponse<PagedResult<PurchaseOrderDto>>>> GetPendingReceiving(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetPendingReceivingAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PurchaseOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PurchaseOrderDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> Update(string id, [FromBody] UpdatePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PurchaseOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<PurchaseOrderDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Purchase order deleted successfully"));
    }

    [HttpPost("{id}/submit")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> Submit(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.SubmitAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PurchaseOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<PurchaseOrderDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/approve")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> Approve(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.ApproveAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PurchaseOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<PurchaseOrderDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/receive")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderDto>>> Receive(string id, [FromBody] ReceivePurchaseOrderDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.ReceiveAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PurchaseOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<PurchaseOrderDto>.Ok(result.Value!));
    }
}