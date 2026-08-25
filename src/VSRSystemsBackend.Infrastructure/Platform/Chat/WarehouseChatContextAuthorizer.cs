using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class WarehouseChatContextAuthorizer : IChatContextAuthorizer
{
    public const string TenantId = "warehouse";
    public const string ContextType = "warehouse.message";

    private readonly IRealtimeSubscriptionAuthorizer _bookingAuthorizer;

    public WarehouseChatContextAuthorizer(IRealtimeSubscriptionAuthorizer bookingAuthorizer)
    {
        _bookingAuthorizer = bookingAuthorizer;
    }

    public async Task<AuthorizedChatContext?> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        // Warehouse messages are linked to projects, stock items, or logistics
        // Authorize based on admin access or valid userId
        var isAuthorized = hasAdministrativeAccess
            || !string.IsNullOrWhiteSpace(userId);

        return isAuthorized
            ? new AuthorizedChatContext(TenantId, conversationId, ContextType, conversationId)
            : null;
    }
}