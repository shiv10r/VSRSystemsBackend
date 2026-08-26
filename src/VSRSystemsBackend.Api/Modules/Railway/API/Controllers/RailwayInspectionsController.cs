using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Domain.Inspection;
using VSRSystemsBackend.Api.Domain.Inspection.Enums;

[ApiController]
[Route("api/railway/inspections")]
public class RailwayInspectionsController : ControllerBase
{
    [HttpPost("template/{templateVersion}/station/{stationId}")]
    public IActionResult CreateRun(string templateVersion, Guid stationId, [FromQuery] Guid organizationId, [FromQuery] Guid divisionId)
    {
        return Ok(new { templateVersion, stationId, organizationId, divisionId });
    }

    [HttpPost("{runId}/findings")]
    public IActionResult SubmitFinding(Guid runId, [FromBody] string itemId, [FromBody] string response)
    {
        return Ok(new { runId, itemId, response });
    }

    [HttpPost("{runId}/defects")]
    public IActionResult RaiseDefect(Guid runId, [FromBody] DefectInputDto input)
    {
        return Ok(input);
    }

    public class DefectInputDto
    {
        public string Description { get; set; } = "";
        public DefectSeverity Severity { get; set; }
    }
}