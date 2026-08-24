using MongoDB.Bson;
using MongoDB.Driver;
using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Infrastructure.Persistence.Mongo;
using VSRSystemsBackend.Infrastructure.Persistence.Mongo.Documents;

namespace VSRSystemsBackend.Infrastructure.Platform.Chat;

public sealed class MongoChatMessageRepository : IChatMessageRepository
{
    private readonly MongoDbContext _mongoDb;
    private readonly SemaphoreSlim _indexLock = new(1, 1);
    private bool _indexesReady;

    public MongoChatMessageRepository(MongoDbContext mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<ChatMessageRecord> InsertAsync(
        ChatMessageRecord message,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var collection = _mongoDb.GetCollection<ChatMessageDocument>(MongoCollectionNames.ChatMessages);
            await EnsureIndexesAsync(collection, cancellationToken);

            var document = new ChatMessageDocument
            {
                Id = ObjectId.GenerateNewId(),
                MessageId = message.MessageId.ToString("D"),
                TenantId = message.TenantId,
                ConversationId = message.ConversationId,
                SenderUserId = message.SenderUserId,
                MessageType = message.MessageType,
                Text = message.Text,
                SentAtUtc = message.SentAt.UtcDateTime
            };

            await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
            return ToRecord(document);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new ChatUnavailableException(exception);
        }
    }

    public async Task<IReadOnlyList<ChatMessageRecord>> GetMessagesAsync(
        string tenantId,
        string conversationId,
        ChatCursor? before,
        int limit,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var collection = _mongoDb.GetCollection<ChatMessageDocument>(MongoCollectionNames.ChatMessages);
            await EnsureIndexesAsync(collection, cancellationToken);

            var filters = Builders<ChatMessageDocument>.Filter;
            var filter = filters.Eq(document => document.TenantId, tenantId)
                & filters.Eq(document => document.ConversationId, conversationId);

            if (before is not null)
            {
                var cursorId = ObjectId.Parse(before.StorageId);
                var cursorTime = before.SentAt.UtcDateTime;
                var cursorFilter = filters.Lt(document => document.SentAtUtc, cursorTime)
                    | (filters.Eq(document => document.SentAtUtc, cursorTime)
                        & filters.Lt(document => document.Id, cursorId));
                filter &= cursorFilter;
            }

            var documents = await collection.Find(filter)
                .SortByDescending(document => document.SentAtUtc)
                .ThenByDescending(document => document.Id)
                .Limit(limit)
                .ToListAsync(cancellationToken);

            return documents.Select(ToRecord).ToList();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new ChatUnavailableException(exception);
        }
    }

    private async Task EnsureIndexesAsync(
        IMongoCollection<ChatMessageDocument> collection,
        CancellationToken cancellationToken)
    {
        if (_indexesReady)
            return;

        await _indexLock.WaitAsync(cancellationToken);
        try
        {
            if (_indexesReady)
                return;

            var keys = Builders<ChatMessageDocument>.IndexKeys;
            await collection.Indexes.CreateManyAsync(
                new[]
                {
                    new CreateIndexModel<ChatMessageDocument>(
                        keys.Ascending(document => document.TenantId)
                            .Ascending(document => document.ConversationId)
                            .Descending(document => document.SentAtUtc)
                            .Descending(document => document.Id),
                        new CreateIndexOptions { Name = "tenant_conversation_sent_at" }),
                    new CreateIndexModel<ChatMessageDocument>(
                        keys.Ascending(document => document.TenantId)
                            .Ascending(document => document.MessageId),
                        new CreateIndexOptions
                        {
                            Name = "tenant_message_id_unique",
                            Unique = true
                        })
                },
                cancellationToken);
            _indexesReady = true;
        }
        finally
        {
            _indexLock.Release();
        }
    }

    private static ChatMessageRecord ToRecord(ChatMessageDocument document) => new(
        document.Id.ToString(),
        Guid.Parse(document.MessageId),
        document.TenantId,
        document.ConversationId,
        document.SenderUserId,
        document.MessageType,
        document.Text,
        new DateTimeOffset(DateTime.SpecifyKind(document.SentAtUtc, DateTimeKind.Utc)));
}
