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
[Route("api/jobs/screening-questions")]
public class ScreeningQuestionsController : ControllerBase
{
    private readonly IScreeningQuestionService _service;

    public ScreeningQuestionsController(IScreeningQuestionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ScreeningQuestionDto>>> Create([FromBody] CreateScreeningQuestionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ScreeningQuestionDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetByJob), new { jobId = result.Value!.JobId }, ApiResponse<ScreeningQuestionDto>.Ok(result.Value));
    }

    [HttpGet("job/{jobId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<ScreeningQuestionDto>>>> GetByJob(
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
            return BadRequest(ApiResponse<PagedResult<ScreeningQuestionDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<ScreeningQuestionDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ScreeningQuestionDto>>> Update(string id, [FromBody] UpdateScreeningQuestionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ScreeningQuestionDto>.Fail(result.Error));

        return Ok(ApiResponse<ScreeningQuestionDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Screening question deleted successfully"));
    }
}