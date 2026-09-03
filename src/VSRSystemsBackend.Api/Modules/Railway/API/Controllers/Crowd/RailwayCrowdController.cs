using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers.Crowd;

[ApiController]
[Authorize]
[Route("api/railway/crowd")]
public sealed class RailwayCrowdController(
    IRailwayScopeAccessor scopeAccessor,
    RailwayDbContext dbContext,
    CrowdHandlers handlers) : ControllerBase
{
    [HttpGet("observations", Name = "railway.crowd.observations.list")]
    public async Task<IActionResult> Observations([FromQuery] Guid? stationId, CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.crowd.read");
        var query = dbContext.CrowdObservations.AsNoTracking().Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value));
        if (stationId.HasValue) query = query.Where(item => item.StationId == stationId.Value);
        return Ok(await query.OrderByDescending(item => item.WindowEnd).Take(500).Select(item => new
        {
            item.Id, item.StationId, item.StationZoneId, item.SourceId, item.SourceEventId, item.WindowStart,
            item.WindowEnd, item.Count, item.Inflow, item.Outflow, item.Confidence, item.QualityFlags
        }).ToArrayAsync(cancellationToken));
    }

    [HttpPost("observations", Name = "railway.crowd.observations.create")]
    public async Task<IActionResult> SubmitObservation([FromBody] ObservationRequest request, CancellationToken cancellationToken)
    {
        var result = await handlers.SubmitManualAsync(scopeAccessor.GetRequiredScope(), new SubmitCrowdObservationCommand(
            request.DivisionId, request.SourceId, request.SourceEventId, request.WindowStart, request.WindowEnd,
            request.Count, request.Inflow, request.Outflow, request.Confidence, request.QualityFlags.ToHashSet()), cancellationToken);
        return result.Created ? CreatedAtAction(nameof(Observations), new { stationId = result.Observation.StationId }, new { result.Observation.Id })
            : Ok(new { result.Observation.Id, duplicate = true });
    }

    [HttpGet("alerts", Name = "railway.crowd.alerts.list")]
    public async Task<IActionResult> Alerts(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.crowd.read");
        return Ok(await dbContext.CrowdAlerts.AsNoTracking()
            .Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value))
            .OrderByDescending(item => item.RaisedAt).Select(item => new
            { item.Id, item.StationId, item.StationZoneId, item.Level, item.IsOpen, item.RaisedAt, item.AcknowledgedAt, item.Version })
            .ToArrayAsync(cancellationToken));
    }

    [HttpPost("alerts/{alertId:guid}/acknowledge", Name = "railway.crowd.alerts.acknowledge")]
    public async Task<IActionResult> Acknowledge(Guid alertId, [FromHeader(Name = "If-Match")] string ifMatch, CancellationToken cancellationToken)
    {
        if (!long.TryParse(ifMatch.Trim('"'), out var expectedVersion)) return StatusCode(StatusCodes.Status428PreconditionRequired);
        await handlers.AcknowledgeAlertAsync(scopeAccessor.GetRequiredScope(), alertId, expectedVersion, cancellationToken);
        return NoContent();
    }

    [HttpGet("sources", Name = "railway.crowd.sources.list")]
    public async Task<IActionResult> Sources(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.crowd.read");
        return Ok(await dbContext.CrowdSources.AsNoTracking()
            .Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value))
            .Select(item => new { item.Id, item.DivisionId, item.StationId, item.StationZoneId, item.Name, item.AdapterType,
                item.Enabled, item.LastObservationAt, item.PreviousSecretValidUntil, item.Version }).ToArrayAsync(cancellationToken));
    }

    [HttpPost("sources", Name = "railway.crowd.sources.create")]
    public async Task<IActionResult> CreateSource([FromBody] CreateSourceRequest request, CancellationToken cancellationToken)
    {
        var credential = await handlers.CreateSourceAsync(scopeAccessor.GetRequiredScope(),
            new CreateCrowdSourceCommand(request.DivisionId, request.StationId, request.StationZoneId, request.Name, request.AdapterType), cancellationToken);
        return CreatedAtAction(nameof(Sources), new { }, credential);
    }

    [HttpPost("sources/{sourceId:guid}/rotate-credential", Name = "railway.crowd.sources.rotate")]
    public async Task<IActionResult> RotateCredential(Guid sourceId, CancellationToken cancellationToken) =>
        Ok(await handlers.RotateCredentialAsync(scopeAccessor.GetRequiredScope(), sourceId, TimeSpan.FromMinutes(15), cancellationToken));

    [HttpGet("incidents", Name = "railway.crowd.incidents.list")]
    public async Task<IActionResult> Incidents(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.crowd.read");
        return Ok(await dbContext.CrowdIncidents.AsNoTracking()
            .Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value))
            .OrderByDescending(item => item.OpenedAt).Select(item => new
            { item.Id, item.DivisionId, item.StationId, item.Title, item.Status, item.OpenedAt, item.ResponseLog, item.ClosedAt, item.Version })
            .ToArrayAsync(cancellationToken));
    }

    [HttpPost("incidents", Name = "railway.crowd.incidents.create")]
    public async Task<IActionResult> OpenIncident([FromBody] IncidentRequest request, CancellationToken cancellationToken)
    {
        var incident = await handlers.OpenIncidentAsync(scopeAccessor.GetRequiredScope(), request.DivisionId, request.StationId, request.Title, cancellationToken);
        return CreatedAtAction(nameof(Incidents), new { }, new { incident.Id });
    }

    [HttpPost("incidents/{incidentId:guid}/responses", Name = "railway.crowd.incidents.respond")]
    public async Task<IActionResult> RecordResponse(Guid incidentId, IncidentResponseRequest request, CancellationToken cancellationToken)
    { await handlers.RecordIncidentResponseAsync(scopeAccessor.GetRequiredScope(), incidentId, request.Action, cancellationToken); return NoContent(); }

    [HttpPost("incidents/{incidentId:guid}/close", Name = "railway.crowd.incidents.close")]
    public async Task<IActionResult> CloseIncident(Guid incidentId, CancellationToken cancellationToken)
    { await handlers.CloseIncidentAsync(scopeAccessor.GetRequiredScope(), incidentId, cancellationToken); return NoContent(); }

    [HttpPost("incidents/{incidentId:guid}/work-order", Name = "railway.crowd.incidents.work-order")]
    public async Task<IActionResult> CreateIncidentWorkOrder(Guid incidentId, IncidentWorkOrderRequest request, CancellationToken cancellationToken)
    { var order = await handlers.CreateIncidentWorkOrderAsync(scopeAccessor.GetRequiredScope(), incidentId, request.Priority, cancellationToken); return Ok(new { workOrderId = order.Id }); }
}

public sealed record ObservationRequest(Guid DivisionId, Guid SourceId, string SourceEventId, DateTimeOffset WindowStart,
    DateTimeOffset WindowEnd, int Count, int? Inflow, int? Outflow, decimal Confidence, IReadOnlyList<string> QualityFlags);
public sealed record CreateSourceRequest(Guid DivisionId, Guid StationId, Guid StationZoneId, string Name, string AdapterType);
public sealed record IncidentRequest(Guid DivisionId, Guid StationId, string Title);
public sealed record IncidentResponseRequest(string Action);
public sealed record IncidentWorkOrderRequest(WorkOrderPriority Priority);
