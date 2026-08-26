namespace VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

public sealed record RailwayCapabilities(
    Guid OrganizationId,
    bool RailwayEnabled,
    bool InspectionEnabled,
    bool MaintenanceEnabled,
    bool CrowdEnabled,
    bool LiveCrowdAdaptersEnabled,
    bool AiEnabled,
    int OfflinePackMaxAgeHours,
    IReadOnlySet<string> Permissions);

public interface IRailwayFeatureGate
{
    ValueTask<RailwayCapabilities> GetAsync(RailwayScope scope, CancellationToken cancellationToken);
}
