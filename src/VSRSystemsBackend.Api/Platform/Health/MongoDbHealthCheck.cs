using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using VSRSystemsBackend.Infrastructure.Persistence.Mongo;

namespace VSRSystemsBackend.Api.Platform.Health;

public sealed class MongoDbHealthCheck : IHealthCheck
{
    private readonly MongoDbContext _mongoDb;
    private readonly ILogger<MongoDbHealthCheck> _logger;

    public MongoDbHealthCheck(MongoDbContext mongoDb, ILogger<MongoDbHealthCheck> logger)
    {
        _mongoDb = mongoDb;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (!_mongoDb.IsConfigured)
        {
            return HealthCheckResult.Degraded(
                _mongoDb.ConfigurationError ?? "MongoDB is not configured; document-backed features are disabled.");
        }

        try
        {
            await _mongoDb.Database!.RunCommandAsync<BsonDocument>(
                new BsonDocument("ping", 1),
                cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy("MongoDB is reachable.");
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "MongoDB health check failed");
            return HealthCheckResult.Degraded("MongoDB is unavailable; document-backed features are disabled.");
        }
    }
}
