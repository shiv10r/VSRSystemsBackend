using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VSRSystemsBackend.Infrastructure.Persistence.Mongo.Documents;

public sealed class ChatMessageDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("messageId")]
    public string MessageId { get; set; } = string.Empty;

    [BsonElement("tenantId")]
    public string TenantId { get; set; } = string.Empty;

    [BsonElement("conversationId")]
    public string ConversationId { get; set; } = string.Empty;

    [BsonElement("senderUserId")]
    public string SenderUserId { get; set; } = string.Empty;

    [BsonElement("messageType")]
    public string MessageType { get; set; } = "text";

    [BsonElement("text")]
    public string? Text { get; set; }

    [BsonElement("sentAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime SentAtUtc { get; set; }
}
