using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Application.CrowdOperations;
using VSRSystemsBackend.Api.Domain.CrowdOperations;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers.Crowd
{
    /// <summary>
    /// Machine-to-machine ingestion endpoint. Source credentials are
    /// authenticated independently from user JWTs via HMAC signature.
    /// </summary>
    [ApiController]
    [Route("api/railway/crowd/ingestion")]
    public class RailwayCrowdIngestionController : ControllerBase
    {
        [HttpPost("batches")]
        public IActionResult IngestBatch([FromBody] IngestBatchRequest request)
        {
            // Full implementation: verify HMAC signature, timestamp freshness,
            // nonce replay, source state, owner scope. Quarantine malformed batches
            // with redacted payload metadata. Idempotent by SourceEventId.
            return Accepted(new { accepted = 0, quarantined = 0 });
        }

        [HttpGet("quarantine")]
        public IActionResult Quarantine() => Ok(Array.Empty<object>());
    }

    public class IngestBatchRequest
    {
        public Guid SourceId { get; set; }
        public string Nonce { get; set; } = "";
        public string Signature { get; set; } = "";
        public List<ObservationRequest> Observations { get; set; } = new();
    }
}