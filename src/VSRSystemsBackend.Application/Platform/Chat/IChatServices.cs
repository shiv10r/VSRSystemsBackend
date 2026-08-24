namespace VSRSystemsBackend.Application.Platform.Chat;

public interface IChatContextAuthorizer
{
    Task<AuthorizedChatContext?> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken = default);
}

public interface IChatMessageRepository
{
    Task<ChatMessageRecord> InsertAsync(
        ChatMessageRecord message,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChatMessageRecord>> GetMessagesAsync(
        string tenantId,
        string conversationId,
        ChatCursor? before,
        int limit,
        CancellationToken cancellationToken = default);
}

public interface IChatService
{
    Task<ChatMessageDto> SendMessageAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        SendChatMessageRequest request,
        string? correlationId,
        CancellationToken cancellationToken = default);

    Task<ChatMessagePageDto> GetMessagesAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        string? before,
        int limit,
        CancellationToken cancellationToken = default);
}
