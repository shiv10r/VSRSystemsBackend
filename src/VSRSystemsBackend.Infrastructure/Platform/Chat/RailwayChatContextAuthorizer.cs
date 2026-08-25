using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class RailwayChatContextAuthorizer : IChatContextAuthorizer
{
    public const string TenantId = "railway";
    public const string ContextType = "railway.message";

    private readonly IRealtimeSubscriptionAuthorizer _bookingAuthorizer;

    public RailwayChatContextAuthorizer(IRealtimeSubscriptionAuthorizer bookingAuthorizer)
    {
        _bookingAuthorizer = bookingAuthorizer;
    }

    public async Task<AuthorizedChatContext?> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        // Railway messages - authorize based on admin access or valid userId
        // Railway module has no dedicated backend APIs, so this provides
        // the standard authorization framework for future extension
        var isAuthorized = hasAdministrativeAccess
            || !string.IsNullOrWhiteSpace(userId);

        return isAuthorized
            ? new AuthorizedChatContext(TenantId, conversationId, ContextType, conversationId)
            : null;
    }
}