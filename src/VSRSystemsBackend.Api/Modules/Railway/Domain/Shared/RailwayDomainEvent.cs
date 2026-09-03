namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

public interface IRailwayDomainEvent
{
    Guid EventId { get; }
    string EventName { get; }
    int SchemaVersion { get; }
    Guid OrganizationId { get; }
    DateTimeOffset OccurredAt { get; }
    string CorrelationId { get; }
    Guid? CausationId { get; }
}

public sealed record RailwayDomainEvent(
    Guid EventId,
    string EventName,
    int SchemaVersion,
    Guid OrganizationId,
    DateTimeOffset OccurredAt,
    string CorrelationId,
    Guid? CausationId = null) : IRailwayDomainEvent;
