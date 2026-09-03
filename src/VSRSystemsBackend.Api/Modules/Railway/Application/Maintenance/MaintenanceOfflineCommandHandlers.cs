using System.Text.Json;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Application.Maintenance;

public sealed record OfflineMaintenanceAction(Guid? AssigneeId = null, Guid? EvidenceId = null, Guid? TaskId = null, string? Reason = null);

public abstract class MaintenanceOfflineCommandHandler(MaintenanceHandlers handlers) : IRailwayOfflineCommandHandler
{
    public abstract string CommandType { get; }
    protected abstract string Action { get; }
    public async ValueTask<RailwayOfflineCommandResult> HandleAsync(RailwayScope scope, RailwayOfflineCommandEnvelope command, CancellationToken cancellationToken)
    {
        var request = command.Payload.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined
            ? new OfflineMaintenanceAction()
            : command.Payload.Deserialize<OfflineMaintenanceAction>() ?? new OfflineMaintenanceAction();
        var evidenceId = request.EvidenceId ?? command.EvidenceIds.FirstOrDefault();
        var order = await handlers.ExecuteAsync(scope, command.AggregateId, Action, request.AssigneeId,
            evidenceId == Guid.Empty ? null : evidenceId, request.TaskId, request.Reason, cancellationToken);
        return new(command.CommandId, RailwayOfflineCommandStatus.Accepted, order.Version);
    }
}

public sealed class StartWorkOfflineHandler(MaintenanceHandlers handlers) : MaintenanceOfflineCommandHandler(handlers)
{ public override string CommandType => "work-order.start"; protected override string Action => "start"; }
public sealed class CompleteWorkTaskOfflineHandler(MaintenanceHandlers handlers) : MaintenanceOfflineCommandHandler(handlers)
{ public override string CommandType => "work-order.complete-task"; protected override string Action => "complete-task"; }
public sealed class AttachWorkPermitOfflineHandler(MaintenanceHandlers handlers) : MaintenanceOfflineCommandHandler(handlers)
{ public override string CommandType => "work-order.attach-permit"; protected override string Action => "permit"; }
public sealed class BlockWorkOfflineHandler(MaintenanceHandlers handlers) : MaintenanceOfflineCommandHandler(handlers)
{ public override string CommandType => "work-order.block"; protected override string Action => "block"; }
public sealed class UnblockWorkOfflineHandler(MaintenanceHandlers handlers) : MaintenanceOfflineCommandHandler(handlers)
{ public override string CommandType => "work-order.unblock"; protected override string Action => "unblock"; }
public sealed class SubmitWorkVerificationOfflineHandler(MaintenanceHandlers handlers) : MaintenanceOfflineCommandHandler(handlers)
{ public override string CommandType => "work-order.submit-verification"; protected override string Action => "submit-verification"; }
