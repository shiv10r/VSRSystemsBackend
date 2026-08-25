using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Application.Platform.Realtime;
using VSRSystemsBackend.Shared.Constants;

namespace VSRSystemsBackend.Api.Platform.Realtime;

[Authorize]
public sealed class RealtimeHub : Hub
{
    private readonly IRealtimeSubscriptionAuthorizer _subscriptionAuthorizer;
    private readonly IChatContextAuthorizer _chatContextAuthorizer;
    private readonly ILogger<RealtimeHub> _logger;

    public RealtimeHub(
        IRealtimeSubscriptionAuthorizer subscriptionAuthorizer,
        IChatContextAuthorizer chatContextAuthorizer,
        ILogger<RealtimeHub> logger)
    {
        _subscriptionAuthorizer = subscriptionAuthorizer;
        _chatContextAuthorizer = chatContextAuthorizer;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, RealtimeGroups.User(userId));

        var tenantId = Context.User?.FindFirst("tenant_id")?.Value;
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, RealtimeGroups.Tenant(tenantId));
            await Groups.AddToGroupAsync(Context.ConnectionId, RealtimeGroups.Presence(tenantId));
            await BroadcastPresenceUpdate(tenantId, userId, true);
        }

        _logger.LogInformation("Realtime connection {ConnectionId} established for user {UserId}", Context.ConnectionId, userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        var tenantId = Context.User?.FindFirst("tenant_id")?.Value;
        
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, RealtimeGroups.Presence(tenantId));
            await BroadcastPresenceUpdate(tenantId, userId ?? "unknown", false, exception != null ? "disconnected" : null);
        }

        _logger.LogInformation(exception, "Realtime connection {ConnectionId} disconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SubscribeToHomeServicesBooking(string bookingId)
    {
        var userId = GetUserId();
        if (userId is null || !await _subscriptionAuthorizer.CanSubscribeToHomeServicesBookingAsync(
                userId,
                HasAdministrativeAccess(),
                bookingId,
                Context.ConnectionAborted))
        {
            _logger.LogWarning(
                "Realtime booking subscription denied for connection {ConnectionId} and booking {BookingId}",
                Context.ConnectionId,
                bookingId);
            throw new HubException("You are not authorized to subscribe to this booking.");
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.HomeServicesBooking(bookingId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToSchoolMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.SchoolMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToWarehouseMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.WarehouseMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToHomeServicesMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.HomeServicesMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToTravelMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.TravelMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToRailwayMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.RailwayMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToHotelMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.HotelMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToNewsMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.NewsMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToJobsMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.JobsMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToCommerceMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.CommerceMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToBankMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.BankMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToMedicalMessage(string conversationId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.MedicalMessage(conversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToTypingIndicators(string conversationId)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId),
            Context.ConnectionAborted);
    }

    public async Task SubscribeToTypingIndicator(string conversationId)
    {
        await SubscribeToTypingIndicators(conversationId);
    }

    public async Task UnsubscribeFromTypingIndicators(string conversationId)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId),
            Context.ConnectionAborted);
    }

    public async Task BroadcastTypingIndicator(string conversationId, bool isTyping)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        var userId = GetUserId() ?? throw new HubException("Authentication is required.");
        var message = new RealtimeEventEnvelope<object>(
            Guid.NewGuid(),
            RealtimeEventTypes.PlatformChatTypingIndicator,
            1,
            DateTimeOffset.UtcNow,
            Context.ConnectionId,
            context.TenantId,
            new { conversationId = context.ConversationId, userId, isTyping });
        await Clients.OthersInGroup(RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId))
            .SendAsync(RealtimeGroups.ClientMethod, message, Context.ConnectionAborted);
    }

    public async Task SubscribeToMessageRead(string conversationId)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId),
            Context.ConnectionAborted);
    }

    public async Task UnsubscribeFromMessageRead(string conversationId)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId),
            Context.ConnectionAborted);
    }

    public async Task BroadcastMessageRead(string conversationId, string messageId)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        var userId = GetUserId() ?? throw new HubException("Authentication is required.");
        var message = new RealtimeEventEnvelope<object>(
            Guid.NewGuid(),
            RealtimeEventTypes.PlatformChatMessageRead,
            1,
            DateTimeOffset.UtcNow,
            Context.ConnectionId,
            context.TenantId,
            new { conversationId = context.ConversationId, messageId, userId });
        await Clients.OthersInGroup(RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId))
            .SendAsync(RealtimeGroups.ClientMethod, message, Context.ConnectionAborted);
    }

    public async Task SubscribeToPresence(string tenantId)
    {
        var userId = GetUserId();
        var authenticatedTenantId = Context.User?.FindFirst("tenant_id")?.Value;
        if (userId is null || string.IsNullOrWhiteSpace(authenticatedTenantId)
            || !string.Equals(tenantId, authenticatedTenantId, StringComparison.Ordinal))
        {
            throw new HubException("You are not authorized to subscribe to this presence group.");
        }

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.Presence(tenantId),
            Context.ConnectionAborted);
    }

    public Task UnsubscribeFromPresence(string tenantId) =>
        Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.Presence(tenantId),
            Context.ConnectionAborted);

    private async Task BroadcastPresenceUpdate(string tenantId, string userId, bool isOnline, string? statusMessage = null)
    {
        var message = new
        {
            type = "presence",
            tenantId,
            userId,
            isOnline,
            statusMessage,
            timestamp = DateTimeOffset.UtcNow
        };
        await Clients.Group(RealtimeGroups.Presence(tenantId))
            .SendAsync("realtimeEvent", message);
    }

    public Task UnsubscribeFromHomeServicesBooking(string bookingId) =>
        Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.HomeServicesBooking(bookingId),
            Context.ConnectionAborted);

    public async Task SubscribeToChatConversation(string conversationId)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId),
            Context.ConnectionAborted);
    }

    public async Task UnsubscribeFromChatConversation(string conversationId)
    {
        var context = await AuthorizeChatContextAsync(conversationId);
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            RealtimeGroups.ChatConversation(context.TenantId, context.ConversationId),
            Context.ConnectionAborted);
    }

    private async Task<AuthorizedChatContext> AuthorizeChatContextAsync(string conversationId)
    {
        var userId = GetUserId();
        var context = userId is null
            ? null
            : await _chatContextAuthorizer.AuthorizeAsync(
                conversationId,
                userId,
                HasAdministrativeAccess(),
                Context.ConnectionAborted);

        if (context is not null)
            return context;

        _logger.LogWarning(
            "Realtime chat subscription denied for connection {ConnectionId} and conversation {ConversationId}",
            Context.ConnectionId,
            conversationId);
        throw new HubException("You are not authorized to subscribe to this conversation.");
    }

    private bool HasAdministrativeAccess() =>
        Context.User?.IsInRole("admin") == true
        || Context.User?.IsInRole("ops_agent") == true
        || Context.User?.IsInRole("support_agent") == true;

    private string? GetUserId() =>
        Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? Context.User?.FindFirst(AppConstants.ClaimTypes.UserId)?.Value;
}
