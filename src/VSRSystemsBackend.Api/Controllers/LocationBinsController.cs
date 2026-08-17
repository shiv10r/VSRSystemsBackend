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
[Route("api/warehouse/locations")]
public class LocationBinsController : ControllerBase
{
    private readonly ILocationBinService _service;

    public LocationBinsController(ILocationBinService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LocationBinDto>>> Create([FromBody] CreateLocationBinDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<LocationBinDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<LocationBinDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LocationBinDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<LocationBinDto>.Fail(result.Error));

        return Ok(ApiResponse<LocationBinDto>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<LocationBinDto>>>> GetByWarehouse(
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
            return BadRequest(ApiResponse<PagedResult<LocationBinDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<LocationBinDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}/active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LocationBinDto>>>> GetActiveByWarehouse(string warehouseId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetActiveByWarehouseIdAsync(warehouseId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<LocationBinDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<LocationBinDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<LocationBinDto>>> Update(string id, [FromBody] UpdateLocationBinDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<LocationBinDto>.Fail(result.Error));

        return Ok(ApiResponse<LocationBinDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Location deleted successfully"));
    }
}