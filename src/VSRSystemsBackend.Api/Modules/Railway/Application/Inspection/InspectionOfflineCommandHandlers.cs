using System.Text.Json;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Application.Inspection;

public sealed record OfflineStartInspection(Guid DivisionId, Guid AssignmentId, Guid TemplateId, Guid TargetId);
public sealed record OfflineSaveInspectionAnswer(string ItemId, string Response, double? Measurement);

public sealed class StartInspectionOfflineHandler(InspectionHandlers handlers) : IRailwayOfflineCommandHandler
{
    public string CommandType => "inspection.start";
    public async ValueTask<RailwayOfflineCommandResult> HandleAsync(RailwayScope scope, RailwayOfflineCommandEnvelope command, CancellationToken cancellationToken)
    {
        var request = command.Payload.Deserialize<OfflineStartInspection>() ?? throw new JsonException();
        var run = await handlers.StartRunAsync(scope, request.DivisionId, request.AssignmentId, request.TemplateId, request.TargetId, cancellationToken);
        return new(command.CommandId, RailwayOfflineCommandStatus.Accepted, run.Version);
    }
}

public sealed class SaveInspectionAnswerOfflineHandler(InspectionHandlers handlers) : IRailwayOfflineCommandHandler
{
    public string CommandType => "inspection.save-response";
    public async ValueTask<RailwayOfflineCommandResult> HandleAsync(RailwayScope scope, RailwayOfflineCommandEnvelope command, CancellationToken cancellationToken)
    {
        var request = command.Payload.Deserialize<OfflineSaveInspectionAnswer>() ?? throw new JsonException();
        var run = await handlers.SaveAnswerAsync(
            scope, command.AggregateId, request.ItemId, request.Response, request.Measurement,
            command.EvidenceIds, cancellationToken);
        return new(command.CommandId, RailwayOfflineCommandStatus.Accepted, run.Version);
    }
}

public sealed class SubmitInspectionOfflineHandler(InspectionHandlers handlers) : IRailwayOfflineCommandHandler
{
    public string CommandType => "inspection.submit";
    public async ValueTask<RailwayOfflineCommandResult> HandleAsync(RailwayScope scope, RailwayOfflineCommandEnvelope command, CancellationToken cancellationToken)
    {
        var run = await handlers.SubmitAsync(scope, command.AggregateId, cancellationToken);
        return new(command.CommandId, RailwayOfflineCommandStatus.Accepted, run.Version);
    }
}
