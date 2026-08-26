using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Realtime;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.BackgroundJobs;

public sealed class CrowdRiskWorker(IServiceScopeFactory scopeFactory, ILogger<CrowdRiskWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        while (!stoppingToken.IsCancellationRequested)
        {
            try { await EvaluateAsync(stoppingToken); }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
            catch (Exception exception) { logger.LogError(exception, "Railway crowd risk evaluation failed."); }
            await timer.WaitForNextTickAsync(stoppingToken);
        }
    }

    internal async Task EvaluateAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RailwayDbContext>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IRailwayEventPublisher>();
        var realtime = scope.ServiceProvider.GetRequiredService<IRailwayRealtimePublisher>();
        var now = DateTimeOffset.UtcNow;
        var observations = await dbContext.CrowdObservations.IgnoreQueryFilters().AsNoTracking()
            .Where(item => item.WindowEnd >= now.AddMinutes(-5) && item.WindowEnd <= now.AddMinutes(1))
            .ToArrayAsync(cancellationToken);
        var policies = await dbContext.CrowdThresholdPolicies.IgnoreQueryFilters().AsNoTracking()
            .Where(item => item.EffectiveFrom <= now && (item.EffectiveUntil == null || item.EffectiveUntil > now))
            .OrderByDescending(item => item.EffectiveFrom).ToArrayAsync(cancellationToken);
        var notifications = new List<(Guid OrganizationId, Guid StationId, RailwayRealtimeEvent Event)>();

        foreach (var group in observations.GroupBy(item => new { item.OrganizationId, item.DivisionId, item.StationId, item.StationZoneId }))
        {
            var policy = policies.FirstOrDefault(item => item.OrganizationId == group.Key.OrganizationId && item.StationZoneId == group.Key.StationZoneId);
            if (policy is null || group.Key.DivisionId is null) continue;
            var latestBySource = group.GroupBy(item => item.SourceId).Select(items => items.OrderByDescending(item => item.WindowEnd).First());
            var count = latestBySource.Sum(item => item.Count);
            var level = CrowdRiskPolicy.Calculate(count, policy.WarningThreshold, policy.CriticalThreshold);
            if (level == CrowdRiskLevel.Normal) continue;
            var hasOpenAlert = await dbContext.CrowdAlerts.IgnoreQueryFilters().AnyAsync(item =>
                item.OrganizationId == group.Key.OrganizationId && item.StationZoneId == group.Key.StationZoneId && item.IsOpen, cancellationToken);
            if (hasOpenAlert) continue;

            var alert = new CrowdAlert(Guid.NewGuid(), group.Key.OrganizationId, group.Key.DivisionId.Value,
                group.Key.StationId, group.Key.StationZoneId, level, now);
            dbContext.CrowdAlerts.Add(alert);
            var domainEvent = new CrowdThresholdBreached(Guid.NewGuid(), group.Key.OrganizationId, group.Key.StationId,
                group.Key.StationZoneId, alert.Id, level, count, now, alert.Id.ToString());
            eventPublisher.Enqueue(domainEvent, domainEvent);
            notifications.Add((group.Key.OrganizationId, group.Key.StationId,
                new RailwayRealtimeEvent(domainEvent.EventId, "crowd-alert-changed", alert.Id, now)));
        }
        if (notifications.Count == 0) return;
        await dbContext.SaveChangesAsync(cancellationToken);
        foreach (var notification in notifications)
            await realtime.PublishToStationAsync(notification.OrganizationId, notification.StationId, notification.Event, cancellationToken);
    }
}
