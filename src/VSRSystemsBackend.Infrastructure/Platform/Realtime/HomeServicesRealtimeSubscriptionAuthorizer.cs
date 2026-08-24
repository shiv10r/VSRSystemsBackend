using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Infrastructure.Platform.Realtime;

public sealed class HomeServicesRealtimeSubscriptionAuthorizer : IRealtimeSubscriptionAuthorizer
{
    private readonly IBookingRepository _bookings;
    private readonly ICustomerRepository _customers;
    private readonly IProfessionalRepository _professionals;

    public HomeServicesRealtimeSubscriptionAuthorizer(
        IBookingRepository bookings,
        ICustomerRepository customers,
        IProfessionalRepository professionals)
    {
        _bookings = bookings;
        _customers = customers;
        _professionals = professionals;
    }

    public async Task<bool> CanSubscribeToHomeServicesBookingAsync(
        string userId,
        bool hasAdministrativeAccess,
        string bookingId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(bookingId))
            return false;

        var booking = await _bookings.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null || booking.IsDeleted)
            return false;

        if (hasAdministrativeAccess)
            return true;

        var customer = await _customers.GetByIdAsync(booking.CustomerId, cancellationToken);
        if (customer?.UserId == userId)
            return true;

        if (string.IsNullOrWhiteSpace(booking.AssignedProfessionalId))
            return false;

        var professional = await _professionals.GetByIdAsync(booking.AssignedProfessionalId, cancellationToken);
        return professional?.UserId == userId;
    }
}
