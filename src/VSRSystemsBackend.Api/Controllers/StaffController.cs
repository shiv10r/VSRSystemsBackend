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
[Route("api/warehouse/staff")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _service;

    public StaffController(IStaffService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StaffMemberDto>>> Create([FromBody] CreateStaffMemberDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StaffMemberDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<StaffMemberDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StaffMemberDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<StaffMemberDto>.Fail(result.Error));

        return Ok(ApiResponse<StaffMemberDto>.Ok(result.Value!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<StaffMemberDto>>>> GetAll(
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
            return BadRequest(ApiResponse<PagedResult<StaffMemberDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<StaffMemberDto>>.Ok(result.Value!));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StaffMemberDto>>>> GetActive(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetActiveStaffAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<StaffMemberDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<StaffMemberDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<StaffMemberDto>>> Update(string id, [FromBody] UpdateStaffMemberDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<StaffMemberDto>.Fail(result.Error));

        return Ok(ApiResponse<StaffMemberDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Staff member deleted successfully"));
    }
}