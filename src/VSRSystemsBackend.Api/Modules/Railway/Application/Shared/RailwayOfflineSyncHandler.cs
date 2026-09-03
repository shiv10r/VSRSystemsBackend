using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

public sealed record RailwayOfflineCommandEnvelope(
    Guid CommandId,
    string IdempotencyKey,
    Guid AggregateId,
    long ExpectedVersion,
    string CommandType,
    JsonElement Payload,
    DateTimeOffset CapturedAt,
    IReadOnlyList<Guid> EvidenceIds);

public enum RailwayOfflineCommandStatus { Accepted, Duplicate, Rejected, Conflicted }

public sealed record RailwayOfflineCommandResult(
    Guid CommandId,
    RailwayOfflineCommandStatus Status,
    long? AuthoritativeVersion = null,
    string? Code = null,
    string? Message = null);

public interface IRailwayOfflineCommandHandler
{
    string CommandType { get; }
    ValueTask<RailwayOfflineCommandResult> HandleAsync(
        RailwayScope scope,
        RailwayOfflineCommandEnvelope command,
        CancellationToken cancellationToken);
}

public sealed class RailwayOfflineCommandRegistry
{
    private readonly IReadOnlyDictionary<string, IRailwayOfflineCommandHandler> handlers;

    public RailwayOfflineCommandRegistry(IEnumerable<IRailwayOfflineCommandHandler> handlers)
    {
        var registrations = handlers.ToList();
        var duplicate = registrations.GroupBy(handler => handler.CommandType, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new InvalidOperationException($"Duplicate Railway offline command handler: {duplicate.Key}.");
        this.handlers = registrations.ToDictionary(handler => handler.CommandType, StringComparer.Ordinal);
    }

    public bool TryResolve(string commandType, out IRailwayOfflineCommandHandler handler) =>
        handlers.TryGetValue(commandType, out handler!);
}

public sealed class RailwayOfflineSyncHandler(
    RailwayDbContext dbContext,
    RailwayOfflineCommandRegistry registry)
{
    public async Task<IReadOnlyList<RailwayOfflineCommandResult>> HandleAsync(
        RailwayScope scope,
        IReadOnlyList<RailwayOfflineCommandEnvelope> commands,
        CancellationToken cancellationToken)
    {
        var results = new List<RailwayOfflineCommandResult>(commands.Count);
        foreach (var command in commands)
        {
            var existing = await dbContext.CommandReceipts.AsNoTracking().SingleOrDefaultAsync(
                receipt => receipt.UserId == scope.UserId && receipt.IdempotencyKey == command.IdempotencyKey,
                cancellationToken);
            if (existing is not null)
            {
                results.Add(new RailwayOfflineCommandResult(
                    command.CommandId, RailwayOfflineCommandStatus.Duplicate,
                    existing.AuthoritativeVersion, existing.Code, existing.Message));
                continue;
            }

            RailwayOfflineCommandResult result;
            if (!registry.TryResolve(command.CommandType, out var handler))
            {
                result = new RailwayOfflineCommandResult(
                    command.CommandId, RailwayOfflineCommandStatus.Rejected, Code: "unknown_command_type",
                    Message: "The offline command type is not registered.");
            }
            else
            {
                try
                {
                    result = await handler.HandleAsync(scope, command, cancellationToken);
                }
                catch (UnauthorizedAccessException)
                {
                    result = new RailwayOfflineCommandResult(
                        command.CommandId, RailwayOfflineCommandStatus.Rejected, Code: "forbidden",
                        Message: "The command is outside the authenticated Railway scope.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    result = new RailwayOfflineCommandResult(
                        command.CommandId, RailwayOfflineCommandStatus.Conflicted, Code: "version_conflict",
                        Message: "The authoritative record has changed.");
                }
            }

            dbContext.CommandReceipts.Add(new RailwayCommandReceipt(
                Guid.NewGuid(), scope.OrganizationId, scope.UserId, command.AggregateId,
                command.IdempotencyKey, command.CommandType, result.Status.ToString(),
                result.AuthoritativeVersion, result.Code, result.Message, DateTimeOffset.UtcNow));
            await dbContext.SaveChangesAsync(cancellationToken);
            results.Add(result);
        }
        return results;
    }
}
