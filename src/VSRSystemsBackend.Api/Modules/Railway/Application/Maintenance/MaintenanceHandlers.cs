using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Application.Maintenance;

public sealed class MaintenanceHandlers(RailwayDbContext dbContext)
{
    public async Task<WorkOrder> CreateAsync(RailwayScope scope, Guid divisionId, Guid sourceId, string sourceType, Guid targetId, WorkOrderPriority priority, bool safetyClassified, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.work-orders.create");
        scope.RequireDivision(divisionId);
        if (!await dbContext.Set<RailwayMasterRecord>().AnyAsync(item => item.Id == targetId && item.DivisionId == divisionId, cancellationToken))
            throw new KeyNotFoundException();
        var order = new WorkOrder(Guid.NewGuid(), scope.OrganizationId, divisionId, sourceId, sourceType, targetId, priority, safetyClassified, scope.UserId, DateTimeOffset.UtcNow);
        dbContext.WorkOrders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<MaintenancePlan> CreatePlanAsync(RailwayScope scope, Guid divisionId, Guid targetId, string name, string recurrenceRule, int slaDays, DateTimeOffset nextDueAt, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.maintenance.manage");
        scope.RequireDivision(divisionId);
        var plan = new MaintenancePlan(Guid.NewGuid(), scope.OrganizationId, divisionId, targetId, name, recurrenceRule, slaDays, nextDueAt);
        dbContext.MaintenancePlans.Add(plan);
        await dbContext.SaveChangesAsync(cancellationToken);
        return plan;
    }

    public async Task<WorkOrder> ExecuteAsync(RailwayScope scope, Guid id, string action, Guid? assignee, Guid? evidenceId, Guid? taskId, string? reason, CancellationToken cancellationToken)
    {
        var order = await dbContext.WorkOrders.SingleOrDefaultAsync(item => item.Id == id, cancellationToken) ?? throw new KeyNotFoundException();
        scope.RequireDivision(order.DivisionId!.Value);
        var now = DateTimeOffset.UtcNow;
        switch (action)
        {
            case "add-task": scope.RequirePermission("railway.work-orders.create"); order.AddTask(reason ?? throw new ArgumentException("Task description required.")); break;
            case "triage": scope.RequirePermission("railway.work-orders.create"); order.Triage(scope.UserId, now); break;
            case "approve": scope.RequirePermission("railway.work-orders.approve"); order.Approve(scope.UserId, now); break;
            case "schedule": scope.RequirePermission("railway.work-orders.assign"); order.Schedule(assignee ?? throw new ArgumentException("Assignee required."), now, scope.UserId, now); break;
            case "permit": order.AttachPermit(evidenceId ?? throw new ArgumentException("Permit evidence required."), scope.UserId, now); break;
            case "start": order.Start(scope.UserId, now); break;
            case "complete-task": order.CompleteTask(taskId ?? throw new ArgumentException("Task required."), scope.UserId, now); break;
            case "block": order.Block(reason ?? string.Empty, scope.UserId, now); break;
            case "unblock": order.Unblock(scope.UserId, now); break;
            case "submit-verification": order.SubmitVerification(scope.UserId, now); break;
            case "verify": scope.RequirePermission("railway.work-orders.verify"); order.Verify(scope.UserId, now); break;
            case "reject-verification": scope.RequirePermission("railway.work-orders.verify"); order.RejectVerification(scope.UserId, now, reason ?? "rejected"); break;
            case "cancel": scope.RequirePermission("railway.work-orders.approve"); order.Cancel(scope.UserId, now, reason ?? "cancelled"); break;
            default: throw new ArgumentException("Unsupported work-order action.", nameof(action));
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }
}
