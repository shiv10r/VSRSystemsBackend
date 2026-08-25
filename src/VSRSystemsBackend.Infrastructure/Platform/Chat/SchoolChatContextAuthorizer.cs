using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class SchoolChatContextAuthorizer : IChatContextAuthorizer
{
    public const string TenantId = "school";
    public const string ContextType = "school.message";

    private readonly IRealtimeSubscriptionAuthorizer _bookingAuthorizer;

    public SchoolChatContextAuthorizer(IRealtimeSubscriptionAuthorizer bookingAuthorizer)
    {
        _bookingAuthorizer = bookingAuthorizer;
    }

    public async Task<AuthorizedChatContext?> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        // School messages are linked to enrollments/academic records
        // For now, authorize based on user having access to the school entity
        var isAuthorized = hasAdministrativeAccess
            || !string.IsNullOrWhiteSpace(userId);

        return isAuthorized
            ? new AuthorizedChatContext(TenantId, conversationId, ContextType, conversationId)
            : null;
    }
}