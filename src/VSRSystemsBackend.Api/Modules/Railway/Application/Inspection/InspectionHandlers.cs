using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Application.Inspection;

public sealed record InspectionTemplateItemInput(
    string ItemId, string Label, bool Required, bool EvidenceRequired, double? Minimum, double? Maximum);

public sealed class InspectionHandlers(
    RailwayDbContext dbContext,
    IRailwayEventPublisher eventPublisher)
{
    public async Task<InspectionPlan> CreatePlanAsync(RailwayScope scope, Guid divisionId, Guid templateId, Guid targetId,
        Guid inspectorId, string schedule, string timeZone, DateTimeOffset nextDueAt, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.inspections.manage"); scope.RequireDivision(divisionId);
        var template = await dbContext.InspectionTemplates.SingleAsync(item => item.Id == templateId && item.DivisionId == divisionId, cancellationToken);
        if (template.Status != InspectionTemplateStatus.Published) throw new InvalidOperationException("Only published templates can be scheduled.");
        var plan = new InspectionPlan(Guid.NewGuid(), scope.OrganizationId, divisionId, template.Id, template.TemplateVersion,
            targetId, inspectorId, schedule, timeZone, nextDueAt);
        dbContext.InspectionPlans.Add(plan); await dbContext.SaveChangesAsync(cancellationToken); return plan;
    }

    public async Task<InspectionTemplate> CreateTemplateAsync(
        RailwayScope scope,
        Guid divisionId,
        string name,
        IReadOnlyList<InspectionTemplateItemInput> items,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.inspections.manage");
        scope.RequireDivision(divisionId);
        var template = new InspectionTemplate(Guid.NewGuid(), scope.OrganizationId, divisionId, name);
        foreach (var item in items)
            template.AddItem(item.ItemId, item.Label, item.Required, item.EvidenceRequired, item.Minimum, item.Maximum);
        dbContext.InspectionTemplates.Add(template);
        await dbContext.SaveChangesAsync(cancellationToken);
        return template;
    }

    public async Task PublishTemplateAsync(RailwayScope scope, Guid templateId, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.inspections.manage");
        var template = await dbContext.InspectionTemplates.SingleOrDefaultAsync(item => item.Id == templateId, cancellationToken)
            ?? throw new KeyNotFoundException();
        scope.RequireDivision(template.DivisionId!.Value);
        template.Publish();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<InspectionRun> StartRunAsync(
        RailwayScope scope,
        Guid divisionId,
        Guid assignmentId,
        Guid templateId,
        Guid targetId,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.inspections.execute");
        scope.RequireDivision(divisionId);
        var template = await dbContext.InspectionTemplates.SingleOrDefaultAsync(item =>
            item.Id == templateId && item.DivisionId == divisionId && item.Status == InspectionTemplateStatus.Published,
            cancellationToken) ?? throw new KeyNotFoundException();
        var targetExists = await dbContext.Set<RailwayMasterRecord>().AnyAsync(item => item.Id == targetId && item.DivisionId == divisionId, cancellationToken);
        if (!targetExists) throw new KeyNotFoundException();
        var run = new InspectionRun(
            Guid.NewGuid(), scope.OrganizationId, divisionId, assignmentId, template.Id,
            template.TemplateVersion, targetId, scope.UserId, template.Items, DateTimeOffset.UtcNow);
        dbContext.InspectionRuns.Add(run);
        await dbContext.SaveChangesAsync(cancellationToken);
        return run;
    }

    public async Task<InspectionRun> SaveAnswerAsync(
        RailwayScope scope,
        Guid runId,
        string itemId,
        string response,
        double? measurement,
        IReadOnlyList<Guid> evidenceIds,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.inspections.execute");
        var run = await RequiredRunAsync(scope, runId, cancellationToken);
        if (run.AssignedInspectorId != scope.UserId) throw new UnauthorizedAccessException();
        if (evidenceIds.Count > 0)
        {
            var cleanEvidence = await dbContext.Evidence.CountAsync(item =>
                evidenceIds.Contains(item.Id) && item.DivisionId == run.DivisionId &&
                item.ScanStatus == RailwayEvidenceScanStatus.Clean,
                cancellationToken);
            if (cleanEvidence != evidenceIds.Distinct().Count())
                throw new InspectionValidationException("Inspection evidence is missing, quarantined, or outside the assigned division.");
        }
        run.Answer(itemId, response, measurement, evidenceIds);
        await dbContext.SaveChangesAsync(cancellationToken);
        return run;
    }

    public async Task<InspectionRun> SubmitAsync(RailwayScope scope, Guid runId, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.inspections.submit");
        var run = await RequiredRunAsync(scope, runId, cancellationToken);
        run.Submit(scope.UserId, DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
        return run;
    }

    public async Task<InspectionRun> ReviewAsync(RailwayScope scope, Guid runId, bool accepted, string? reason, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.inspections.review");
        var run = await RequiredRunAsync(scope, runId, cancellationToken);
        run.Review(accepted, scope.UserId, DateTimeOffset.UtcNow, reason);
        await dbContext.SaveChangesAsync(cancellationToken);
        return run;
    }

    public async Task<Defect> RaiseDefectAsync(
        RailwayScope scope,
        Guid runId,
        string description,
        DefectSeverity severity,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.defects.raise");
        var run = await RequiredRunAsync(scope, runId, cancellationToken);
        var defect = new Defect(
            Guid.NewGuid(), scope.OrganizationId, run.DivisionId!.Value, run.Id, run.TargetId,
            description, severity, DateTimeOffset.UtcNow);
        dbContext.Defects.Add(defect);
        if (severity == DefectSeverity.Critical)
        {
            var raised = new CriticalDefectRaised(
                Guid.NewGuid(), scope.OrganizationId, defect.Id, defect.TargetId, DateTimeOffset.UtcNow, run.Id.ToString());
            eventPublisher.Enqueue(raised, raised);
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return defect;
    }

    private async Task<InspectionRun> RequiredRunAsync(RailwayScope scope, Guid runId, CancellationToken cancellationToken)
    {
        var run = await dbContext.InspectionRuns.SingleOrDefaultAsync(item => item.Id == runId, cancellationToken)
            ?? throw new KeyNotFoundException();
        scope.RequireDivision(run.DivisionId!.Value);
        return run;
    }
}
