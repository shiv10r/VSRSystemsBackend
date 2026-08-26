namespace VSRSystemsBackend.Api.Platform.Outbox;

public sealed class PlatformOutboxMessage
{
    private PlatformOutboxMessage() { }

    public PlatformOutboxMessage(
        Guid id,
        Guid organizationId,
        string eventName,
        int schemaVersion,
        string payload,
        string correlationId,
        Guid? causationId,
        DateTimeOffset occurredAt)
    {
        Id = id;
        OrganizationId = organizationId;
        EventName = eventName;
        SchemaVersion = schemaVersion;
        Payload = payload;
        CorrelationId = correlationId;
        CausationId = causationId;
        OccurredAt = occurredAt;
    }

    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string EventName { get; private set; } = string.Empty;
    public int SchemaVersion { get; private set; }
    public string Payload { get; private set; } = string.Empty;
    public string CorrelationId { get; private set; } = string.Empty;
    public Guid? CausationId { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public DateTimeOffset? DispatchedAt { get; private set; }
    public int Attempts { get; private set; }
    public string? LastError { get; private set; }
    public DateTimeOffset? LeaseUntil { get; private set; }
    public DateTimeOffset? DeadLetteredAt { get; private set; }
    public void Lease(DateTimeOffset until) { LeaseUntil = until; Attempts++; }
    public void MarkDispatched(DateTimeOffset now) { DispatchedAt = now; LeaseUntil = null; LastError = null; }
    public void MarkFailed(string error, DateTimeOffset now) { LastError = error[..Math.Min(error.Length, 2000)]; LeaseUntil = null; if (Attempts >= 10) DeadLetteredAt = now; }
}
