using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.BackgroundJobs;

public sealed class MaintenanceScheduleWorker(IServiceScopeFactory scopeFactory, ILogger<MaintenanceScheduleWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try { await GenerateAsync(stoppingToken); } catch (Exception exception) { logger.LogError(exception, "Maintenance scheduling failed."); }
        }
    }
    private async Task GenerateAsync(CancellationToken token)
    {
        using var scope = scopeFactory.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<RailwayDbContext>(); var now = DateTimeOffset.UtcNow;
        var plans = await db.MaintenancePlans.IgnoreQueryFilters().Where(x => x.Enabled && x.NextDueAt <= now).ToArrayAsync(token);
        foreach (var plan in plans)
        {
            var sourceId = OccurrenceId(plan.Id, plan.NextDueAt);
            if (!await db.WorkOrders.IgnoreQueryFilters().AnyAsync(x => x.SourceId == sourceId && x.SourceType == "PreventivePlan", token))
            {
                var order = new WorkOrder(Guid.NewGuid(), plan.OrganizationId, plan.DivisionId!.Value, sourceId, "PreventivePlan", plan.TargetId, WorkOrderPriority.Medium, false, Guid.Empty, now);
                order.AddTask(plan.Name); db.WorkOrders.Add(order);
            }
            plan.Advance(Next(plan.NextDueAt, plan.RecurrenceRule));
        }
        await db.SaveChangesAsync(token);
    }
    private static Guid OccurrenceId(Guid planId, DateTimeOffset dueAt) => new(SHA256.HashData(Encoding.UTF8.GetBytes($"{planId:N}:{dueAt:O}"))[..16]);
    private static DateTimeOffset Next(DateTimeOffset current, string rule) => rule.Contains("MONTHLY", StringComparison.OrdinalIgnoreCase) ? current.AddMonths(1) : rule.Contains("WEEKLY", StringComparison.OrdinalIgnoreCase) ? current.AddDays(7) : current.AddDays(1);
}
