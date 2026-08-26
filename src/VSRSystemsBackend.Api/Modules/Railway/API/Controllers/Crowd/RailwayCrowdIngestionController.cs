using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers.Crowd;

/// <summary>Machine-to-machine aggregate ingestion authenticated independently from user JWTs.</summary>
[ApiController]
[Route("api/railway/crowd/ingestion")]
public sealed class RailwayCrowdIngestionController(
    CrowdIngestionService ingestionService,
    RailwayDbContext dbContext,
    IRailwayScopeAccessor scopeAccessor) : ControllerBase
{
    [HttpPost("batches", Name = "railway.crowd.ingestion.batch")]
    [RequestSizeLimit(2_000_000)]
    public async Task<IActionResult> IngestBatch(
        [FromHeader(Name = "X-Railway-Source-Id")] Guid sourceId,
        [FromHeader(Name = "X-Railway-Timestamp")] long timestamp,
        [FromHeader(Name = "X-Railway-Nonce")] string nonce,
        [FromHeader(Name = "X-Railway-Signature")] string signature,
        CancellationToken cancellationToken)
    {
        if (sourceId == Guid.Empty || timestamp <= 0 || string.IsNullOrWhiteSpace(nonce) || string.IsNullOrWhiteSpace(signature))
            return BadRequest(new { code = "missing_authentication_headers" });
        await using var stream = new MemoryStream();
        await Request.Body.CopyToAsync(stream, cancellationToken);
        var result = await ingestionService.IngestAsync(sourceId, DateTimeOffset.FromUnixTimeSeconds(timestamp), nonce,
            signature, stream.ToArray(), cancellationToken);
        if (result.Accepted) return Accepted(new { accepted = result.AcceptedCount, duplicates = result.DuplicateCount });
        if (result.FailureCode == "replayed_nonce") return Conflict(new { code = result.FailureCode });
        if (result.FailureCode is "invalid_signature" or "expired_timestamp" or "source_disabled" or "source_not_found")
            return Unauthorized(new { code = "source_authentication_failed" });
        return BadRequest(new { code = result.FailureCode });
    }

    [Authorize]
    [HttpGet("quarantine", Name = "railway.crowd.ingestion.quarantine")]
    public async Task<IActionResult> Quarantine(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope();
        scope.RequirePermission("railway.crowd.manage");
        var items = await dbContext.CrowdQuarantine.AsNoTracking()
            .Where(item => item.DivisionId == null || scope.DivisionIds.Contains(item.DivisionId.Value))
            .OrderByDescending(item => item.CreatedAt)
            .Take(100)
            .Select(item => new { item.Id, item.SourceId, item.Reason, item.PayloadHash, item.CreatedAt })
            .ToArrayAsync(cancellationToken);
        return Ok(items);
    }
}
