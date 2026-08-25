using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class TravelChatContextAuthorizer : IChatContextAuthorizer
{
    public const string TenantId = "travel";
    public const string ContextType = "travel.message";

    private readonly IRealtimeSubscriptionAuthorizer _bookingAuthorizer;

    public TravelChatContextAuthorizer(IRealtimeSubscriptionAuthorizer bookingAuthorizer)
    {
        _bookingAuthorizer = bookingAuthorizer;
    }

    public async Task<AuthorizedChatContext?> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        // Travel messages are linked to trips, destinations, or bookings
        // Authorize based on admin access or valid userId
        var isAuthorized = hasAdministrativeAccess
            || !string.IsNullOrWhiteSpace(userId);

        return isAuthorized
            ? new AuthorizedChatContext(TenantId, conversationId, ContextType, conversationId)
            : null;
    }
}