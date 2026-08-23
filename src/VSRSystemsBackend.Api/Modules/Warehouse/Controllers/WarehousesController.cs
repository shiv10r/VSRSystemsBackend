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
[Route("api/warehouse/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _service;

    public WarehousesController(IWarehouseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> Create([FromBody] CreateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<WarehouseDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<WarehouseDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<WarehouseDto>.Fail(result.Error));

        return Ok(ApiResponse<WarehouseDto>.Ok(result.Value!));
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> GetByCode(string code, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByCodeAsync(code, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<WarehouseDto>.Fail(result.Error));

        return Ok(ApiResponse<WarehouseDto>.Ok(result.Value!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<WarehouseDto>>>> GetAll(
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

        var result = await _service.GetAllAsync(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<WarehouseDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<WarehouseDto>>.Ok(result.Value!));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WarehouseDto>>>> GetActive(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetActiveWarehousesAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<WarehouseDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<WarehouseDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<WarehouseDto>>> Update(string id, [FromBody] UpdateWarehouseDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<WarehouseDto>.Fail(result.Error));

        return Ok(ApiResponse<WarehouseDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Warehouse deleted successfully"));
    }
}