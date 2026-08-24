namespace VSRSystemsBackend.Application.Platform.Realtime;

public sealed record RealtimeEventEnvelope<T>(
    Guid EventId,
    string EventType,
    int Version,
    DateTimeOffset OccurredAt,
    string? CorrelationId,
    string? TenantId,
    T Payload);

public static class RealtimeEventTypes
{
    public const string HomeServicesBookingStatusChanged = "home-services.booking.status-changed";
    public const string PlatformChatMessageCreated = "platform.chat.message-created";
}

public static class RealtimeGroups
{
    public const string ClientMethod = "realtimeEvent";

    public static string User(string userId) => $"user:{userId}";
    public static string Tenant(string tenantId) => $"tenant:{tenantId}";
    public static string HomeServicesBooking(string bookingId) => $"context:home-services.booking:{bookingId}";
    public static string ChatConversation(string tenantId, string conversationId) =>
        $"context:chat.conversation:{tenantId}:{conversationId}";
}
