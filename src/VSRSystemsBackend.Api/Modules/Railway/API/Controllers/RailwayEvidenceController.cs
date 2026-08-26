using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Storage;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

public sealed record FinalizeRailwayEvidenceRequest(string Sha256);

[ApiController]
[Authorize]
[Route("api/railway/evidence")]
public sealed class RailwayEvidenceController(
    IRailwayScopeAccessor scopeAccessor,
    IRailwayEvidenceService evidenceService) : ControllerBase
{
    [HttpPost("uploads", Name = "railway.evidence.initiate")]
    public async Task<ActionResult<RailwayEvidenceUpload>> Initiate(
        InitiateRailwayEvidenceRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        try
        {
            return Ok(await evidenceService.InitiateAsync(scopeAccessor.GetRequiredScope(), request, cancellationToken));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException exception)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: exception.Message);
        }
    }

    [HttpPost("{evidenceId:guid}/finalize", Name = "railway.evidence.finalize")]
    public async Task<IActionResult> Finalize(
        Guid evidenceId,
        FinalizeRailwayEvidenceRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        try
        {
            await evidenceService.FinalizeAsync(scopeAccessor.GetRequiredScope(), evidenceId, request.Sha256, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException exception)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: exception.Message);
        }
    }
}
