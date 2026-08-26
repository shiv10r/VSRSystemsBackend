using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Domain.Inspection.Enums;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

[ApiController]
[Authorize]
[Route("api/railway/inspections")]
public class RailwayInspectionsController(IRailwayScopeAccessor scopeAccessor) : ControllerBase
{
    [HttpPost("template/{templateVersion}/station/{stationId}")]
    public IActionResult CreateRun(string templateVersion, Guid stationId, [FromBody] CreateInspectionRunRequest request)
    {
        var scope = scopeAccessor.GetRequiredScope();
        scope.RequireDivision(request.DivisionId);
        return Ok(new { templateVersion, stationId, scope.OrganizationId, request.DivisionId });
    }

    [HttpPost("{runId}/findings")]
    public IActionResult SubmitFinding(Guid runId, [FromBody] SubmitFindingRequest request)
    {
        return Ok(new { runId, request.ItemId, request.Response });
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

public sealed record CreateInspectionRunRequest(Guid DivisionId);
public sealed record SubmitFindingRequest(string ItemId, string Response);
