using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Travel;

namespace VSRSystemsBackend.Application.Travel.Interfaces;

public interface IPackageRepository : IRepository<TravelPackage>
{
    Task<IReadOnlyList<TravelPackage>> GetActivePackagesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TravelPackage>> GetByDestinationAsync(string destination, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TravelPackage>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<TravelPackage?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}

public interface IDestinationRepository : IRepository<Destination>
{
    Task<IReadOnlyList<Destination>> GetActiveDestinationsAsync(CancellationToken cancellationToken = default);
    Task<Destination?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}

public interface IBookingRepository : IRepository<Booking>
{
    Task<IReadOnlyList<Booking>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetByPackageIdAsync(string packageId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Booking?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default);
}

public interface IGroupTripRepository : IRepository<GroupTrip>
{
    Task<IReadOnlyList<GroupTrip>> GetActiveTripsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<GroupTrip>> GetByPackageIdAsync(string packageId, CancellationToken cancellationToken = default);
}

public interface IWishlistRepository : IRepository<TravelWishlist>
{
    Task<TravelWishlist?> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
}