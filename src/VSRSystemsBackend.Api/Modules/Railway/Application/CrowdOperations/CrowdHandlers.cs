using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Modules.Railway.Application.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

namespace VSRSystemsBackend.Api.Modules.Railway.Application.CrowdOperations;

public sealed record CreateCrowdSourceCommand(Guid DivisionId, Guid StationId, Guid StationZoneId, string Name, string AdapterType);
public sealed record CrowdSourceCredential(Guid SourceId, string SigningSecret);
public sealed record SubmitCrowdObservationCommand(Guid DivisionId, Guid SourceId, string SourceEventId,
    DateTimeOffset WindowStart, DateTimeOffset WindowEnd, int Count, int? Inflow, int? Outflow,
    decimal Confidence, IReadOnlySet<string> QualityFlags);

public sealed class CrowdHandlers(RailwayDbContext dbContext, ICrowdSourceSecretProtector secretProtector, MaintenanceHandlers maintenanceHandlers)
{
    public async Task<CrowdSourceCredential> CreateSourceAsync(RailwayScope scope, CreateCrowdSourceCommand command,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.crowd.manage");
        scope.RequireDivision(command.DivisionId);
        if (!await dbContext.Stations.AnyAsync(item => item.Id == command.StationId && item.DivisionId == command.DivisionId, cancellationToken) ||
            !await dbContext.StationZones.AnyAsync(item => item.Id == command.StationZoneId && item.StationId == command.StationId, cancellationToken))
            throw new UnauthorizedAccessException("The station or zone is outside the authenticated Railway scope.");
        var secret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var source = new CrowdSource(Guid.NewGuid(), scope.OrganizationId, command.DivisionId, command.StationId,
            command.StationZoneId, command.Name.Trim(), command.AdapterType.Trim(), secretProtector.Protect(secret));
        dbContext.CrowdSources.Add(source);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CrowdSourceCredential(source.Id, secret);
    }

    public async Task<CrowdSourceCredential> RotateCredentialAsync(RailwayScope scope, Guid sourceId,
        TimeSpan overlap, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.crowd.manage");
        var source = await dbContext.CrowdSources.SingleAsync(item => item.Id == sourceId, cancellationToken);
        if (source.DivisionId is null) throw new InvalidOperationException("Crowd source requires a division.");
        scope.RequireDivision(source.DivisionId.Value);
        var secret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        source.RotateSigningSecret(secretProtector.Protect(secret), DateTimeOffset.UtcNow.Add(overlap));
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CrowdSourceCredential(source.Id, secret);
    }

    public async Task<(CrowdObservation Observation, bool Created)> SubmitManualAsync(RailwayScope scope,
        SubmitCrowdObservationCommand command, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.crowd.manage");
        scope.RequireDivision(command.DivisionId);
        var source = await dbContext.CrowdSources.SingleAsync(item => item.Id == command.SourceId && item.DivisionId == command.DivisionId, cancellationToken);
        var duplicate = await dbContext.CrowdObservations.SingleOrDefaultAsync(item =>
            item.SourceId == source.Id && item.SourceEventId == command.SourceEventId, cancellationToken);
        if (duplicate is not null) return (duplicate, false);
        var value = new NormalizedCrowdObservation(scope.OrganizationId, command.DivisionId, source.StationId,
            source.StationZoneId, source.Id, command.SourceEventId, command.WindowStart, command.WindowEnd,
            command.Count, command.Inflow, command.Outflow, command.Confidence, command.QualityFlags);
        var observation = new CrowdObservation(Guid.NewGuid(), value);
        dbContext.CrowdObservations.Add(observation);
        source.RecordObservation(command.WindowEnd);
        await dbContext.SaveChangesAsync(cancellationToken);
        return (observation, true);
    }

    public async Task AcknowledgeAlertAsync(RailwayScope scope, Guid alertId, long expectedVersion, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.crowd.manage");
        var alert = await dbContext.CrowdAlerts.SingleAsync(item => item.Id == alertId, cancellationToken);
        if (alert.DivisionId is not null) scope.RequireDivision(alert.DivisionId.Value);
        if (alert.Version != expectedVersion) throw new DbUpdateConcurrencyException("Crowd alert has changed.");
        alert.Acknowledge(scope.UserId, DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CrowdIncident> OpenIncidentAsync(RailwayScope scope, Guid divisionId, Guid stationId,
        string title, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.crowd.manage");
        scope.RequireDivision(divisionId);
        if (!await dbContext.Stations.AnyAsync(item => item.Id == stationId && item.DivisionId == divisionId, cancellationToken))
            throw new UnauthorizedAccessException("The station is outside the authenticated Railway scope.");
        var incident = new CrowdIncident(Guid.NewGuid(), scope.OrganizationId, divisionId, stationId, title.Trim(), scope.UserId, DateTimeOffset.UtcNow);
        dbContext.CrowdIncidents.Add(incident);
        await dbContext.SaveChangesAsync(cancellationToken);
        return incident;
    }

    public async Task RecordIncidentResponseAsync(RailwayScope scope, Guid incidentId, string action, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.crowd.manage"); var incident = await dbContext.CrowdIncidents.SingleAsync(item => item.Id == incidentId, cancellationToken);
        scope.RequireDivision(incident.DivisionId!.Value); incident.RecordResponse(action, scope.UserId, DateTimeOffset.UtcNow); await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CloseIncidentAsync(RailwayScope scope, Guid incidentId, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.crowd.manage"); var incident = await dbContext.CrowdIncidents.SingleAsync(item => item.Id == incidentId, cancellationToken);
        scope.RequireDivision(incident.DivisionId!.Value); incident.Close(scope.UserId, DateTimeOffset.UtcNow); await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<WorkOrder> CreateIncidentWorkOrderAsync(RailwayScope scope, Guid incidentId, WorkOrderPriority priority, CancellationToken cancellationToken)
    {
        var incident = await dbContext.CrowdIncidents.AsNoTracking().SingleAsync(item => item.Id == incidentId, cancellationToken);
        return await maintenanceHandlers.CreateAsync(scope, incident.DivisionId!.Value, incident.Id, "CrowdIncident", incident.StationId, priority, false, cancellationToken);
    }
}
