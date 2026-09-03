namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

public sealed class RailwayAuditRecord
{
    private RailwayAuditRecord() { }
    public RailwayAuditRecord(Guid id, Guid organizationId, Guid? divisionId, Guid actorId, string action, string resourceType,
        string resourceId, string? beforeJson, string? afterJson, string correlationId, DateTimeOffset occurredAt)
    { Id = id; OrganizationId = organizationId; DivisionId = divisionId; ActorId = actorId; Action = action; ResourceType = resourceType; ResourceId = resourceId; BeforeJson = beforeJson; AfterJson = afterJson; CorrelationId = correlationId; OccurredAt = occurredAt; }
    public Guid Id { get; private set; } public Guid OrganizationId { get; private set; } public Guid? DivisionId { get; private set; }
    public Guid ActorId { get; private set; } public string Action { get; private set; } = string.Empty; public string ResourceType { get; private set; } = string.Empty;
    public string ResourceId { get; private set; } = string.Empty; public string? BeforeJson { get; private set; } public string? AfterJson { get; private set; }
    public string CorrelationId { get; private set; } = string.Empty; public DateTimeOffset OccurredAt { get; private set; }
}
