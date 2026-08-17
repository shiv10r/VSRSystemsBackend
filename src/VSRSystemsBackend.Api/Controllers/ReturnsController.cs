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
[Route("api/warehouse/returns")]
public class ReturnsController : ControllerBase
{
    private readonly IReturnService _service;

    public ReturnsController(IReturnService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReturnRecordDto>>> Create([FromBody] CreateReturnRecordDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReturnRecordDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<ReturnRecordDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ReturnRecordDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<ReturnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ReturnRecordDto>.Ok(result.Value!));
    }

    [HttpGet("return-number/{returnNumber}")]
    public async Task<ActionResult<ApiResponse<ReturnRecordDto>>> GetByReturnNumber(string returnNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByReturnNumberAsync(returnNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<ReturnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ReturnRecordDto>.Ok(result.Value!));
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<ApiResponse<PagedResult<ReturnRecordDto>>>> GetByType(
        string type,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByTypeAsync(type, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<ReturnRecordDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<ReturnRecordDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<ReturnRecordDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<ReturnRecordDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<ReturnRecordDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ReturnRecordDto>>> Update(string id, [FromBody] UpdateReturnRecordDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReturnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ReturnRecordDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Return deleted successfully"));
    }

    [HttpPost("{id}/receive")]
    public async Task<ActionResult<ApiResponse<ReturnRecordDto>>> Receive(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.ReceiveAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReturnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ReturnRecordDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/inspect")]
    public async Task<ActionResult<ApiResponse<ReturnRecordDto>>> Inspect(string id, [FromBody] InspectReturnDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.InspectAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReturnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ReturnRecordDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<ApiResponse<ReturnRecordDto>>> Complete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.CompleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReturnRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ReturnRecordDto>.Ok(result.Value!));
    }
}