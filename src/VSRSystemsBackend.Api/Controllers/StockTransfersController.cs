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
[Route("api/warehouse/stock-transfers")]
public class StockTransfersController : ControllerBase
{
    private readonly IStockTransferService _service;

    public StockTransfersController(IStockTransferService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StockTransferDto>>> Create([FromBody] CreateStockTransferDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockTransferDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<StockTransferDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StockTransferDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<StockTransferDto>.Fail(result.Error));

        return Ok(ApiResponse<StockTransferDto>.Ok(result.Value!));
    }

    [HttpGet("transfer-number/{transferNumber}")]
    public async Task<ActionResult<ApiResponse<StockTransferDto>>> GetByTransferNumber(string transferNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByTransferNumberAsync(transferNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<StockTransferDto>.Fail(result.Error));

        return Ok(ApiResponse<StockTransferDto>.Ok(result.Value!));
    }

    [HttpGet("from-warehouse/{fromWarehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockTransferDto>>>> GetByFromWarehouse(
        string fromWarehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByFromWarehouseAsync(fromWarehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockTransferDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockTransferDto>>.Ok(result.Value!));
    }

    [HttpGet("to-warehouse/{toWarehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockTransferDto>>>> GetByToWarehouse(
        string toWarehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByToWarehouseAsync(toWarehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockTransferDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockTransferDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockTransferDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockTransferDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockTransferDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StockTransferDto>>> Update(string id, [FromBody] UpdateStockTransferDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockTransferDto>.Fail(result.Error));

        return Ok(ApiResponse<StockTransferDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Stock transfer deleted successfully"));
    }

    [HttpPost("{id}/dispatch")]
    public async Task<ActionResult<ApiResponse<StockTransferDto>>> Dispatch(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DispatchAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockTransferDto>.Fail(result.Error));

        return Ok(ApiResponse<StockTransferDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/receive")]
    public async Task<ActionResult<ApiResponse<StockTransferDto>>> Receive(string id, [FromBody] ReceiveStockTransferDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.ReceiveAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockTransferDto>.Fail(result.Error));

        return Ok(ApiResponse<StockTransferDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<ApiResponse<StockTransferDto>>> Complete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.CompleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StockTransferDto>.Fail(result.Error));

        return Ok(ApiResponse<StockTransferDto>.Ok(result.Value!));
    }
}