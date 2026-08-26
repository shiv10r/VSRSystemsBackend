using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Domain.Inspection.Enums;

[ApiController]
[Authorize]
[Route("api/railway/defects")]
public class RailwayDefectsController : ControllerBase
{
    [HttpGet]
    public IActionResult List() => Ok("Defects list");

    [HttpPost]
    public IActionResult Create([FromBody] CreateDefectRequest request) => Ok(request);

    [HttpPatch("{defectId}")]
    public IActionResult Resolve(Guid defectId, [FromBody] ResolveDefectRequest request) => Ok(new { defectId, request.Accepted, request.Reason });
}

public sealed record CreateDefectRequest(string Description, DefectSeverity Severity);
public sealed record ResolveDefectRequest(bool Accepted, string Reason);
