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
[Route("api/warehouse/dispatches")]
public class DispatchesController : ControllerBase
{
    private readonly IDispatchService _service;

    public DispatchesController(IDispatchService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DispatchDto>>> Create([FromBody] CreateDispatchDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<DispatchDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<DispatchDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DispatchDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<DispatchDto>.Fail(result.Error));

        return Ok(ApiResponse<DispatchDto>.Ok(result.Value!));
    }

    [HttpGet("dispatch-number/{dispatchNumber}")]
    public async Task<ActionResult<ApiResponse<DispatchDto>>> GetByDispatchNumber(string dispatchNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByDispatchNumberAsync(dispatchNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<DispatchDto>.Fail(result.Error));

        return Ok(ApiResponse<DispatchDto>.Ok(result.Value!));
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<DispatchDto>>>> GetByOrder(
        string orderId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByOrderIdAsync(orderId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<DispatchDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<DispatchDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<DispatchDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<DispatchDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<DispatchDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DispatchDto>>> Update(string id, [FromBody] UpdateDispatchDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<DispatchDto>.Fail(result.Error));

        return Ok(ApiResponse<DispatchDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Dispatch deleted successfully"));
    }

    [HttpPost("{id}/dispatch")]
    public async Task<ActionResult<ApiResponse<DispatchDto>>> Dispatch(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DispatchAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<DispatchDto>.Fail(result.Error));

        return Ok(ApiResponse<DispatchDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<ApiResponse<DispatchDto>>> Complete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.CompleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<DispatchDto>.Fail(result.Error));

        return Ok(ApiResponse<DispatchDto>.Ok(result.Value!));
    }
}