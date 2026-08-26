using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Domain.CrowdOperations;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers.Crowd
{
    [ApiController]
    [Authorize]
    [Route("api/railway/crowd")]
    public class RailwayCrowdController : ControllerBase
    {
        [HttpGet("observations")]
        public IActionResult Observations([FromQuery] Guid? stationId) => Ok(new { stationId, items = Array.Empty<object>() });

        [HttpPost("observations")]
        public IActionResult SubmitObservation([FromBody] ObservationRequest request) =>
            Ok(new { accepted = true, request.SourceEventId });

        [HttpGet("alerts")]
        public IActionResult Alerts() => Ok(Array.Empty<CrowdAlert>());

        [HttpPost("alerts/{alertId}/acknowledge")]
        public IActionResult Acknowledge(Guid alertId) => Ok(new { alertId, acknowledged = true });

        [HttpGet("sources")]
        public IActionResult Sources() => Ok(Array.Empty<object>());

        [HttpGet("incidents")]
        public IActionResult Incidents() => Ok(Array.Empty<CrowdIncident>());

        [HttpPost("incidents")]
        public IActionResult OpenIncident([FromBody] IncidentRequest request) =>
            Ok(new { id = Guid.NewGuid(), request.StationId, request.Title });
    }

    public class ObservationRequest
    {
        public string SourceEventId { get; set; } = "";
        public Guid StationId { get; set; }
        public Guid ZoneId { get; set; }
        public int Count { get; set; }
        public decimal Confidence { get; set; }
    }

    public class IncidentRequest
    {
        public Guid StationId { get; set; }
        public string Title { get; set; } = "";
    }
}
