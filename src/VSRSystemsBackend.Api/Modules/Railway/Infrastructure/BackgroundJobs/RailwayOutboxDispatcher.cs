using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Platform.Outbox;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.BackgroundJobs;

public interface IRailwayIntegrationEventSink { Task PublishAsync(PlatformOutboxMessage message, CancellationToken cancellationToken); }
public sealed class RailwayIntegrationEventLogSink(ILogger<RailwayIntegrationEventLogSink> logger) : IRailwayIntegrationEventSink
{ public Task PublishAsync(PlatformOutboxMessage message, CancellationToken cancellationToken) { logger.LogInformation("Railway event {EventName} {EventId}", message.EventName, message.Id); return Task.CompletedTask; } }

public sealed class RailwayOutboxDispatcher(IServiceScopeFactory scopeFactory, ILogger<RailwayOutboxDispatcher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = scopeFactory.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<RailwayDbContext>(); var sink = scope.ServiceProvider.GetRequiredService<IRailwayIntegrationEventSink>(); var now = DateTimeOffset.UtcNow;
            var messages = await db.OutboxMessages.Where(item => item.DispatchedAt == null && item.DeadLetteredAt == null && (item.LeaseUntil == null || item.LeaseUntil < now)).OrderBy(item => item.OccurredAt).Take(50).ToArrayAsync(stoppingToken);
            foreach (var message in messages)
            {
                message.Lease(now.AddMinutes(1)); await db.SaveChangesAsync(stoppingToken);
                try { await sink.PublishAsync(message, stoppingToken); message.MarkDispatched(DateTimeOffset.UtcNow); }
                catch (Exception exception) { message.MarkFailed(exception.Message, DateTimeOffset.UtcNow); logger.LogError(exception, "Railway outbox dispatch failed for {EventId}", message.Id); }
                await db.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
