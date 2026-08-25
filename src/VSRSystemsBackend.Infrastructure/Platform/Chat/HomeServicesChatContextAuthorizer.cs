using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class HomeServicesChatContextAuthorizer : IChatContextAuthorizer
{
    // Booking IDs are conversation IDs until relational conversation metadata is introduced.
    public const string TenantId = "home-services";
    public const string ContextType = "home-services.booking";

    private readonly IRealtimeSubscriptionAuthorizer _bookingAuthorizer;

    public HomeServicesChatContextAuthorizer(IRealtimeSubscriptionAuthorizer bookingAuthorizer)
    {
        _bookingAuthorizer = bookingAuthorizer;
    }

    public async Task<AuthorizedChatContext?> AuthorizeAsync(
        string moduleKey,
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        if (!string.Equals(moduleKey, ChatModules.HomeServices, StringComparison.OrdinalIgnoreCase))
            return null;

        var isAuthorized = await _bookingAuthorizer.CanSubscribeToHomeServicesBookingAsync(
            userId,
            hasAdministrativeAccess,
            conversationId,
            cancellationToken);

        return isAuthorized
            ? new AuthorizedChatContext(TenantId, conversationId, ContextType, conversationId)
            : null;
    }
}
