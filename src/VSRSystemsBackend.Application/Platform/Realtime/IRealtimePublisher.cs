namespace VSRSystemsBackend.Application.Platform.Realtime;

public interface IRealtimePublisher
{
    Task SendToUserAsync<T>(
        string userId,
        RealtimeEventEnvelope<T> message,
        CancellationToken cancellationToken = default);

    Task SendToTenantAsync<T>(
        string tenantId,
        RealtimeEventEnvelope<T> message,
        CancellationToken cancellationToken = default);

    Task SendToGroupAsync<T>(
        string groupName,
        RealtimeEventEnvelope<T> message,
        CancellationToken cancellationToken = default);
}
