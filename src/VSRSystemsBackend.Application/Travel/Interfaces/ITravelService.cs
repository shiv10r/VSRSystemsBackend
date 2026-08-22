using VSRSystemsBackend.Application.Travel.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Travel.Interfaces;

public interface IPackageService
{
    Task<Result<TravelPackageDto>> CreateAsync(CreateTravelPackageDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelPackageDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<TravelPackageDto>> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TravelPackageDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TravelPackageDto>>> GetActivePackagesAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TravelPackageDto>>> GetByDestinationAsync(string destination, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TravelPackageDto>>> GetByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<TravelPackageDto>> UpdateAsync(string id, UpdateTravelPackageDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IDestinationService
{
    Task<Result<DestinationDto>> CreateAsync(CreateDestinationDto dto, CancellationToken cancellationToken = default);
    Task<Result<DestinationDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<DestinationDto>> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DestinationDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DestinationDto>>> GetActiveDestinationsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<DestinationDto>> UpdateAsync(string id, UpdateDestinationDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IBookingService
{
    Task<Result<BookingDto>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BookingDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BookingDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BookingDto>>> GetByPackageIdAsync(string packageId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BookingDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> UpdateAsync(string id, UpdateBookingDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> ConfirmAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> CancelAsync(string id, string reason, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> CompleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IGroupTripService
{
    Task<Result<GroupTripDto>> CreateAsync(CreateGroupTripDto dto, CancellationToken cancellationToken = default);
    Task<Result<GroupTripDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<GroupTripDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<GroupTripDto>>> GetActiveTripsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<GroupTripDto>>> GetByPackageIdAsync(string packageId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<GroupTripDto>> UpdateAsync(string id, UpdateGroupTripDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IDepartureService
{
    Task<Result<TravelDepartureDto>> CreateAsync(CreateTravelDepartureDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelDepartureDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TravelDepartureDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TravelDepartureDto>>> GetActiveAsync(string? packageId = null, CancellationToken cancellationToken = default);
}

public interface IPaymentService
{
    Task<Result<TravelPaymentOrderDto>> CreatePaymentOrderAsync(string sessionId, CreateTravelPaymentOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result> VerifyPaymentAsync(TravelPaymentVerificationDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelPaymentDto>> GetByBookingIdAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Result<TravelRefundDto>> CreateRefundAsync(CreateTravelRefundDto dto, CancellationToken cancellationToken = default);
}

public interface ILeadService
{
    Task<Result<LeadDto>> CreateAsync(CreateLeadDto dto, CancellationToken cancellationToken = default);
    Task<Result<LeadDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LeadDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LeadDto>>> GetActiveAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<LeadDto>> UpdateStatusAsync(string id, string status, CancellationToken cancellationToken = default);
}

public interface IBookingSessionService
{
    Task<Result<TravelBookingSessionDto>> CreateAsync(CreateTravelBookingSessionDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelBookingSessionDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TravelBookingSessionDto>>> GetByPackageIdAsync(string packageId, PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IWishlistService
{
    Task<Result<TravelWishlistDto>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<TravelWishlistDto>> AddItemAsync(string wishlistId, string packageId, CancellationToken cancellationToken = default);
    Task<Result<TravelWishlistDto>> RemoveItemAsync(string wishlistId, string packageId, CancellationToken cancellationToken = default);
    Task<Result<TravelWishlistDto>> ClearAsync(string wishlistId, CancellationToken cancellationToken = default);
}