using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;
using Xunit;

namespace VSRSystemsBackend.UnitTests.Modules.Railway;

public sealed class WorkOrderDomainTests
{
    [Fact]
    public void Draft_cannot_transition_directly_to_in_progress()
    {
        var order = Create(false);

        Assert.Throws<InvalidWorkOrderTransitionException>(() =>
            order.TransitionTo(WorkOrderStatus.InProgress, Guid.NewGuid(), DateTimeOffset.UtcNow, "invalid"));
    }

    [Fact]
    public void Completed_work_is_immutable()
    {
        var technician = Guid.NewGuid();
        var order = Complete(Create(false), technician, technician);

        Assert.Throws<InvalidWorkOrderTransitionException>(() =>
            order.TransitionTo(WorkOrderStatus.InProgress, technician, DateTimeOffset.UtcNow, "invalid"));
    }

    [Fact]
    public void Safety_work_requires_permit_and_independent_verifier()
    {
        var technician = Guid.NewGuid();
        var order = Create(true);
        var taskId = order.Tasks.Single().Id;
        order.Triage(Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Approve(Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Schedule(technician, DateTimeOffset.UtcNow, Guid.NewGuid(), DateTimeOffset.UtcNow);

        Assert.Throws<WorkOrderPolicyException>(() => order.Start(technician, DateTimeOffset.UtcNow));
        order.AttachPermit(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Start(technician, DateTimeOffset.UtcNow);
        order.CompleteTask(taskId, technician, DateTimeOffset.UtcNow);
        order.SubmitVerification(technician, DateTimeOffset.UtcNow);

        Assert.Throws<WorkOrderPolicyException>(() => order.Verify(technician, DateTimeOffset.UtcNow));
    }

    private static WorkOrder Create(bool safety)
    {
        var order = new WorkOrder(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Defect", Guid.NewGuid(),
            WorkOrderPriority.High, safety, Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.AddTask("Repair");
        return order;
    }

    private static WorkOrder Complete(WorkOrder order, Guid technician, Guid verifier)
    {
        var taskId = order.Tasks.Single().Id;
        order.Triage(Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Approve(Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Schedule(technician, DateTimeOffset.UtcNow, Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Start(technician, DateTimeOffset.UtcNow);
        order.CompleteTask(taskId, technician, DateTimeOffset.UtcNow);
        order.SubmitVerification(technician, DateTimeOffset.UtcNow);
        order.Verify(verifier, DateTimeOffset.UtcNow);
        return order;
    }
}
