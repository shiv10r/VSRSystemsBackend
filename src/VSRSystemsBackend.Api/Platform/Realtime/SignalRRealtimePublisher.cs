using Microsoft.AspNetCore.SignalR;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Api.Platform.Realtime;

public sealed class SignalRRealtimePublisher : IRealtimePublisher
{
    private readonly IHubContext<RealtimeHub> _hubContext;
    private readonly ILogger<SignalRRealtimePublisher> _logger;

    public SignalRRealtimePublisher(
        IHubContext<RealtimeHub> hubContext,
        ILogger<SignalRRealtimePublisher> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public Task SendToUserAsync<T>(
        string userId,
        RealtimeEventEnvelope<T> message,
        CancellationToken cancellationToken = default) =>
        SendAsync(_hubContext.Clients.Group(RealtimeGroups.User(userId)), message, "user", userId, cancellationToken);

    public Task SendToTenantAsync<T>(
        string tenantId,
        RealtimeEventEnvelope<T> message,
        CancellationToken cancellationToken = default) =>
        SendAsync(_hubContext.Clients.Group(RealtimeGroups.Tenant(tenantId)), message, "tenant", tenantId, cancellationToken);

    public Task SendToGroupAsync<T>(
        string groupName,
        RealtimeEventEnvelope<T> message,
        CancellationToken cancellationToken = default) =>
        SendAsync(_hubContext.Clients.Group(groupName), message, "group", groupName, cancellationToken);

    private async Task SendAsync<T>(
        IClientProxy client,
        RealtimeEventEnvelope<T> message,
        string targetType,
        string target,
        CancellationToken cancellationToken)
    {
        try
        {
            await client.SendAsync(RealtimeGroups.ClientMethod, message, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                exception,
                "Realtime event {EventId} of type {EventType} failed for {TargetType} {Target}",
                message.EventId,
                message.EventType,
                targetType,
                target);
            throw;
        }
    }
}
