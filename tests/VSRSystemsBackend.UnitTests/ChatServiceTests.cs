using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VSRSystemsBackend.Api.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;
using VSRSystemsBackend.Infrastructure.Persistence.Mongo;
using VSRSystemsBackend.Infrastructure.Platform.Chat;
using Xunit;

namespace VSRSystemsBackend.UnitTests;

public sealed class ChatServiceTests
{
    [Fact]
    public async Task SendPersistsAuthorizedTenantBeforeRealtimePublication()
    {
        var contextAuthorizer = AuthorizedContext();
        var repository = new Mock<IChatMessageRepository>();
        var publisher = new Mock<IRealtimePublisher>();
        var calls = new List<string>();

        repository.Setup(store => store.InsertAsync(It.IsAny<ChatMessageRecord>(), It.IsAny<CancellationToken>()))
            .Callback<ChatMessageRecord, CancellationToken>((message, _) =>
            {
                calls.Add("persist");
                Assert.Equal("home-services", message.TenantId);
                Assert.Equal("booking-1", message.ConversationId);
                Assert.Equal("user-1", message.SenderUserId);
            })
            .ReturnsAsync((ChatMessageRecord message, CancellationToken _) => message with
            {
                StorageId = "64b64c3f2f6f8f0f0f0f0f0f"
            });
        publisher.Setup(realtime => realtime.SendToGroupAsync(
                It.IsAny<string>(),
                It.IsAny<RealtimeEventEnvelope<ChatMessageDto>>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, RealtimeEventEnvelope<ChatMessageDto>, CancellationToken>((group, message, _) =>
            {
                calls.Add("publish");
                Assert.Equal("context:chat.conversation:home-services:booking-1", group);
                Assert.Equal("platform.chat.message-created", message.EventType);
                Assert.Equal("home-services", message.TenantId);
                Assert.Equal("trace-1", message.CorrelationId);
            })
            .Returns(Task.CompletedTask);
        var service = CreateService(contextAuthorizer, repository, publisher);

        var result = await service.SendMessageAsync(
            ChatModules.HomeServices,
            "booking-1",
            "user-1",
            false,
            new SendChatMessageRequest { Text = "  On my way  " },
            "trace-1");

        Assert.Equal(new[] { "persist", "publish" }, calls);
        Assert.Equal("On my way", result.Text);
    }

    [Fact]
    public async Task RealtimeFailureDoesNotFailAlreadyPersistedMessage()
    {
        var repository = new Mock<IChatMessageRepository>();
        repository.Setup(store => store.InsertAsync(It.IsAny<ChatMessageRecord>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatMessageRecord message, CancellationToken _) => message with
            {
                StorageId = "64b64c3f2f6f8f0f0f0f0f0f"
            });
        var publisher = new Mock<IRealtimePublisher>();
        publisher.Setup(realtime => realtime.SendToGroupAsync(
                It.IsAny<string>(),
                It.IsAny<RealtimeEventEnvelope<ChatMessageDto>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("transport unavailable"));
        var service = CreateService(AuthorizedContext(), repository, publisher);

        var result = await service.SendMessageAsync(
            ChatModules.HomeServices,
            "booking-1",
            "user-1",
            false,
            new SendChatMessageRequest { Text = "Saved message" },
            null);

        Assert.Equal("Saved message", result.Text);
        repository.Verify(store => store.InsertAsync(It.IsAny<ChatMessageRecord>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PersistenceFailureNeverPublishesRealtimeEvent()
    {
        var repository = new Mock<IChatMessageRepository>();
        repository.Setup(store => store.InsertAsync(It.IsAny<ChatMessageRecord>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ChatUnavailableException());
        var publisher = new Mock<IRealtimePublisher>();
        var service = CreateService(AuthorizedContext(), repository, publisher);

        await Assert.ThrowsAsync<ChatUnavailableException>(() => service.SendMessageAsync(
            ChatModules.HomeServices,
            "booking-1",
            "user-1",
            false,
            new SendChatMessageRequest { Text = "Not saved" },
            null));

        publisher.Verify(realtime => realtime.SendToGroupAsync(
            It.IsAny<string>(),
            It.IsAny<RealtimeEventEnvelope<ChatMessageDto>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeniedConversationNeverReadsOrWritesMongo()
    {
        var contextAuthorizer = new Mock<IChatContextAuthorizer>();
        contextAuthorizer.Setup(authorizer => authorizer.AuthorizeAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((AuthorizedChatContext?)null);
        var repository = new Mock<IChatMessageRepository>();
        var service = CreateService(
            contextAuthorizer,
            repository,
            new Mock<IRealtimePublisher>());

        await Assert.ThrowsAsync<ChatAccessDeniedException>(() => service.GetMessagesAsync(
            ChatModules.HomeServices,
            "booking-1",
            "other-user",
            false,
            null,
            30));

        repository.Verify(store => store.GetMessagesAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<ChatCursor?>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HistoryUsesTenantScopeAndStableCursorPagination()
    {
        var repository = new Mock<IChatMessageRepository>();
        var sentAt = DateTimeOffset.FromUnixTimeMilliseconds(1_700_000_000_000);
        var records = new[]
        {
            Record("64b64c3f2f6f8f0f0f0f0f01", sentAt.AddMinutes(2)),
            Record("64b64c3f2f6f8f0f0f0f0f02", sentAt.AddMinutes(1)),
            Record("64b64c3f2f6f8f0f0f0f0f03", sentAt)
        };
        repository.Setup(store => store.GetMessagesAsync(
                "home-services",
                "booking-1",
                null,
                3,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(records);
        var service = CreateService(
            AuthorizedContext(),
            repository,
            new Mock<IRealtimePublisher>());

        var page = await service.GetMessagesAsync(ChatModules.HomeServices, "booking-1", "user-1", false, null, 2);

        Assert.Equal(2, page.Items.Count);
        Assert.True(ChatCursorCodec.TryDecode(page.NextCursor, out var cursor));
        Assert.Equal(records[1].StorageId, cursor!.StorageId);
        Assert.Equal(records[1].SentAt, cursor.SentAt);
    }

    [Fact]
    public async Task InvalidCursorIsRejectedBeforeMongoQuery()
    {
        var repository = new Mock<IChatMessageRepository>();
        var service = CreateService(
            AuthorizedContext(),
            repository,
            new Mock<IRealtimePublisher>());

        await Assert.ThrowsAsync<ChatValidationException>(() => service.GetMessagesAsync(
            ChatModules.HomeServices,
            "booking-1",
            "user-1",
            false,
            "not-a-cursor",
            30));

        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HomeServicesContextIsReturnedOnlyAfterBookingAuthorization()
    {
        var bookingAuthorizer = new Mock<IRealtimeSubscriptionAuthorizer>();
        bookingAuthorizer.Setup(authorizer => authorizer.CanSubscribeToHomeServicesBookingAsync(
                "user-1",
                false,
                "booking-1",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var authorizer = new HomeServicesChatContextAuthorizer(bookingAuthorizer.Object);

        var context = await authorizer.AuthorizeAsync(ChatModules.HomeServices, "booking-1", "user-1", false);
        var denied = await authorizer.AuthorizeAsync(ChatModules.HomeServices, "booking-2", "user-1", false);

        Assert.NotNull(context);
        Assert.Equal("home-services", context.TenantId);
        Assert.Equal("home-services.booking", context.ContextType);
        Assert.Null(denied);
    }

    [Fact]
    public async Task ModuleContextRequiresAdministrativeAccessUntilOwnershipProviderExists()
    {
        var bookingAuthorizer = new Mock<IRealtimeSubscriptionAuthorizer>();
        var authorizer = new ModuleChatContextAuthorizer(
            new HomeServicesChatContextAuthorizer(bookingAuthorizer.Object));

        var context = await authorizer.AuthorizeAsync("interior", "project-1", "user-1", true);
        var denied = await authorizer.AuthorizeAsync("interior", "project-1", "user-1", false);
        var unknown = await authorizer.AuthorizeAsync("unknown", "project-1", "user-1", true);

        Assert.NotNull(context);
        Assert.Equal("interior", context.TenantId);
        Assert.Equal("interior.message", context.ContextType);
        Assert.Null(denied);
        Assert.Null(unknown);
    }

    [Fact]
    public async Task UnconfiguredMongoRepositoryFailsAsOptionalChatCapability()
    {
        var mongoDb = new MongoDbContext(new MongoDbOptions(), null, null);
        var repository = new MongoChatMessageRepository(mongoDb);

        await Assert.ThrowsAsync<ChatUnavailableException>(() => repository.GetMessagesAsync(
            "home-services",
            "booking-1",
            null,
            30));
    }

    private static ChatService CreateService(
        Mock<IChatContextAuthorizer> contextAuthorizer,
        Mock<IChatMessageRepository> repository,
        Mock<IRealtimePublisher> publisher) =>
        new(
            contextAuthorizer.Object,
            repository.Object,
            publisher.Object,
            NullLogger<ChatService>.Instance);

    private static Mock<IChatContextAuthorizer> AuthorizedContext()
    {
        var contextAuthorizer = new Mock<IChatContextAuthorizer>();
        contextAuthorizer.Setup(authorizer => authorizer.AuthorizeAsync(
                ChatModules.HomeServices,
                "booking-1",
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AuthorizedChatContext(
                "home-services",
                "booking-1",
                "home-services.booking",
                "booking-1"));
        return contextAuthorizer;
    }

    private static ChatMessageRecord Record(string storageId, DateTimeOffset sentAt) => new(
        storageId,
        Guid.NewGuid(),
        "home-services",
        "booking-1",
        "user-1",
        "text",
        "message",
        sentAt);
}
