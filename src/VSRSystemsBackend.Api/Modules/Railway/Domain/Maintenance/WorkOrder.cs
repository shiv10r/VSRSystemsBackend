namespace VSRSystemsBackend.Api.Domain.Maintenance
{
    public class WorkOrder
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public string SourceType { get; set; } = "";
        public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Draft;
        public string Priority { get; set; } = "Medium";
        public Guid? AssignedTo { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public string? Reason { get; set; }
        public List<WorkOrderTask> Tasks = new();
        public List<LaborLog> LaborLogs = new();
        public List<MaterialUsage> MaterialUsages = new();

        private static readonly Dictionary<WorkOrderStatus, WorkOrderStatus[]> Allowed =
            new()
            {
                [WorkOrderStatus.Draft] = new[] { WorkOrderStatus.Triaged, WorkOrderStatus.Cancelled },
                [WorkOrderStatus.Triaged] = new[] { WorkOrderStatus.Approved, WorkOrderStatus.Cancelled },
                [WorkOrderStatus.Approved] = new[] { WorkOrderStatus.Scheduled, WorkOrderStatus.Cancelled },
                [WorkOrderStatus.Scheduled] = new[] { WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled },
                [WorkOrderStatus.InProgress] = new[] { WorkOrderStatus.Blocked, WorkOrderStatus.AwaitingVerification, WorkOrderStatus.Completed },
                [WorkOrderStatus.Blocked] = new[] { WorkOrderStatus.InProgress, WorkOrderStatus.Cancelled },
                [WorkOrderStatus.AwaitingVerification] = new[] { WorkOrderStatus.Completed, WorkOrderStatus.InProgress },
            };

        public void TransitionTo(WorkOrderStatus to, Guid actor, DateTime now, string reason)
        {
            if (!Allowed[Status].Contains(to))
            {
                throw new InvalidOperationException($"Invalid transition from {Status} to {to}");
            }
            Status = to;
            if (to == WorkOrderStatus.Completed)
            {
                CompletedAt = now;
            }
            Reason = reason;
        }

        public void Start(Guid technicianId, DateTime now)
        {
            if (!AssignedTo.HasValue || AssignedTo.Value != technicianId)
            {
                throw new InvalidOperationException("Only the assigned technician can start this work order");
            }
            TransitionTo(WorkOrderStatus.InProgress, technicianId, now, "started");
        }
    }

    public class WorkOrderTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = "";
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class LaborLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Task { get; set; } = "";
        public decimal Hours { get; set; }
        public DateTime At { get; set; } = DateTime.UtcNow;
        public Guid LoggedBy { get; set; }
    }

    public class MaterialUsage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Item { get; set; } = "";
        public decimal Quantity { get; set; }
        public DateTime At { get; set; } = DateTime.UtcNow;
        public Guid LoggedBy { get; set; }
    }
}