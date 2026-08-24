using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VSRSystemsBackend.Application.Platform.Chat;

public sealed class SendChatMessageRequest
{
    [Required]
    [MaxLength(4000)]
    public string Text { get; init; } = string.Empty;
}

public sealed record ChatMessageDto(
    Guid MessageId,
    string ConversationId,
    string SenderUserId,
    string MessageType,
    string? Text,
    DateTimeOffset SentAt);

public sealed record ChatMessagePageDto(
    IReadOnlyList<ChatMessageDto> Items,
    string? NextCursor);

public sealed record AuthorizedChatContext(
    string TenantId,
    string ConversationId,
    string ContextType,
    string ContextId);

public sealed record ChatMessageRecord(
    string StorageId,
    Guid MessageId,
    string TenantId,
    string ConversationId,
    string SenderUserId,
    string MessageType,
    string? Text,
    DateTimeOffset SentAt);

public sealed record ChatCursor(DateTimeOffset SentAt, string StorageId);

public static class ChatCursorCodec
{
    public static string Encode(ChatCursor cursor) =>
        $"{cursor.SentAt.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture)}.{cursor.StorageId}";

    public static bool TryDecode(string? value, out ChatCursor? cursor)
    {
        cursor = null;
        if (string.IsNullOrWhiteSpace(value))
            return true;

        var separator = value.IndexOf('.', StringComparison.Ordinal);
        if (separator <= 0 || separator == value.Length - 1)
            return false;

        var timestampPart = value[..separator];
        var storageId = value[(separator + 1)..];
        if (!long.TryParse(timestampPart, NumberStyles.None, CultureInfo.InvariantCulture, out var unixMilliseconds)
            || storageId.Length != 24
            || storageId.Any(character => !Uri.IsHexDigit(character)))
        {
            return false;
        }

        try
        {
            cursor = new ChatCursor(DateTimeOffset.FromUnixTimeMilliseconds(unixMilliseconds), storageId);
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }
}

public sealed class ChatAccessDeniedException : Exception
{
    public ChatAccessDeniedException() : base("You are not authorized to access this conversation.")
    {
    }
}

public sealed class ChatValidationException : Exception
{
    public ChatValidationException(string message) : base(message)
    {
    }
}

public sealed class ChatUnavailableException : Exception
{
    public ChatUnavailableException(Exception? innerException = null)
        : base("Messaging is temporarily unavailable.", innerException)
    {
    }
}
