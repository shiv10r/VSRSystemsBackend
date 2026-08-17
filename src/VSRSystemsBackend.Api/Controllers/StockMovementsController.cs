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
[Route("api/warehouse/stock-movements")]
public class StockMovementsController : ControllerBase
{
    private readonly IStockMovementService _service;

    public StockMovementsController(IStockMovementService service)
    {
        _service = service;
    }

    [HttpGet("item/{itemId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockMovementDto>>>> GetByItem(
        string itemId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByItemIdAsync(itemId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockMovementDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockMovementDto>>.Ok(result.Value!));
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockMovementDto>>>> GetByDateRange(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByDateRangeAsync(from, to, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockMovementDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockMovementDto>>.Ok(result.Value!));
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<ApiResponse<PagedResult<StockMovementDto>>>> GetByType(
        string type,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByTypeAsync(type, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<StockMovementDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StockMovementDto>>.Ok(result.Value!));
    }
}