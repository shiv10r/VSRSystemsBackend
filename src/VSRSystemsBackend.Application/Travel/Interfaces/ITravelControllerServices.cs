using System.Collections.Generic;
using VSRSystemsBackend.Application.Travel.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Travel.Interfaces;

public interface ITravelDestinationService
{
    Task<Result<IReadOnlyList<TravelDestinationDto>>> GetDestinationsAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TravelDestinationDto>>> GetActiveDestinationsAsync(CancellationToken cancellationToken = default);
    Task<Result<TravelDestinationDto>> GetDestinationByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<TravelDestinationDto>> CreateDestinationAsync(CreateTravelDestinationDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelDestinationDto>> GetDestinationBySlugAsync(string slug, CancellationToken cancellationToken = default);
}

public interface ITravelPackageService
{
    Task<Result<IReadOnlyList<TravelPackageDto>>> GetPackagesAsync(string? destinationId, string? theme, string? sort, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TravelPackageDto>>> GetActivePackagesAsync(string? destinationId, CancellationToken cancellationToken = default);
    Task<Result<TravelPackageDto>> GetPackageBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Result<TravelPackageDto>> CreatePackageAsync(CreateTravelPackageDto dto, CancellationToken cancellationToken = default);
}

public interface ITravelDepartureService
{
    Task<Result<IReadOnlyList<TravelDepartureDto>>> GetDeparturesAsync(string? packageId, string? departureCity, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TravelDepartureDto>>> GetActiveDeparturesAsync(string? packageId, CancellationToken cancellationToken = default);
    Task<Result<TravelDepartureDto>> GetDepartureByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<TravelDepartureDto>> CreateDepartureAsync(CreateTravelDepartureDto dto, CancellationToken cancellationToken = default);
}

public interface ITravelBookingService
{
    Task<Result<TravelBookingSessionDto>> CreateBookingSessionAsync(CreateTravelBookingSessionDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelBookingSessionDto>> GetBookingSessionAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<TravelBookingDto>> CreateBookingAsync(CreateTravelBookingDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelBookingDto>> GetBookingAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TravelBookingDto>>> GetMyBookingsAsync(CancellationToken cancellationToken = default);
    Task<Result> CancelBookingAsync(string id, string reason, CancellationToken cancellationToken = default);
}

public interface ITravelPaymentService
{
    Task<Result<TravelPaymentOrderDto>> CreatePaymentOrderAsync(string sessionId, CreateTravelPaymentOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelPaymentVerificationDto>> VerifyPaymentAsync(TravelPaymentVerificationDto dto, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TravelPaymentDto>>> GetPaymentsByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Result<TravelRefundDto>> CreateRefundAsync(CreateTravelRefundDto dto, CancellationToken cancellationToken = default);
    Task<Result<TravelRefundDto>> GetRefundAsync(string refundId, CancellationToken cancellationToken = default);
}