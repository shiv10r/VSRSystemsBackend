namespace VSRSystemsBackend.Application.Platform.Realtime;

public interface IRealtimeSubscriptionAuthorizer
{
    Task<bool> CanSubscribeToHomeServicesBookingAsync(
        string userId,
        bool hasAdministrativeAccess,
        string bookingId,
        CancellationToken cancellationToken = default);
}
