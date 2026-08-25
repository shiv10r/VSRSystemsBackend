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
            await Groups.AddToGroupAsync(Context.ConnectionId, RealtimeGroups.Tenant(tenantId));

        _logger.LogInformation("Realtime connection {ConnectionId} established for user {UserId}", Context.ConnectionId, userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
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
