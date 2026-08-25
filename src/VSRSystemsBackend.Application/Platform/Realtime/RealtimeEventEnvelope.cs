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
    public const string PlatformChatTypingIndicator = "platform.chat.typing-indicator";
}

public static class RealtimeGroups
{
    public const string ClientMethod = "realtimeEvent";

    public static string User(string userId) => $"user:{userId}";
    public static string Tenant(string tenantId) => $"tenant:{tenantId}";
    public static string HomeServicesBooking(string bookingId) => $"context:home-services.booking:{bookingId}";
    public static string ChatConversation(string tenantId, string conversationId) =>
        $"context:chat.conversation:{tenantId}:{conversationId}";
    public static string SchoolMessage(string conversationId) =>
        $"context:school.message:{conversationId}";
    public static string WarehouseMessage(string conversationId) =>
        $"context:warehouse.message:{conversationId}";
    public static string HomeServicesMessage(string conversationId) =>
        $"context:home-services.message:{conversationId}";
    public static string TravelMessage(string conversationId) =>
        $"context:travel.message:{conversationId}";
    public static string RailwayMessage(string conversationId) =>
        $"context:railway.message:{conversationId}";
    public static string HotelMessage(string conversationId) =>
        $"context:hotel.message:{conversationId}";
    public static string NewsMessage(string conversationId) =>
        $"context:news.message:{conversationId}";
    public static string JobsMessage(string conversationId) =>
        $"context:jobs.message:{conversationId}";
    public static string CommerceMessage(string conversationId) =>
        $"context:commerce.message:{conversationId}";
    public static string BankMessage(string conversationId) =>
        $"context:bank.message:{conversationId}";
    public static string MedicalMessage(string conversationId) =>
        $"context:medical.message:{conversationId}";
    public static string Presence(string tenantId) => $"presence:{tenantId}";
}
