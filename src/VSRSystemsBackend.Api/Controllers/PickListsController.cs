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
[Route("api/warehouse/pick-lists")]
public class PickListsController : ControllerBase
{
    private readonly IPickListService _service;

    public PickListsController(IPickListService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PickListDto>>> Create([FromBody] CreatePickListDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PickListDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<PickListDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PickListDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PickListDto>.Fail(result.Error));

        return Ok(ApiResponse<PickListDto>.Ok(result.Value!));
    }

    [HttpGet("pick-number/{pickNumber}")]
    public async Task<ActionResult<ApiResponse<PickListDto>>> GetByPickNumber(string pickNumber, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByPickNumberAsync(pickNumber, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PickListDto>.Fail(result.Error));

        return Ok(ApiResponse<PickListDto>.Ok(result.Value!));
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<PickListDto>>>> GetByOrder(
        string orderId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByOrderIdAsync(orderId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PickListDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PickListDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<PickListDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PickListDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PickListDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PickListDto>>> Update(string id, [FromBody] UpdatePickListDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PickListDto>.Fail(result.Error));

        return Ok(ApiResponse<PickListDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Pick list deleted successfully"));
    }

    [HttpPost("{id}/start-picking")]
    public async Task<ActionResult<ApiResponse<PickListDto>>> StartPicking(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.StartPickingAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PickListDto>.Fail(result.Error));

        return Ok(ApiResponse<PickListDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/complete-picking")]
    public async Task<ActionResult<ApiResponse<PickListDto>>> CompletePicking(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.CompletePickingAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PickListDto>.Fail(result.Error));

        return Ok(ApiResponse<PickListDto>.Ok(result.Value!));
    }
}