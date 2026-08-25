using VSRSystemsBackend.Application.Platform.Chat;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class ModuleChatContextAuthorizer(HomeServicesChatContextAuthorizer homeServicesAuthorizer)
    : IChatContextAuthorizer
{
    private static readonly HashSet<string> AdministrativeModules = new(StringComparer.OrdinalIgnoreCase)
    {
        "bank",
        "commerce",
        "hotel",
        "interior",
        "jobs",
        "medical",
        "news",
        "railway",
        "school",
        "travel",
        "warehouse"
    };

    public Task<AuthorizedChatContext?> AuthorizeAsync(
        string moduleKey,
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(moduleKey)
            || string.IsNullOrWhiteSpace(conversationId)
            || string.IsNullOrWhiteSpace(userId))
        {
            return Task.FromResult<AuthorizedChatContext?>(null);
        }

        var normalizedModuleKey = moduleKey.Trim().ToLowerInvariant();
        if (normalizedModuleKey == ChatModules.HomeServices)
        {
            return homeServicesAuthorizer.AuthorizeAsync(
                normalizedModuleKey,
                conversationId,
                userId,
                hasAdministrativeAccess,
                cancellationToken);
        }

        var context = hasAdministrativeAccess && AdministrativeModules.Contains(normalizedModuleKey)
            ? new AuthorizedChatContext(
                normalizedModuleKey,
                conversationId,
                $"{normalizedModuleKey}.message",
                conversationId)
            : null;
        return Task.FromResult(context);
    }
}
