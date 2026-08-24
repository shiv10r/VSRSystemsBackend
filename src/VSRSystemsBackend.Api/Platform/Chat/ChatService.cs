using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Api.Platform.Chat;

public sealed class ChatService : IChatService
{
    private const int MaximumPageSize = 100;

    private readonly IChatContextAuthorizer _contextAuthorizer;
    private readonly IChatMessageRepository _messages;
    private readonly IRealtimePublisher _realtimePublisher;
    private readonly ILogger<ChatService> _logger;

    public ChatService(
        IChatContextAuthorizer contextAuthorizer,
        IChatMessageRepository messages,
        IRealtimePublisher realtimePublisher,
        ILogger<ChatService> logger)
    {
        _contextAuthorizer = contextAuthorizer;
        _messages = messages;
        _realtimePublisher = realtimePublisher;
        _logger = logger;
    }

    public async Task<ChatMessageDto> SendMessageAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        SendChatMessageRequest request,
        string? correlationId,
        CancellationToken cancellationToken = default)
    {
        var context = await AuthorizeAsync(
            conversationId,
            userId,
            hasAdministrativeAccess,
            cancellationToken);
        var text = request.Text?.Trim();
        if (string.IsNullOrWhiteSpace(text))
            throw new ChatValidationException("Message text is required.");
        if (text.Length > 4000)
            throw new ChatValidationException("Message text cannot exceed 4000 characters.");

        var saved = await _messages.InsertAsync(
            new ChatMessageRecord(
                string.Empty,
                Guid.NewGuid(),
                context.TenantId,
                context.ConversationId,
                userId,
                "text",
                text,
                DateTimeOffset.UtcNow),
            cancellationToken);
        var response = ToDto(saved);
        var message = new RealtimeEventEnvelope<ChatMessageDto>(
            Guid.NewGuid(),
            RealtimeEventTypes.PlatformChatMessageCreated,
            1,
            DateTimeOffset.UtcNow,
            correlationId,
            context.TenantId,
            response);

        try
        {
            await _realtimePublisher.SendToGroupAsync(
                RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId),
                message,
                CancellationToken.None);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                exception,
                "Chat message {MessageId} persisted but realtime publication failed",
                saved.MessageId);
        }

        return response;
    }

    public async Task<ChatMessagePageDto> GetMessagesAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        string? before,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var context = await AuthorizeAsync(
            conversationId,
            userId,
            hasAdministrativeAccess,
            cancellationToken);
        if (!ChatCursorCodec.TryDecode(before, out var cursor))
            throw new ChatValidationException("The pagination cursor is invalid.");

        var pageSize = Math.Clamp(limit, 1, MaximumPageSize);
        var records = await _messages.GetMessagesAsync(
            context.TenantId,
            context.ConversationId,
            cursor,
            pageSize + 1,
            cancellationToken);
        var hasMore = records.Count > pageSize;
        var page = records.Take(pageSize).ToList();
        var nextCursor = hasMore && page.Count > 0
            ? ChatCursorCodec.Encode(new ChatCursor(page[^1].SentAt, page[^1].StorageId))
            : null;

        return new ChatMessagePageDto(page.Select(ToDto).ToList(), nextCursor);
    }

    private async Task<AuthorizedChatContext> AuthorizeAsync(
        string conversationId,
        string userId,
        bool hasAdministrativeAccess,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(conversationId) || string.IsNullOrWhiteSpace(userId))
            throw new ChatAccessDeniedException();

        return await _contextAuthorizer.AuthorizeAsync(
                conversationId,
                userId,
                hasAdministrativeAccess,
                cancellationToken)
            ?? throw new ChatAccessDeniedException();
    }

    private static ChatMessageDto ToDto(ChatMessageRecord message) => new(
        message.MessageId,
        message.ConversationId,
        message.SenderUserId,
        message.MessageType,
        message.Text,
        message.SentAt);
}
