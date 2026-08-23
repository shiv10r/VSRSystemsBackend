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
[Route("api/jobs/applications")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _service;

    public JobApplicationsController(IJobApplicationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> Create([FromBody] CreateJobApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<JobApplicationDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return Ok(ApiResponse<JobApplicationDto>.Ok(result.Value!));
    }

    [HttpGet("job/{jobId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<JobApplicationDto>>>> GetByJob(
        string jobId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _service.GetByJobIdAsync(jobId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<JobApplicationDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobApplicationDto>>.Ok(result.Value!));
    }

    [HttpGet("candidate/{candidateId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<JobApplicationDto>>>> GetByCandidate(
        string candidateId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _service.GetByCandidateIdAsync(candidateId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<JobApplicationDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobApplicationDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<JobApplicationDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<JobApplicationDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<JobApplicationDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> Update(string id, [FromBody] UpdateJobApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return Ok(ApiResponse<JobApplicationDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Job application deleted successfully"));
    }

    [HttpPost("{id}/screen")]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> Screen(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.ScreenAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return Ok(ApiResponse<JobApplicationDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/interview")]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> Interview(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.InterviewAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return Ok(ApiResponse<JobApplicationDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/offer")]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> Offer(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.OfferAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return Ok(ApiResponse<JobApplicationDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/hire")]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> Hire(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.HireAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return Ok(ApiResponse<JobApplicationDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/reject")]
    public async Task<ActionResult<ApiResponse<JobApplicationDto>>> Reject(string id, [FromQuery] string? reason = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.RejectAsync(id, reason ?? string.Empty, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<JobApplicationDto>.Fail(result.Error));

        return Ok(ApiResponse<JobApplicationDto>.Ok(result.Value!));
    }
}
