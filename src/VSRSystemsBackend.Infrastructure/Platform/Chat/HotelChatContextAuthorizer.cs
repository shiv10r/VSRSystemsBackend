using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class HotelChatContextAuthorizer : IChatContextAuthorizer
{
    public const string TenantId = "hotel";
    public const string ContextType = "hotel.message";

    private readonly IRealtimeSubscriptionAuthorizer _bookingAuthorizer;

    public HotelChatContextAuthorizer(IRealtimeSubscriptionAuthorizer bookingAuthorizer)
    {
        _bookingAuthorizer = bookingAuthorizer;
    }

    public async Task<AuthorizedChatContext?> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        // Hotel messages are linked to reservations, rooms, or guests
        // Authorize based on admin access or valid userId
        var isAuthorized = hasAdministrativeAccess
            || !string.IsNullOrWhiteSpace(userId);

        return isAuthorized
            ? new AuthorizedChatContext(TenantId, conversationId, ContextType, conversationId)
            : null;
    }
}