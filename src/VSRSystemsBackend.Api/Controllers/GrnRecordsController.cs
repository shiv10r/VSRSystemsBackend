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
[Route("api/warehouse/grns")]
public class GrnRecordsController : ControllerBase
{
    private readonly IGrnService _service;

    public GrnRecordsController(IGrnService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<GrnRecordDto>>> Create([FromBody] CreateGrnRecordDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<GrnRecordDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<GrnRecordDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GrnRecordDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<GrnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<GrnRecordDto>.Ok(result.Value!));
    }

    [HttpGet("grn-number/{grnNumber}")]
    public async Task<ActionResult<ApiResponse<GrnRecordDto>>> GetByGrnNumber(string grnNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByGrnNumberAsync(grnNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<GrnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<GrnRecordDto>.Ok(result.Value!));
    }

    [HttpGet("po/{poId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<GrnRecordDto>>>> GetByPo(
        string poId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByPoIdAsync(poId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<GrnRecordDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<GrnRecordDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<GrnRecordDto>>>> GetByWarehouse(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByWarehouseIdAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<GrnRecordDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<GrnRecordDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<GrnRecordDto>>> Update(string id, [FromBody] UpdateGrnRecordDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<GrnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<GrnRecordDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("GRN record deleted successfully"));
    }
}