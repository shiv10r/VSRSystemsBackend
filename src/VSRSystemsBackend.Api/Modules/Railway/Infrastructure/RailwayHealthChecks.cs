using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure;

public sealed class RailwayReadinessHealthCheck(IServiceScopeFactory scopeFactory, IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (!configuration.GetValue("Railway:Enabled", false) && !configuration.GetValue("RAILWAY_ENABLED", false))
            return HealthCheckResult.Healthy("Railway is disabled.");
        try
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RailwayDbContext>();
            if (!await db.Database.CanConnectAsync(cancellationToken)) return HealthCheckResult.Unhealthy("Railway database is unavailable.");
            var deadLetters = await db.OutboxMessages.CountAsync(item => item.DeadLetteredAt != null, cancellationToken);
            return deadLetters > 0 ? HealthCheckResult.Degraded($"Railway has {deadLetters} dead-lettered events.") : HealthCheckResult.Healthy("Railway dependencies are ready.");
        }
        catch (Exception exception) { return HealthCheckResult.Unhealthy("Railway readiness check failed.", exception); }
    }
}
