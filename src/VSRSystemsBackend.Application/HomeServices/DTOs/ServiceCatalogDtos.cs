using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.HomeServices.DTOs;

// ── Catalog ──────────────────────────────────────────────────────────────────

public class ServiceCategoryDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public int ServiceCount { get; set; }
}

public class ServiceDto
{
    public string Id { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsEmergency { get; set; }
    public bool NeedsInspection { get; set; }
    public decimal InspectionFee { get; set; }
    public bool IsActive { get; set; }
    public decimal StartingPrice { get; set; }
    public List<ServicePackageDto> Packages { get; set; } = new();
    public List<ServiceAddOnDto> AddOns { get; set; } = new();
    public List<ServiceProblemDto> Problems { get; set; } = new();
}

public class ServicePackageDto
{
    public string Id { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int DurationMins { get; set; }
    public string WhatIncluded { get; set; } = string.Empty;
    public string Warranty { get; set; } = string.Empty;
    public bool IsPopular { get; set; }
    public bool IsActive { get; set; }
    public decimal? DiscountedPrice { get; set; }
}

public class ServiceAddOnDto
{
    public string Id { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMins { get; set; }
    public bool IsActive { get; set; }
}

public class ServiceProblemDto
{
    public string Id { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class CreateServiceCategoryDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Icon { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Color { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdateServiceCategoryDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Tagline { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateServiceDto
{
    [Required]
    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string LongDescription { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsEmergency { get; set; }
    public bool NeedsInspection { get; set; }
    public decimal InspectionFee { get; set; }
}

public class UpdateServiceDto
{
    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string LongDescription { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsEmergency { get; set; }
    public bool NeedsInspection { get; set; }
    public decimal InspectionFee { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateServicePackageDto
{
    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string DetailedDescription { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal BasePrice { get; set; }

    [Range(15, 1440)]
    public int DurationMins { get; set; } = 60;

    [MaxLength(2000)]
    public string WhatIncluded { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string WhatExcluded { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Warranty { get; set; } = string.Empty;

    public bool InspectionRequired { get; set; }
    public bool PartsIncluded { get; set; }
    public bool IsPopular { get; set; }
    public bool IsEmergencyEligible { get; set; }
}

public class UpdateServicePackageDto
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal BasePrice { get; set; }

    [Range(15, 1440)]
    public int DurationMins { get; set; } = 60;

    [MaxLength(2000)]
    public string WhatIncluded { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string WhatExcluded { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Warranty { get; set; } = string.Empty;

    public bool IsPopular { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateServiceAddOnDto
{
    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal Price { get; set; }

    [Range(0, 1440)]
    public int DurationMins { get; set; }
}

public class UpdateServiceAddOnDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal Price { get; set; }

    [Range(0, 1440)]
    public int DurationMins { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateServiceProblemDto
{
    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}

// ── Locations / Serviceability ───────────────────────────────────────────────

public class CityDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LaunchedAt { get; set; }
    public List<ZoneDto> Zones { get; set; } = new();
}

public class ZoneDto
{
    public string Id { get; set; } = string.Empty;
    public string CityId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<LocalityDto> Localities { get; set; } = new();
}

public class LocalityDto
{
    public string Id { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;
}

public class ServiceabilityRequestDto
{
    [Required]
    [MaxLength(20)]
    public string Pincode { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ServiceId { get; set; }
}

public class ServiceabilityResultDto
{
    public bool IsServiceable { get; set; }
    public string? CityId { get; set; }
    public string? CityName { get; set; }
    public string? ZoneId { get; set; }
    public string? ZoneName { get; set; }
    public string? LocalityId { get; set; }
    public string? LocalityName { get; set; }
    public string Pincode { get; set; } = string.Empty;
    public bool ServiceAvailable { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class SearchCatalogQueryDto
{
    [MaxLength(200)]
    public string Q { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CategoryId { get; set; }

    [MaxLength(20)]
    public string? Pincode { get; set; }

    public bool? EmergencyOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class SearchCatalogResultDto
{
    public List<ServiceDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

// ──────────────────────────────────────────────────────────────────────
// Flight
// ──────────────────────────────────────────────────────────────────────
public class FlightSearchRequestDto
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string DepartureDate { get; set; } = string.Empty;
    public string? ReturnDate { get; set; }
    public string TripType { get; set; } = "OneWay";
    public int Adults { get; set; } = 1;
    public int Children { get; set; } = 0;
    public int Infants { get; set; } = 0;
    public string? CabinClass { get; set; }
    public bool? DirectOnly { get; set; }
}

public class FlightSearchResultDto
{
    public string SearchId { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public List<FlightSegmentDto> Segments { get; set; } = new();
    public string Airline { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public DateTime Departure { get; set; }
    public DateTime Arrival { get; set; }
    public TimeSpan Duration { get; set; }
    public int Stops { get; set; }
    public string Cabin { get; set; } = string.Empty;
    public string Baggage { get; set; } = string.Empty;
    public string Refundability { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal BaseFare { get; set; }
    public decimal Taxes { get; set; }
    public decimal Fees { get; set; }
    public decimal Total { get; set; }
    public bool Refundable { get; set; }
    public bool Changeable { get; set; }
    public string FareRulesSummary { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class FlightSegmentDto
{
    public string DepartureAirport { get; set; } = string.Empty;
    public string ArrivalAirport { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Airline { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int Stops { get; set; }
}

public class FlightRevalidateRequestDto
{
    public string SearchId { get; set; } = string.Empty;
    public string ResultId { get; set; } = string.Empty;
}

public class CreateBookingSessionDto
{
    public string ProductType { get; set; } = "Flight";
    public string SearchReference { get; set; } = string.Empty;
    public List<BookingTravelerDto> Travelers { get; set; } = new();
    public string? SupplierReference { get; set; }
}

public class BookingTravelerDto
{
    public string FullName { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; } // passport etc
    public string DocumentType { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CreateFinalQuoteDto
{
    public string BookingSessionId { get; set; } = string.Empty;
    public decimal BaseAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal SupplierFee { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal ConvenienceFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal CouponDiscount { get; set; }
    public decimal WalletAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PricingVersion { get; set; } = string.Empty;
}

public class RefundPreviewDto
{
    public string BookingId { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public decimal SupplierDeduction { get; set; }
    public decimal PlatformDeduction { get; set; }
    public decimal RefundAmount { get; set; }
}