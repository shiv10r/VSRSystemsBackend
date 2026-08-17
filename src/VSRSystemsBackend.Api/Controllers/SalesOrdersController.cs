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
[Route("api/warehouse/sales-orders")]
public class SalesOrdersController : ControllerBase
{
    private readonly ISalesOrderService _service;

    public SalesOrdersController(ISalesOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> Create([FromBody] CreateSalesOrderDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<SalesOrderDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<SalesOrderDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<SalesOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<SalesOrderDto>.Ok(result.Value!));
    }

    [HttpGet("order-number/{orderNumber}")]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> GetByOrderNumber(string orderNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByOrderNumberAsync(orderNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<SalesOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<SalesOrderDto>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<SalesOrderDto>>>> GetByWarehouse(
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
            return BadRequest(ApiResponse<PagedResult<SalesOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<SalesOrderDto>>.Ok(result.Value!));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<SalesOrderDto>>>> GetByCustomer(
        string customerId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByCustomerIdAsync(customerId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<SalesOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<SalesOrderDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<SalesOrderDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<SalesOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<SalesOrderDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/for-picking")]
    public async Task<ActionResult<ApiResponse<PagedResult<SalesOrderDto>>>> GetForPicking(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetOrdersForPickingAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<SalesOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<SalesOrderDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/for-dispatch")]
    public async Task<ActionResult<ApiResponse<PagedResult<SalesOrderDto>>>> GetForDispatch(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetOrdersForDispatchAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<SalesOrderDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<SalesOrderDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> Update(string id, [FromBody] UpdateSalesOrderDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<SalesOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<SalesOrderDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Sales order deleted successfully"));
    }

    [HttpPost("{id}/confirm")]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> Confirm(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.ConfirmAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<SalesOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<SalesOrderDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/reserve")]
    public async Task<ActionResult<ApiResponse<SalesOrderDto>>> Reserve(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.ReserveAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<SalesOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<SalesOrderDto>.Ok(result.Value!));
    }
}