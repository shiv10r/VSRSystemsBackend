using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

[ApiController, Authorize, Route("api/railway/operations")]
public sealed class RailwayOperationsController(IRailwayScopeAccessor scopeAccessor, RailwayDbContext db) : ControllerBase
{
    [HttpGet("overview")]
    public async Task<IActionResult> Overview(CancellationToken token)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.read"); var now = DateTimeOffset.UtcNow;
        var divisions = scope.DivisionIds;
        var dueInspections = await db.InspectionAssignments.CountAsync(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value) && x.DueAt <= now, token);
        var criticalDefects = await db.Defects.CountAsync(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value) && x.Severity == DefectSeverity.Critical && x.Status != DefectStatus.Closed, token);
        var crowdRisk = await db.CrowdAlerts.CountAsync(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value) && x.IsOpen && x.Level == CrowdRiskLevel.Critical, token);
        var incidents = await db.CrowdIncidents.CountAsync(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value) && x.Status == "Open", token);
        var overdueWork = await db.WorkOrders.CountAsync(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value) && x.Status != WorkOrderStatus.Completed && x.Status != WorkOrderStatus.Cancelled && x.CreatedAt < now.AddDays(-7), token);
        var assets = await db.Assets.CountAsync(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value) && x.RetiredAt == null, token);
        var activity = await db.AuditRecords.AsNoTracking().Where(x => x.DivisionId == null || divisions.Contains(x.DivisionId.Value)).OrderByDescending(x => x.OccurredAt).Take(12)
            .Select(x => new { x.Id, x.Action, x.ResourceType, x.ResourceId, x.OccurredAt }).ToArrayAsync(token);
        return Ok(new { dueInspections, criticalDefects, crowdRisk, activeIncidents = incidents, overdueWork, activeAssets = assets, activity });
    }

    [HttpGet("reports/{reportType}")]
    public async Task<IActionResult> Report(string reportType, CancellationToken token)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.reports.read"); var divisions = scope.DivisionIds;
        string csv = reportType switch
        {
            "defects" => "Id,Severity,Status,RaisedAt\n" + string.Join('\n', await db.Defects.AsNoTracking().Where(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value)).Select(x => $"{x.Id},{x.Severity},{x.Status},{x.RaisedAt:O}").ToArrayAsync(token)),
            "work-orders" => "Id,Priority,Status,CreatedAt\n" + string.Join('\n', await db.WorkOrders.AsNoTracking().Where(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value)).Select(x => $"{x.Id},{x.Priority},{x.Status},{x.CreatedAt:O}").ToArrayAsync(token)),
            "crowd-incidents" => "Id,StationId,Status,OpenedAt\n" + string.Join('\n', await db.CrowdIncidents.AsNoTracking().Where(x => x.DivisionId != null && divisions.Contains(x.DivisionId.Value)).Select(x => $"{x.Id},{x.StationId},{x.Status},{x.OpenedAt:O}").ToArrayAsync(token)),
            _ => throw new ArgumentException("Unknown Railway report type.")
        };
        return File(Encoding.UTF8.GetBytes(csv), "text/csv", $"railway-{reportType}-{DateTime.UtcNow:yyyyMMdd}.csv");
    }

    [HttpPost("advisories")]
    public IActionResult Advisory(AdvisoryRequest request)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.ai.use");
        var prohibited = new[] { "signal", "route control", "evacuate", "isolation", "return to service" };
        if (prohibited.Any(term => request.Question.Contains(term, StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new { code = "unsafe_operational_request", message = "AI cannot execute or advise safety-critical control actions." });
        return Ok(new { status = "AdvisoryOnly", summary = "Review current inspections, open defects, crowd alerts, and maintenance backlog with an authorized operator.", sources = new[] { "Railway scoped operational records" }, generatedAt = DateTimeOffset.UtcNow, provider = "VSR policy engine", model = "deterministic-v1", requiresHumanAcceptance = true });
    }
}
public sealed record AdvisoryRequest(string Question);
