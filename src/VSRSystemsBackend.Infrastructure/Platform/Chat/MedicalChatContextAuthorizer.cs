using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class MedicalChatContextAuthorizer : IChatContextAuthorizer
{
    public const string TenantId = "medical";
    public const string ContextType = "medical.message";

    private readonly IRealtimeSubscriptionAuthorizer _bookingAuthorizer;

    public MedicalChatContextAuthorizer(IRealtimeSubscriptionAuthorizer bookingAuthorizer)
    {
        _bookingAuthorizer = bookingAuthorizer;
    }

    public async Task<AuthorizedChatContext?> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        // Medical messages - authorize based on admin access or valid userId
        var isAuthorized = hasAdministrativeAccess
            || !string.IsNullOrWhiteSpace(userId);

        return isAuthorized
            ? new AuthorizedChatContext(TenantId, conversationId, ContextType, conversationId)
            : null;
    }
}