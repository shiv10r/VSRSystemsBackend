using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

public sealed class RailwayCommandReceipt : RailwayEntity
{
    private RailwayCommandReceipt() { }

    public RailwayCommandReceipt(
        Guid id,
        Guid organizationId,
        Guid userId,
        Guid aggregateId,
        string idempotencyKey,
        string commandType,
        string status,
        long? authoritativeVersion,
        string? code,
        string? message,
        DateTimeOffset processedAt)
        : base(id, organizationId)
    {
        UserId = userId;
        AggregateId = aggregateId;
        IdempotencyKey = idempotencyKey;
        CommandType = commandType;
        Status = status;
        AuthoritativeVersion = authoritativeVersion;
        Code = code;
        Message = message;
        ProcessedAt = processedAt;
    }

    public Guid UserId { get; private set; }
    public Guid AggregateId { get; private set; }
    public string IdempotencyKey { get; private set; } = string.Empty;
    public string CommandType { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public long? AuthoritativeVersion { get; private set; }
    public string? Code { get; private set; }
    public string? Message { get; private set; }
    public DateTimeOffset ProcessedAt { get; private set; }
}
