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
[Route("api/jobs/companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _service;

    public CompaniesController(ICompanyService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CompanyDto>>> Create([FromBody] CreateCompanyDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CompanyDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<CompanyDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CompanyDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CompanyDto>.Fail(result.Error));

        return Ok(ApiResponse<CompanyDto>.Ok(result.Value!));
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ApiResponse<CompanyDto>>> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBySlugAsync(slug, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CompanyDto>.Fail(result.Error));

        return Ok(ApiResponse<CompanyDto>.Ok(result.Value!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<CompanyDto>>>> GetAll(
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
            return BadRequest(ApiResponse<PagedResult<CompanyDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<CompanyDto>>.Ok(result.Value!));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<PagedResult<CompanyDto>>>> GetActive(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _service.GetActiveCompaniesAsync(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<CompanyDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<CompanyDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CompanyDto>>> Update(string id, [FromBody] UpdateCompanyDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CompanyDto>.Fail(result.Error));

        return Ok(ApiResponse<CompanyDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Company deleted successfully"));
    }
}
