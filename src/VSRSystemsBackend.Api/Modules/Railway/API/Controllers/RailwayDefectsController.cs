using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Domain.Inspection;
using VSRSystemsBackend.Api.Domain.Inspection.Enums;

[ApiController]
[Route("api/railway/defects")]
public class RailwayDefectsController : ControllerBase
{
    [HttpGet]
    public IActionResult List() => Ok("Defects list");

    [HttpPost]
    public IActionResult Create([FromBody] string description, DefectSeverity severity) => Ok(new { description, severity });

    [HttpPatch("{defectId}")]
    public IActionResult Resolve(Guid defectId, [FromBody] bool accepted, [FromBody] string reason) => Ok(new { defectId, accepted, reason });
}