using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

public sealed class WorkOrder : RailwayEntity
{
    private readonly List<WorkOrderTask> tasks = [];
    private readonly List<WorkOrderHistory> history = [];
    private WorkOrder() { }

    public WorkOrder(Guid id, Guid organizationId, Guid divisionId, Guid sourceId, string sourceType, Guid targetId, WorkOrderPriority priority, bool safetyClassified, Guid createdBy, DateTimeOffset now)
        : base(id, organizationId, divisionId)
    {
        SourceId = sourceId;
        SourceType = sourceType;
        TargetId = targetId;
        Priority = priority;
        SafetyClassified = safetyClassified;
        CreatedBy = createdBy;
        CreatedAt = now;
        Record(createdBy, now, "created");
    }

    public Guid SourceId { get; private set; }
    public string SourceType { get; private set; } = string.Empty;
    public Guid TargetId { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public WorkOrderPriority Priority { get; private set; }
    public bool SafetyClassified { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid? ApprovedBy { get; private set; }
    public Guid? AssignedTo { get; private set; }
    public Guid? PermitEvidenceId { get; private set; }
    public Guid? VerifiedBy { get; private set; }
    public DateTimeOffset? ScheduledAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? BlockReason { get; private set; }
    public IReadOnlyCollection<WorkOrderTask> Tasks => tasks;
    public IReadOnlyCollection<WorkOrderHistory> History => history;

    public void AddTask(string description)
    {
        if (Status != WorkOrderStatus.Draft) throw new WorkOrderPolicyException("Tasks can only be added in Draft.");
        tasks.Add(new WorkOrderTask(Guid.NewGuid(), description));
        Version++;
    }

    public void Triage(Guid actor, DateTimeOffset now) => TransitionTo(WorkOrderStatus.Triaged, actor, now, "triaged");
    public void Approve(Guid actor, DateTimeOffset now) { TransitionTo(WorkOrderStatus.Approved, actor, now, "approved"); ApprovedBy = actor; }
    public void Schedule(Guid assignee, DateTimeOffset scheduledAt, Guid actor, DateTimeOffset now)
    { if (assignee == Guid.Empty) throw new WorkOrderPolicyException("An assignee is required."); TransitionTo(WorkOrderStatus.Scheduled, actor, now, "scheduled"); AssignedTo = assignee; ScheduledAt = scheduledAt; }
    public void AttachPermit(Guid evidenceId, Guid actor, DateTimeOffset now)
    { if (!SafetyClassified || Status is not (WorkOrderStatus.Approved or WorkOrderStatus.Scheduled)) throw new WorkOrderPolicyException("A permit can only be attached to approved safety work."); PermitEvidenceId = evidenceId; Record(actor, now, "permit-attached"); Version++; }
    public void Start(Guid actor, DateTimeOffset now)
    { if (AssignedTo != actor) throw new UnauthorizedAccessException("Only the assigned technician can start work."); if (SafetyClassified && (ApprovedBy is null || PermitEvidenceId is null)) throw new WorkOrderPolicyException("Safety work requires approval and a permit."); TransitionTo(WorkOrderStatus.InProgress, actor, now, "started"); }
    public void CompleteTask(Guid taskId, Guid actor, DateTimeOffset now)
    { RequireAssignedActive(actor); var task = tasks.SingleOrDefault(item => item.Id == taskId) ?? throw new KeyNotFoundException(); task.Complete(now); Record(actor, now, "task-completed"); Version++; }
    public void Block(string reason, Guid actor, DateTimeOffset now)
    { RequireAssignedActive(actor); if (string.IsNullOrWhiteSpace(reason)) throw new WorkOrderPolicyException("A block reason is required."); BlockReason = reason.Trim(); TransitionTo(WorkOrderStatus.Blocked, actor, now, reason); }
    public void Unblock(Guid actor, DateTimeOffset now) { if (AssignedTo != actor) throw new UnauthorizedAccessException(); TransitionTo(WorkOrderStatus.InProgress, actor, now, "unblocked"); BlockReason = null; }
    public void SubmitVerification(Guid actor, DateTimeOffset now)
    { RequireAssignedActive(actor); if (tasks.Count == 0 || tasks.Any(task => !task.IsCompleted)) throw new WorkOrderPolicyException("Every task must be completed."); TransitionTo(WorkOrderStatus.AwaitingVerification, actor, now, "submitted-verification"); }
    public void Verify(Guid verifier, DateTimeOffset now)
    { if (SafetyClassified && verifier == AssignedTo) throw new WorkOrderPolicyException("Safety work requires an independent verifier."); TransitionTo(WorkOrderStatus.Completed, verifier, now, "verified"); VerifiedBy = verifier; CompletedAt = now; }
    public void RejectVerification(Guid verifier, DateTimeOffset now, string reason) => TransitionTo(WorkOrderStatus.InProgress, verifier, now, reason);
    public void Cancel(Guid actor, DateTimeOffset now, string reason) => TransitionTo(WorkOrderStatus.Cancelled, actor, now, reason);

    public void TransitionTo(WorkOrderStatus next, Guid actor, DateTimeOffset now, string reason)
    {
        var allowed = Status switch
        {
            WorkOrderStatus.Draft => next is WorkOrderStatus.Triaged or WorkOrderStatus.Cancelled,
            WorkOrderStatus.Triaged => next is WorkOrderStatus.Approved or WorkOrderStatus.Cancelled,
            WorkOrderStatus.Approved => next is WorkOrderStatus.Scheduled or WorkOrderStatus.Cancelled,
            WorkOrderStatus.Scheduled => next is WorkOrderStatus.InProgress or WorkOrderStatus.Cancelled,
            WorkOrderStatus.InProgress => next is WorkOrderStatus.Blocked or WorkOrderStatus.AwaitingVerification,
            WorkOrderStatus.Blocked => next is WorkOrderStatus.InProgress or WorkOrderStatus.Cancelled,
            WorkOrderStatus.AwaitingVerification => next is WorkOrderStatus.Completed or WorkOrderStatus.InProgress,
            _ => false,
        };
        if (!allowed) throw new InvalidWorkOrderTransitionException(Status, next);
        Status = next;
        Record(actor, now, reason);
        Version++;
    }

    private void RequireAssignedActive(Guid actor)
    { if (AssignedTo != actor) throw new UnauthorizedAccessException(); if (Status != WorkOrderStatus.InProgress) throw new WorkOrderPolicyException("Work must be in progress."); }
    private void Record(Guid actor, DateTimeOffset now, string reason) => history.Add(new WorkOrderHistory(Guid.NewGuid(), Status, actor, now, reason));
}

public sealed class WorkOrderTask
{
    private WorkOrderTask() { }
    internal WorkOrderTask(Guid id, string description) { Id = id; Description = description; }
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    internal void Complete(DateTimeOffset now) { if (IsCompleted) throw new WorkOrderPolicyException("Task is already complete."); IsCompleted = true; CompletedAt = now; }
}

public sealed class WorkOrderHistory
{
    private WorkOrderHistory() { }
    internal WorkOrderHistory(Guid id, WorkOrderStatus status, Guid actorId, DateTimeOffset occurredAt, string reason)
    { Id = id; Status = status; ActorId = actorId; OccurredAt = occurredAt; Reason = reason; }
    public Guid Id { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public Guid ActorId { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public string Reason { get; private set; } = string.Empty;
}

public sealed class InvalidWorkOrderTransitionException(WorkOrderStatus from, WorkOrderStatus to)
    : Exception($"Invalid work-order transition from {from} to {to}.");
public sealed class WorkOrderPolicyException(string message) : Exception(message);
