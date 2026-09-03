using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure;

public sealed class RailwayFeatureGate(IConfiguration configuration) : IRailwayFeatureGate
{
    public ValueTask<RailwayCapabilities> GetAsync(RailwayScope scope, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var railwayEnabled = ReadFlag("RAILWAY_ENABLED");
        var capabilities = new RailwayCapabilities(
            scope.OrganizationId,
            railwayEnabled,
            railwayEnabled && ReadFlag("RAILWAY_INSPECTION_ENABLED"),
            railwayEnabled && ReadFlag("RAILWAY_MAINTENANCE_ENABLED"),
            railwayEnabled && ReadFlag("RAILWAY_CROWD_ENABLED"),
            railwayEnabled && ReadFlag("RAILWAY_CROWD_ENABLED") && ReadFlag("RAILWAY_LIVE_ADAPTERS_ENABLED"),
            railwayEnabled && ReadFlag("RAILWAY_AI_ENABLED"),
            Math.Clamp(configuration.GetValue("RAILWAY_OFFLINE_PACK_MAX_AGE_HOURS", 72), 1, 72),
            scope.Permissions);

        return ValueTask.FromResult(capabilities);
    }

    private bool ReadFlag(string name) => configuration.GetValue(name, false);
}
