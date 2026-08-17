using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Travel.DTOs;

public class TravelPackageDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DestinationId { get; set; } = string.Empty;
    public string DestinationName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? Itinerary { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public int MaxGroupSize { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTravelPackageDto
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DestinationId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }

    [MaxLength(3000)]
    public string? Inclusions { get; set; }

    [MaxLength(3000)]
    public string? Exclusions { get; set; }

    [MaxLength(10000)]
    public string? Itinerary { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public int MaxGroupSize { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";
}

public class UpdateTravelPackageDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string DestinationId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }

    [MaxLength(3000)]
    public string? Inclusions { get; set; }

    [MaxLength(3000)]
    public string? Exclusions { get; set; }

    [MaxLength(10000)]
    public string? Itinerary { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public int MaxGroupSize { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class DestinationDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? State { get; set; }
    public string? Description { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? BestTimeToVisit { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDestinationDto
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? State { get; set; }

    [MaxLength(3000)]
    public string? Description { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(200)]
    public string? BestTimeToVisit { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";
}

public class UpdateDestinationDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? State { get; set; }

    [MaxLength(3000)]
    public string? Description { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(200)]
    public string? BestTimeToVisit { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class BookingDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public DateTime TravelDate { get; set; }
    public int NumberOfTravelers { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBookingDto
{
    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CustomerPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    public DateTime TravelDate { get; set; }

    public int NumberOfTravelers { get; set; } = 1;

    [MaxLength(1000)]
    public string? SpecialRequests { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "pending";

    [MaxLength(50)]
    public string? PaymentMethod { get; set; }
}

public class UpdateBookingDto
{
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    public DateTime? TravelDate { get; set; }

    public int NumberOfTravelers { get; set; }

    public string Status { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [MaxLength(1000)]
    public string? SpecialRequests { get; set; }
}

public class GroupTripDto
{
    public string Id { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentBookings { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TripLeader { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateGroupTripDto
{
    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int MaxCapacity { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "upcoming";

    [MaxLength(200)]
    public string? TripLeader { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}

public class UpdateGroupTripDto
{
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int MaxCapacity { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? TripLeader { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}

public class TravelWishlistDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public List<TravelWishlistItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class TravelWishlistItemDto
{
    public string Id { get; set; } = string.Empty;
    public string WishlistId { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; }
}