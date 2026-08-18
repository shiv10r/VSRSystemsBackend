using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobService _service;

    public JobsController(IJobService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<JobDto>>> Create([FromBody] CreateJobDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<JobDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<JobDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<JobDto>.Fail(result.Error));

        return Ok(ApiResponse<JobDto>.Ok(result.Value!));
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ApiResponse<JobDto>>> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBySlugAsync(slug, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<JobDto>.Fail(result.Error));

        return Ok(ApiResponse<JobDto>.Ok(result.Value!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<JobDto>>>> GetAll(
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
            return BadRequest(ApiResponse<PagedResult<JobDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobDto>>.Ok(result.Value!));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<PagedResult<JobDto>>>> GetActive(
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

        var result = await _service.GetActiveJobsAsync(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<JobDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobDto>>.Ok(result.Value!));
    }

    [HttpGet("company/{companyId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<JobDto>>>> GetByCompany(
        string companyId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _service.GetByCompanyIdAsync(companyId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<JobDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobDto>>.Ok(result.Value!));
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<ApiResponse<PagedResult<JobDto>>>> GetByCategory(
        string category,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _service.GetByCategoryAsync(category, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<JobDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobDto>>.Ok(result.Value!));
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<PagedResult<JobDto>>>> Search(
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _service.SearchAsync(searchTerm, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<JobDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<JobDto>>> Update(string id, [FromBody] UpdateJobDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobDto>.Fail(result.Error));

        return Ok(ApiResponse<JobDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Job deleted successfully"));
    }

    [HttpPost("{id}/publish")]
    public async Task<ActionResult<ApiResponse<JobDto>>> Publish(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.PublishAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobDto>.Fail(result.Error));

        return Ok(ApiResponse<JobDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/close")]
    public async Task<ActionResult<ApiResponse<JobDto>>> Close(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.CloseAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobDto>.Fail(result.Error));

        return Ok(ApiResponse<JobDto>.Ok(result.Value!));
    }
}
