using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Travel.DTOs;

public class TravelPackageDto
{
    public string Id { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public string Route { get; set; } = string.Empty;
    public int Price { get; set; }
    public int? OriginalPrice { get; set; }
    public double Rating { get; set; }
    public int Travelers { get; set; }
    public int Departures { get; set; }
    public string Badge { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    public string TripType { get; set; } = string.Empty;
    public string DepartureCity { get; set; } = string.Empty;
    public List<string> Inclusions { get; set; } = new();
}

public class CreateTravelPackageDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Image { get; set; } = string.Empty;

    [Required]
    public int DurationDays { get; set; }

    [Required]
    public int DurationNights { get; set; }

    [Required]
    public string Route { get; set; } = string.Empty;

    [Required]
    public int Price { get; set; }

    public int? OriginalPrice { get; set; }

    [Required]
    public double Rating { get; set; }

    [Required]
    public int Travelers { get; set; }

    [Required]
    public int Departures { get; set; }

    [Required]
    public string Badge { get; set; } = string.Empty;

    [Required]
    public string Theme { get; set; } = string.Empty;

    [Required]
    public string TripType { get; set; } = string.Empty;

    [Required]
    public string DepartureCity { get; set; } = string.Empty;

    public List<string> Inclusions { get; set; } = new();
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

public class TravelDestinationDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int PackageCount { get; set; }
    public int StartingPrice { get; set; }
    public string Tagline { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateTravelDestinationDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Tagline { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class TravelPackageDtoExtended
{
    public string Id { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public string Route { get; set; } = string.Empty;
    public int Price { get; set; }
    public int? OriginalPrice { get; set; }
    public double Rating { get; set; }
    public int Travelers { get; set; }
    public int Departures { get; set; }
    public string Badge { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    public string TripType { get; set; } = string.Empty;
    public string DepartureCity { get; set; } = string.Empty;
    public List<string> Inclusions { get; set; } = new();
}

public class CreateTravelPackageDtoExtended
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Image { get; set; } = string.Empty;

    [Required]
    public int DurationDays { get; set; }

    [Required]
    public int DurationNights { get; set; }

    [Required]
    public string Route { get; set; } = string.Empty;

    [Required]
    public int Price { get; set; }

    public int? OriginalPrice { get; set; }

    [Required]
    public double Rating { get; set; }

    [Required]
    public int Travelers { get; set; }

    [Required]
    public int Departures { get; set; }

    [Required]
    public string Badge { get; set; } = string.Empty;

    [Required]
    public string Theme { get; set; } = string.Empty;

    [Required]
    public string TripType { get; set; } = string.Empty;

    [Required]
    public string DepartureCity { get; set; } = string.Empty;

    public List<string> Inclusions { get; set; } = new();
}

public class TravelDepartureDto
{
    public string Id { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string DateLabel { get; set; } = string.Empty;
    public string DepartureCity { get; set; } = string.Empty;
    public int SeatsLeft { get; set; }
    public int TotalSeats { get; set; }
    public int Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CreateTravelDepartureDto
{
    [Required]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string DateLabel { get; set; } = string.Empty;

    [Required]
    public string DepartureCity { get; set; } = string.Empty;

    [Required]
    public int TotalSeats { get; set; }

    [Required]
    public int Price { get; set; }

    public string Image { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

public class TravelBookingSessionDto
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string QuoteReference { get; set; } = string.Empty;
    public decimal QuotedAmount { get; set; }
    public decimal? FinalAmount { get; set; }
    public decimal? DepositAmount { get; set; }
    public string? HoldReference { get; set; }
    public DateTime? HoldExpiresAt { get; set; }
}

public class TravelBookingDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string DepartureId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceAmount { get; set; }
    public string? CouponCode { get; set; }
    public DateTime? HoldExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
}

public class CreateTravelBookingSessionDto
{
    [Required]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    public string DepartureId { get; set; } = string.Empty;

    public List<string> TravelerIds { get; set; } = new();

    public string? CouponCode { get; set; }

    public decimal? DepositAmount { get; set; }
}

public class CreateTravelBookingDto
{
    [Required]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    public string DepartureId { get; set; } = string.Empty;

    [Required]
    public List<TravelTravelerDto> Travelers { get; set; } = new();

    public string? CouponCode { get; set; }

    public decimal? DepositAmount { get; set; }
}

public class TravelTravelerDto
{
    [Required]
    [MaxLength(50)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? PassportNumber { get; set; }

    [MaxLength(50)]
    public string? PassportExpiry { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
}

public class CancelTravelBookingDto
{
    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
}

public class TravelPaymentOrderDto
{
    public string Id { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public string Gateway { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ProviderReference { get; set; }
    public string? WebhookUrl { get; set; }
}

public class TravelPaymentVerificationDto
{
    public string PaymentId { get; set; } = string.Empty;
    public string ProviderReference { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

public class TravelPaymentDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? GatewayReference { get; set; }
    public DateTime? PaidAt { get; set; }
}

public class TravelRefundDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime? ProcessedAt { get; set; }
    public string? GatewayRefundId { get; set; }
}

public class CreateTravelPaymentOrderDto
{
    [Required]
    public string Gateway { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Currency { get; set; } = string.Empty;
}

public class CreateTravelRefundDto
{
    [Required]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Reason { get; set; } = string.Empty;
}

public class LeadDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string PreferredTravelDate { get; set; } = string.Empty;
    public int Priority { get; set; } = 1;
    public bool IsConverted { get; set; } = false;
    public string? ConvertedBookingId { get; set; }
    public DateTime? ConvertedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateLeadDto
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Destination { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? TripPreferences { get; set; }

    [MaxLength(500)]
    public string? BudgetRange { get; set; }

    public string PreferredTravelDate { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Source { get; set; } = "website";

    public int Priority { get; set; } = 1;
}