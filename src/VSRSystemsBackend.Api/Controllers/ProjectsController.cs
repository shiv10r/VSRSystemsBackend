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
[Route("api/warehouse/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProjectRecordDto>>> Create([FromBody] CreateProjectRecordDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ProjectRecordDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<ProjectRecordDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectRecordDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<ProjectRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ProjectRecordDto>.Ok(result.Value!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<ProjectRecordDto>>>> GetAll(
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
            return BadRequest(ApiResponse<PagedResult<ProjectRecordDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<ProjectRecordDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<ProjectRecordDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<ProjectRecordDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<ProjectRecordDto>>.Ok(result.Value!));
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<ProjectRecordDto>>>> GetByWarehouse(
        string warehouseId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByWarehouseIdAsync(warehouseId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<ProjectRecordDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<ProjectRecordDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectRecordDto>>> Update(string id, [FromBody] UpdateProjectRecordDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ProjectRecordDto>.Fail(result.Error));

        return Ok(ApiResponse<ProjectRecordDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Project deleted successfully"));
    }
}