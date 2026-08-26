using System.Text.Json;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Platform.Outbox;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

public interface IRailwayEventPublisher
{
    void Enqueue(IRailwayDomainEvent domainEvent, object payload);
}

public sealed class RailwayEventPublisher(RailwayDbContext dbContext) : IRailwayEventPublisher
{
    public void Enqueue(IRailwayDomainEvent domainEvent, object payload)
    {
        dbContext.OutboxMessages.Add(new PlatformOutboxMessage(
            domainEvent.EventId,
            domainEvent.OrganizationId,
            domainEvent.EventName,
            domainEvent.SchemaVersion,
            JsonSerializer.Serialize(payload),
            domainEvent.CorrelationId,
            domainEvent.CausationId,
            domainEvent.OccurredAt));
    }
}
