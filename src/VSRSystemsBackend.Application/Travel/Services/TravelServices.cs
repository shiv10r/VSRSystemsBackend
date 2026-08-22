using System.Collections.Generic;
using VSRSystemsBackend.Application.Travel.DTOs;
using VSRSystemsBackend.Application.Travel.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Travel;

namespace VSRSystemsBackend.Application.Travel.Services;

public class TravelDestinationService : ITravelDestinationService
{
    private readonly IRepository<Destination> _destinationRepository;
    private readonly IRepository<TravelPackage> _packageRepository;

    public TravelDestinationService(IRepository<Destination> destinationRepository, IRepository<TravelPackage> packageRepository)
    {
        _destinationRepository = destinationRepository;
        _packageRepository = packageRepository;
    }

    public async Task<Result<IReadOnlyList<TravelDestinationDto>>> GetDestinationsAsync(CancellationToken cancellationToken = default)
    {
        var destinations = await _destinationRepository.FindAsync(d => d.Status == "active", cancellationToken);
        var packages = await _packageRepository.FindAsync(p => p.Status == "active", cancellationToken);
        var dtos = destinations
            .OrderBy(d => d.Name)
            .Select(d => ToDto(d, packages))
            .ToList();
        return Result<IReadOnlyList<TravelDestinationDto>>.Success(dtos);
    }

    public Task<Result<IReadOnlyList<TravelDestinationDto>>> GetActiveDestinationsAsync(CancellationToken cancellationToken = default)
        => GetDestinationsAsync(cancellationToken);

    public async Task<Result<TravelDestinationDto>> GetDestinationByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var destination = await _destinationRepository.GetByIdAsync(id, cancellationToken);
        if (destination is null || destination.IsDeleted)
            return Result<TravelDestinationDto>.Failure("Destination not found");

        var packages = await _packageRepository.FindAsync(p => p.Status == "active", cancellationToken);
        return Result<TravelDestinationDto>.Success(ToDto(destination, packages));
    }

    public async Task<Result<TravelDestinationDto>> GetDestinationBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var destinations = await _destinationRepository.FindAsync(d => d.Code == slug && d.Status == "active", cancellationToken);
        var destination = destinations.FirstOrDefault();
        if (destination is null)
            return Result<TravelDestinationDto>.Failure("Destination not found");

        var packages = await _packageRepository.FindAsync(p => p.Status == "active", cancellationToken);
        return Result<TravelDestinationDto>.Success(ToDto(destination, packages));
    }

    public Task<Result<TravelDestinationDto>> CreateDestinationAsync(CreateTravelDestinationDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelDestinationDto>.Failure("Not implemented"));

    private static TravelDestinationDto ToDto(Destination d, IReadOnlyList<TravelPackage> packages)
    {
        var forDestination = packages.Where(p => p.DestinationId == d.Id).ToList();
        var startingPrice = forDestination.Count > 0 ? (int)forDestination.Min(p => p.Price) : 0;
        return new TravelDestinationDto
        {
            Id = d.Id,
            Name = d.Name,
            Country = d.Country,
            ImageUrl = d.ImageUrls.FirstOrDefault() ?? string.Empty,
            PackageCount = forDestination.Count,
            Tagline = d.Description ?? string.Empty,
            IsActive = d.Status == "active",
            StartingPrice = startingPrice
        };
    }
}

public class TravelPackageService : ITravelPackageService
{
    private readonly IRepository<TravelPackage> _packageRepository;
    private readonly IRepository<Destination> _destinationRepository;
    private readonly IRepository<TravelDeparture> _departureRepository;

    public TravelPackageService(IRepository<TravelPackage> packageRepository, IRepository<Destination> destinationRepository, IRepository<TravelDeparture> departureRepository)
    {
        _packageRepository = packageRepository;
        _destinationRepository = destinationRepository;
        _departureRepository = departureRepository;
    }

    public async Task<Result<IReadOnlyList<TravelPackageDto>>> GetPackagesAsync(string? destinationId, string? theme, string? sort, CancellationToken cancellationToken = default)
    {
        var packages = await _packageRepository.FindAsync(p => p.Status == "active", cancellationToken);

        if (!string.IsNullOrWhiteSpace(destinationId))
        {
            var matched = await _destinationRepository.FindAsync(d => d.Id == destinationId || d.Name == destinationId, cancellationToken);
            var targetId = matched.FirstOrDefault()?.Id ?? destinationId;
            packages = packages.Where(p => p.DestinationId == targetId).ToList();
        }

        if (!string.IsNullOrWhiteSpace(theme))
            packages = packages.Where(p => string.Equals(p.Category, theme, StringComparison.OrdinalIgnoreCase)).ToList();

        packages = sort?.ToLowerInvariant() switch
        {
            "price-low" => packages.OrderBy(p => p.Price).ToList(),
            "price-high" => packages.OrderByDescending(p => p.Price).ToList(),
            "rating" => packages.OrderByDescending(p => p.DiscountedPrice ?? p.Price).ToList(),
            _ => packages.OrderByDescending(p => p.MaxGroupSize).ToList()
        };

        var destinations = await _destinationRepository.FindAsync(d => d.Status == "active", cancellationToken);
        var departures = await _departureRepository.FindAsync(d => d.Status == "active", cancellationToken);

        var dtos = packages.Select(p => ToDto(p, destinations, departures)).ToList();
        return Result<IReadOnlyList<TravelPackageDto>>.Success(dtos);
    }

    public Task<Result<IReadOnlyList<TravelPackageDto>>> GetActivePackagesAsync(string? destinationId, CancellationToken cancellationToken = default)
        => GetPackagesAsync(destinationId, null, null, cancellationToken);

    public async Task<Result<TravelPackageDto>> GetPackageBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var packages = await _packageRepository.FindAsync(p => p.Code == slug && p.Status == "active", cancellationToken);
        var package = packages.FirstOrDefault();
        if (package is null)
            return Result<TravelPackageDto>.Failure("Package not found");

        var destinations = await _destinationRepository.FindAsync(d => d.Status == "active", cancellationToken);
        var departures = await _departureRepository.FindAsync(d => d.Status == "active", cancellationToken);
        return Result<TravelPackageDto>.Success(ToDto(package, destinations, departures));
    }

    public Task<Result<TravelPackageDto>> CreatePackageAsync(CreateTravelPackageDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelPackageDto>.Failure("Not implemented"));

    private static TravelPackageDto ToDto(TravelPackage p, IReadOnlyList<Destination> destinations, IReadOnlyList<TravelDeparture> departures)
    {
        var destinationName = destinations.FirstOrDefault(d => d.Id == p.DestinationId)?.Name ?? p.DestinationId;
        var packageDepartures = departures.Where(d => d.PackageId == p.Id).ToList();
        var firstDeparture = packageDepartures.FirstOrDefault();
        return new TravelPackageDto
        {
            Id = p.Id,
            Slug = p.Code,
            Title = p.Name,
            Destination = destinationName,
            Image = p.ImageUrls.FirstOrDefault() ?? string.Empty,
            DurationDays = p.DurationDays,
            DurationNights = p.DurationDays > 0 ? p.DurationDays - 1 : 0,
            Route = p.Description ?? string.Empty,
            Price = (int)p.Price,
            OriginalPrice = p.DiscountedPrice.HasValue ? (int?)Math.Round(p.DiscountedPrice.Value) : null,
            Rating = 0,
            Travelers = p.MaxGroupSize,
            Departures = packageDepartures.Count,
            Badge = string.Empty,
            Theme = p.Category,
            TripType = p.Category,
            DepartureCity = firstDeparture?.DepartureCity ?? string.Empty,
            Inclusions = (p.Inclusions ?? string.Empty)
                .Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList()
        };
    }
}

public class TravelDepartureService : ITravelDepartureService
{
    private readonly IRepository<TravelDeparture> _departureRepository;

    public TravelDepartureService(IRepository<TravelDeparture> departureRepository)
    {
        _departureRepository = departureRepository;
    }

    public async Task<Result<IReadOnlyList<TravelDepartureDto>>> GetDeparturesAsync(string? packageId, string? departureCity, CancellationToken cancellationToken = default)
    {
        var departures = await _departureRepository.FindAsync(d => d.Status == "active", cancellationToken);
        if (!string.IsNullOrWhiteSpace(packageId))
            departures = departures.Where(d => d.PackageId == packageId).ToList();
        if (!string.IsNullOrWhiteSpace(departureCity))
            departures = departures.Where(d => d.DepartureCity.Contains(departureCity, StringComparison.OrdinalIgnoreCase)).ToList();

        var dtos = departures
            .OrderBy(d => d.DepartureDate)
            .Select(ToDto)
            .ToList();
        return Result<IReadOnlyList<TravelDepartureDto>>.Success(dtos);
    }

    public Task<Result<IReadOnlyList<TravelDepartureDto>>> GetActiveDeparturesAsync(string? packageId, CancellationToken cancellationToken = default)
        => GetDeparturesAsync(packageId, null, cancellationToken);

    public async Task<Result<TravelDepartureDto>> GetDepartureByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var departure = await _departureRepository.GetByIdAsync(id, cancellationToken);
        if (departure is null || departure.IsDeleted)
            return Result<TravelDepartureDto>.Failure("Departure not found");
        return Result<TravelDepartureDto>.Success(ToDto(departure));
    }

    public Task<Result<TravelDepartureDto>> CreateDepartureAsync(CreateTravelDepartureDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelDepartureDto>.Failure("Not implemented"));

    private static TravelDepartureDto ToDto(TravelDeparture d) => new()
    {
        Id = d.Id,
        PackageId = d.PackageId,
        Title = d.Title,
        DateLabel = d.DepartureDate.ToString("d MMM yyyy"),
        DepartureCity = d.DepartureCity,
        SeatsLeft = d.AvailableSeats,
        TotalSeats = d.TotalSeats,
        Price = (int)d.Price,
        Image = d.ImageUrl ?? string.Empty,
        Status = d.Status
    };
}

public class TravelBookingService : ITravelBookingService
{
    public Task<Result<TravelBookingSessionDto>> CreateBookingSessionAsync(CreateTravelBookingSessionDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelBookingSessionDto>.Failure("Not implemented"));

    public Task<Result<TravelBookingSessionDto>> GetBookingSessionAsync(string id, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelBookingSessionDto>.Failure("Not implemented"));

    public Task<Result<TravelBookingDto>> CreateBookingAsync(CreateTravelBookingDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelBookingDto>.Failure("Not implemented"));

    public Task<Result<TravelBookingDto>> GetBookingAsync(string id, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelBookingDto>.Failure("Not implemented"));

    public Task<Result<IReadOnlyList<TravelBookingDto>>> GetMyBookingsAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(Result<IReadOnlyList<TravelBookingDto>>.Success(new List<TravelBookingDto>()));

    public Task<Result> CancelBookingAsync(string id, string reason, CancellationToken cancellationToken = default)
        => Task.FromResult(Result.Failure("Not implemented"));
}

public class TravelPaymentService : ITravelPaymentService
{
    public Task<Result<TravelPaymentOrderDto>> CreatePaymentOrderAsync(string sessionId, CreateTravelPaymentOrderDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelPaymentOrderDto>.Failure("Not implemented"));

    public Task<Result<TravelPaymentVerificationDto>> VerifyPaymentAsync(TravelPaymentVerificationDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelPaymentVerificationDto>.Failure("Not implemented"));

    public Task<Result<IReadOnlyList<TravelPaymentDto>>> GetPaymentsByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<IReadOnlyList<TravelPaymentDto>>.Success(new List<TravelPaymentDto>()));

    public Task<Result<TravelRefundDto>> CreateRefundAsync(CreateTravelRefundDto dto, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelRefundDto>.Failure("Not implemented"));

    public Task<Result<TravelRefundDto>> GetRefundAsync(string refundId, CancellationToken cancellationToken = default)
        => Task.FromResult(Result<TravelRefundDto>.Failure("Not implemented"));
}