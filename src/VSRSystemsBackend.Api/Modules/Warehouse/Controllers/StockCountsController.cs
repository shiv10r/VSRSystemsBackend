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
[Route("api/warehouse/stock-counts")]
public class StockCountsController : ControllerBase
{
    private readonly IStockCountService _service;

    public StockCountsController(IStockCountService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StockCountDto>>> Create([FromBody] CreateStockCountDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockCountDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<StockCountDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StockCountDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<StockCountDto>.Fail(result.Error));

        return Ok(ApiResponse<StockCountDto>.Ok(result.Value!));
    }

    [HttpGet("count-number/{countNumber}")]
    public async Task<ActionResult<ApiResponse<StockCountDto>>> GetByCountNumber(string countNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByCountNumberAsync(countNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<StockCountDto>.Fail(result.Error));

        return Ok(ApiResponse<StockCountDto>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockCountDto>>>> GetByWarehouse(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByWarehouseIdAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockCountDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockCountDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockCountDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockCountDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockCountDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StockCountDto>>> Update(string id, [FromBody] UpdateStockCountDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockCountDto>.Fail(result.Error));

        return Ok(ApiResponse<StockCountDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Stock count deleted successfully"));
    }

    [HttpPost("{id}/approve")]
    public async Task<ActionResult<ApiResponse<StockCountDto>>> Approve(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.ApproveAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockCountDto>.Fail(result.Error));

        return Ok(ApiResponse<StockCountDto>.Ok(result.Value!));
    }
}