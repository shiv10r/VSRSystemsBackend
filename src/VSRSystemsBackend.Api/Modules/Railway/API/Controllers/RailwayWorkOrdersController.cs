using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

public sealed record CreateWorkOrderRequest(Guid DivisionId, Guid SourceId, string SourceType, Guid TargetId, WorkOrderPriority Priority, bool SafetyClassified);
public sealed record WorkOrderActionRequest(Guid? AssigneeId = null, Guid? EvidenceId = null, Guid? TaskId = null, string? Reason = null);
public sealed record CreateMaintenancePlanRequest(Guid DivisionId, Guid TargetId, string Name, string RecurrenceRule, int SlaDays, DateTimeOffset NextDueAt);

[ApiController]
[Authorize]
[Route("api/railway/work-orders")]
public sealed class RailwayWorkOrdersController(
    IRailwayScopeAccessor scopeAccessor,
    MaintenanceHandlers handlers,
    RailwayDbContext dbContext) : ControllerBase
{
    [HttpGet(Name = "railway.work-orders.list")]
    public async Task<ActionResult<IReadOnlyList<WorkOrder>>> List([FromQuery] WorkOrderStatus? status, CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope();
        scope.RequirePermission("railway.work-orders.read");
        var query = dbContext.WorkOrders.AsNoTracking().Where(item => item.DivisionId.HasValue && scope.DivisionIds.Contains(item.DivisionId.Value));
        if (status.HasValue) query = query.Where(item => item.Status == status);
        return Ok(await query.OrderByDescending(item => item.CreatedAt).ToListAsync(cancellationToken));
    }

    [HttpPost(Name = "railway.work-orders.create")]
    public async Task<ActionResult<WorkOrder>> Create(CreateWorkOrderRequest request, [FromHeader(Name = "Idempotency-Key")] string idempotencyKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        return Ok(await handlers.CreateAsync(scopeAccessor.GetRequiredScope(), request.DivisionId, request.SourceId, request.SourceType, request.TargetId, request.Priority, request.SafetyClassified, cancellationToken));
    }

    [HttpPost("{orderId:guid}/{action}", Name = "railway.work-orders.execute")]
    public async Task<ActionResult<WorkOrder>> Execute(Guid orderId, string action, WorkOrderActionRequest request, [FromHeader(Name = "If-Match")] string ifMatch, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ifMatch)) return BadRequest();
        return Ok(await handlers.ExecuteAsync(scopeAccessor.GetRequiredScope(), orderId, action, request.AssigneeId, request.EvidenceId, request.TaskId, request.Reason, cancellationToken));
    }
}

[ApiController]
[Authorize]
[Route("api/railway/maintenance/plans")]
public sealed class RailwayMaintenancePlansController(IRailwayScopeAccessor scopeAccessor, MaintenanceHandlers handlers, RailwayDbContext dbContext) : ControllerBase
{
    [HttpGet(Name = "railway.maintenance.plans.list")]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.maintenance.read");
        return Ok(await dbContext.MaintenancePlans.AsNoTracking()
            .Where(item => item.DivisionId != null && scope.DivisionIds.Contains(item.DivisionId.Value))
            .OrderBy(item => item.NextDueAt).ToArrayAsync(cancellationToken));
    }

    [HttpPost(Name = "railway.maintenance.plans.create")]
    public async Task<ActionResult<MaintenancePlan>> Create(CreateMaintenancePlanRequest request, [FromHeader(Name = "Idempotency-Key")] string idempotencyKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest();
        return Ok(await handlers.CreatePlanAsync(scopeAccessor.GetRequiredScope(), request.DivisionId, request.TargetId, request.Name, request.RecurrenceRule, request.SlaDays, request.NextDueAt, cancellationToken));
    }
}
