using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Modules.Railway.Application.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

public sealed record CreateInspectionTemplateRequest(Guid DivisionId, string Name, IReadOnlyList<InspectionTemplateItemInput> Items);
public sealed record StartInspectionRunRequest(Guid DivisionId, Guid AssignmentId, Guid TemplateId, Guid TargetId);
public sealed record SaveInspectionAnswerRequest(string ItemId, string Response, double? Measurement, IReadOnlyList<Guid> EvidenceIds);
public sealed record ReviewInspectionRequest(bool Accepted, string? Reason);
public sealed record RaiseInspectionDefectRequest(string Description, DefectSeverity Severity);
public sealed record CreateInspectionPlanRequest(Guid DivisionId, Guid TemplateId, Guid TargetId, Guid InspectorId, string Schedule, string TimeZone, DateTimeOffset NextDueAt);

[ApiController]
[Authorize]
[Route("api/railway/inspections")]
public sealed class RailwayInspectionsController(
    IRailwayScopeAccessor scopeAccessor,
    InspectionHandlers handlers,
    RailwayDbContext dbContext) : ControllerBase
{
    [HttpGet("templates", Name = "railway.inspections.templates.list")]
    public async Task<IActionResult> Templates(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.inspections.read");
        var templates = await dbContext.InspectionTemplates.AsNoTracking()
            .Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value))
            .OrderBy(item => item.Name).Select(item => new
            {
                item.Id, item.DivisionId, item.Name, item.TemplateVersion, item.Status, item.Version,
                Items = item.Items.Select(entry => new { entry.ItemId, entry.Label, entry.Required, entry.EvidenceRequired, entry.Minimum, entry.Maximum })
            }).ToArrayAsync(cancellationToken);
        return Ok(templates);
    }

    [HttpGet("assignments", Name = "railway.inspections.assignments.list")]
    public async Task<IActionResult> Assignments(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.inspections.read");
        var canReview = scope.Permissions.Contains("railway.inspections.review") || scope.Permissions.Contains("railway.inspections.manage");
        var query = dbContext.InspectionAssignments.AsNoTracking()
            .Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value));
        if (!canReview) query = query.Where(item => item.InspectorId == scope.UserId);
        return Ok(await query.OrderBy(item => item.DueAt).Select(item => new
        { item.Id, item.DivisionId, item.PlanId, item.TemplateId, item.TemplateVersion, item.TargetId, item.InspectorId, item.DueAt, item.OccurrenceKey, item.Version })
            .ToArrayAsync(cancellationToken));
    }

    [HttpPost("plans", Name = "railway.inspections.plans.create")]
    public async Task<IActionResult> CreatePlan(CreateInspectionPlanRequest request, CancellationToken cancellationToken) =>
        Ok(await handlers.CreatePlanAsync(scopeAccessor.GetRequiredScope(), request.DivisionId, request.TemplateId,
            request.TargetId, request.InspectorId, request.Schedule, request.TimeZone, request.NextDueAt, cancellationToken));

    [HttpGet("runs", Name = "railway.inspections.runs.list")]
    public async Task<IActionResult> Runs([FromQuery] InspectionRunStatus? status, CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.inspections.read");
        var canReview = scope.Permissions.Contains("railway.inspections.review") || scope.Permissions.Contains("railway.inspections.manage");
        var query = dbContext.InspectionRuns.AsNoTracking()
            .Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value));
        if (!canReview) query = query.Where(item => item.AssignedInspectorId == scope.UserId);
        if (status.HasValue) query = query.Where(item => item.Status == status.Value);
        return Ok(await query.OrderByDescending(item => item.StartedAt).Select(item => new
        { item.Id, item.DivisionId, item.AssignmentId, item.TemplateId, item.TemplateVersion, item.TargetId, item.AssignedInspectorId,
            item.Status, item.StartedAt, item.SubmittedAt, item.ReviewedBy, item.ReviewReason, item.AmendsInspectionRunId, item.Version })
            .ToArrayAsync(cancellationToken));
    }

    [HttpGet("runs/{runId:guid}", Name = "railway.inspections.runs.get")]
    public async Task<IActionResult> Run(Guid runId, CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.inspections.read");
        var run = await dbContext.InspectionRuns.AsNoTracking().SingleAsync(item => item.Id == runId, cancellationToken);
        if (run.DivisionId is not null) scope.RequireDivision(run.DivisionId.Value);
        if (run.AssignedInspectorId != scope.UserId && !scope.Permissions.Contains("railway.inspections.review") && !scope.Permissions.Contains("railway.inspections.manage"))
            throw new UnauthorizedAccessException("Inspection run is assigned to another inspector.");
        return Ok(new
        {
            run.Id, run.DivisionId, run.AssignmentId, run.TemplateId, run.TemplateVersion, run.TargetId, run.AssignedInspectorId,
            run.Status, run.StartedAt, run.SubmittedAt, run.ReviewedBy, run.ReviewReason, run.AmendsInspectionRunId, run.Version,
            Requirements = run.Requirements.Select(item => new { item.ItemId, item.Required, item.EvidenceRequired, item.Minimum, item.Maximum }),
            Answers = run.Answers.Select(item => new { item.ItemId, item.Response, item.Measurement, item.EvidenceIds })
        });
    }

    [HttpPost("templates", Name = "railway.inspections.templates.create")]
    public async Task<ActionResult<InspectionTemplate>> CreateTemplate(
        CreateInspectionTemplateRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        return Ok(await handlers.CreateTemplateAsync(
            scopeAccessor.GetRequiredScope(), request.DivisionId, request.Name, request.Items, cancellationToken));
    }

    [HttpPost("templates/{templateId:guid}/publish", Name = "railway.inspections.templates.publish")]
    public async Task<IActionResult> PublishTemplate(Guid templateId, [FromHeader(Name = "If-Match")] string ifMatch, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ifMatch)) return BadRequest();
        await handlers.PublishTemplateAsync(scopeAccessor.GetRequiredScope(), templateId, cancellationToken);
        return NoContent();
    }

    [HttpPost("runs", Name = "railway.inspections.runs.start")]
    public async Task<ActionResult<InspectionRun>> StartRun(
        StartInspectionRunRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        return Ok(await handlers.StartRunAsync(
            scopeAccessor.GetRequiredScope(), request.DivisionId, request.AssignmentId,
            request.TemplateId, request.TargetId, cancellationToken));
    }

    [HttpPut("runs/{runId:guid}/answers", Name = "railway.inspections.runs.answer")]
    public async Task<ActionResult<InspectionRun>> SaveAnswer(
        Guid runId,
        SaveInspectionAnswerRequest request,
        [FromHeader(Name = "If-Match")] string ifMatch,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ifMatch)) return BadRequest();
        return Ok(await handlers.SaveAnswerAsync(
            scopeAccessor.GetRequiredScope(), runId, request.ItemId, request.Response,
            request.Measurement, request.EvidenceIds, cancellationToken));
    }

    [HttpPost("runs/{runId:guid}/submit", Name = "railway.inspections.runs.submit")]
    public async Task<ActionResult<InspectionRun>> Submit(
        Guid runId,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        return Ok(await handlers.SubmitAsync(scopeAccessor.GetRequiredScope(), runId, cancellationToken));
    }

    [HttpPost("runs/{runId:guid}/review", Name = "railway.inspections.runs.review")]
    public async Task<ActionResult<InspectionRun>> Review(
        Guid runId,
        ReviewInspectionRequest request,
        [FromHeader(Name = "If-Match")] string ifMatch,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ifMatch)) return BadRequest();
        return Ok(await handlers.ReviewAsync(
            scopeAccessor.GetRequiredScope(), runId, request.Accepted, request.Reason, cancellationToken));
    }

    [HttpPost("runs/{runId:guid}/defects", Name = "railway.inspections.runs.raise-defect")]
    public async Task<ActionResult<Defect>> RaiseDefect(
        Guid runId,
        RaiseInspectionDefectRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        return Ok(await handlers.RaiseDefectAsync(
            scopeAccessor.GetRequiredScope(), runId, request.Description, request.Severity, cancellationToken));
    }
}
