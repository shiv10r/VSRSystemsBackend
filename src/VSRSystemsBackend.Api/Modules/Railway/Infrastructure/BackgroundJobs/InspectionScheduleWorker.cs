using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.BackgroundJobs;

public sealed class InspectionScheduleWorker(IServiceScopeFactory scopeFactory, ILogger<InspectionScheduleWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try { await GenerateAsync(stoppingToken); }
            catch (Exception exception) { logger.LogError(exception, "Inspection scheduling failed."); }
        }
    }

    private async Task GenerateAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<RailwayDbContext>(); var now = DateTimeOffset.UtcNow;
        var plans = await db.InspectionPlans.IgnoreQueryFilters().Where(item => item.Enabled && item.NextDueAt <= now).ToArrayAsync(cancellationToken);
        foreach (var plan in plans)
        {
            var occurrenceKey = $"{plan.Id:N}:{plan.NextDueAt:O}";
            if (!await db.InspectionAssignments.IgnoreQueryFilters().AnyAsync(item => item.OccurrenceKey == occurrenceKey, cancellationToken))
                db.InspectionAssignments.Add(new InspectionAssignment(Guid.NewGuid(), plan.OrganizationId, plan.DivisionId!.Value,
                    plan.Id, plan.TemplateId, plan.TemplateVersion, plan.TargetId, plan.InspectorId, plan.NextDueAt, occurrenceKey));
            plan.Advance(Next(plan.NextDueAt, plan.Schedule));
        }
        await db.SaveChangesAsync(cancellationToken);
    }

    private static DateTimeOffset Next(DateTimeOffset current, string schedule) =>
        schedule.Contains("MONTHLY", StringComparison.OrdinalIgnoreCase) ? current.AddMonths(1) :
        schedule.Contains("WEEKLY", StringComparison.OrdinalIgnoreCase) ? current.AddDays(7) : current.AddDays(1);
}
