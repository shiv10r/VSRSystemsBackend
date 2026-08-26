using VSRSystemsBackend.Api.Domain.Maintenance;

namespace VSRSystemsBackend.Api.Application.Maintenance
{
    public class MaintenanceHandlers
    {
        public WorkOrder CreateWorkOrder(Guid sourceId, string sourceType, string priority, Guid? assignedTo, Guid createdBy)
        {
            var order = new WorkOrder
            {
                Id = Guid.NewGuid(),
                SourceId = sourceId,
                SourceType = sourceType,
                Priority = priority,
                AssignedTo = assignedTo,
                CreatedBy = createdBy,
                Status = WorkOrderStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };
            return order;
        }

        public WorkOrder Approve(WorkOrder order, Guid supervisorId, DateTime now)
        {
            order.TransitionTo(WorkOrderStatus.Approved, supervisorId, now, "approved");
            return order;
        }

        public WorkOrder Complete(WorkOrder order, Guid completedBy, DateTime now, string reason)
        {
            order.TransitionTo(WorkOrderStatus.Completed, completedBy, now, reason);
            return order;
        }

        public MaintenancePlan CreatePlan(string name, string description, string recurrenceRule, int slaDays)
        {
            return new MaintenancePlan
            {
                Name = name,
                Description = description,
                RecurrenceRule = recurrenceRule,
                SlaDays = slaDays
            };
        }

        public WorkOrder GenerateFromPlan(MaintenancePlan plan, Guid createdBy)
        {
            var order = CreateWorkOrder(plan.Id, "PreventivePlan", "Medium", null, createdBy);
            plan.GeneratedWorkOrders.Add(order);
            plan.LastGeneratedAt = DateTime.UtcNow;
            return order;
        }
    }
}