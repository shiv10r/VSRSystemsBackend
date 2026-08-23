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
[Route("api/jobs/candidates")]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _service;

    public CandidatesController(ICandidateService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> Create([FromBody] CreateCandidateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CandidateDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<CandidateDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CandidateDto>.Fail(result.Error));

        return Ok(ApiResponse<CandidateDto>.Ok(result.Value!));
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> GetByEmail(string email, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByEmailAsync(email, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CandidateDto>.Fail(result.Error));

        return Ok(ApiResponse<CandidateDto>.Ok(result.Value!));
    }

    [HttpGet("phone/{phone}")]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> GetByPhone(string phone, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByPhoneAsync(phone, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CandidateDto>.Fail(result.Error));

        return Ok(ApiResponse<CandidateDto>.Ok(result.Value!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<CandidateDto>>>> GetAll(
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
            return BadRequest(ApiResponse<PagedResult<CandidateDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<CandidateDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CandidateDto>>> Update(string id, [FromBody] UpdateCandidateDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CandidateDto>.Fail(result.Error));

        return Ok(ApiResponse<CandidateDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Candidate deleted successfully"));
    }
}