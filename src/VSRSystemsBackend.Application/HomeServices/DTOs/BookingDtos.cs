using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.HomeServices.DTOs;

// ── Bookings ─────────────────────────────────────────────────────────────────

public class BookingDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string AddressId { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public string BookingType { get; set; } = string.Empty;
    public DateTime ScheduledStart { get; set; }
    public DateTime ExpectedEnd { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AssignedProfessionalId { get; set; }
    public string? AssignedProfessionalName { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public decimal GrandTotal { get; set; }
    public DateTime? ActualStartAt { get; set; }
    public DateTime? ActualEndAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancelReason { get; set; }
}

public class BookingDetailDto : BookingDto
{
    public string CustomerNotes { get; set; } = string.Empty;
    public string OpsNotes { get; set; } = string.Empty;
    public bool IsRework { get; set; }
    public string? OriginalBookingId { get; set; }
    public List<BookingItemDto> Items { get; set; } = new();
    public List<BookingAddOnDto> AddOns { get; set; } = new();
    public List<BookingMaterialDto> Materials { get; set; } = new();
    public List<BookingAssignmentDto> Assignments { get; set; } = new();
    public List<BookingStatusHistoryDto> StatusHistory { get; set; } = new();
    public List<BookingNoteDto> Notes { get; set; } = new();
}

public class BookingItemDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class BookingAddOnDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string AddOnId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class BookingMaterialDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public string? PhotoUrl { get; set; }
    public bool ApprovedByCustomer { get; set; }
}

public class BookingAssignmentDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string ProfessionalId { get; set; } = string.Empty;
    public string ProfessionalName { get; set; } = string.Empty;
    public DateTime OfferedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string Response { get; set; } = string.Empty;
    public string? DeclineReason { get; set; }
}

public class BookingStatusHistoryDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string PreviousStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string? ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class BookingNoteDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
}

public class CreateBookingDto
{
    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AddressId { get; set; } = string.Empty;

    [MaxLength(20)]
    [RegularExpression("^(instant|scheduled|emergency)$")]
    public string BookingType { get; set; } = "scheduled";

    public DateTime? ScheduledStart { get; set; }

    [MaxLength(50)]
    public string? PriceQuoteId { get; set; }

    public List<string> AddOnIds { get; set; } = new();

    [MaxLength(2000)]
    public string CustomerNotes { get; set; } = string.Empty;
}

public class RescheduleBookingDto
{
    [Required]
    public DateTime ScheduledStart { get; set; }

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
}

public class CancelBookingDto
{
    [Required]
    [MaxLength(200)]
    public string Reason { get; set; } = string.Empty;
}

public class ApproveAdditionalWorkDto
{
    [Required]
    public List<AdditionalWorkItemRequest> Items { get; set; } = new();
}

public class AdditionalWorkItemRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 999)]
    public int Quantity { get; set; } = 1;

    [Range(0, 999999)]
    public decimal UnitPrice { get; set; }

    [MaxLength(1000)]
    public string? PhotoUrl { get; set; }
}

public class BookingQueryDto
{
    [MaxLength(50)]
    public string? CustomerId { get; set; }

    [MaxLength(50)]
    public string? ProfessionalId { get; set; }

    [MaxLength(30)]
    public string? Status { get; set; }

    [MaxLength(30)]
    public string? PaymentStatus { get; set; }

    [MaxLength(50)]
    public string? ServiceId { get; set; }

    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class BookingListResultDto
{
    public List<BookingDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

// ── Recurring / AMC ──────────────────────────────────────────────────────────

public class RecurringBookingDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string AddressId { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public DateTime NextRunAt { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateRecurringBookingDto
{
    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AddressId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [RegularExpression("^(weekly|biweekly|monthly)$")]
    public string Frequency { get; set; } = "monthly";

    public DateTime NextRunAt { get; set; }
    public DateTime? EndDate { get; set; }
}

public class AmcContractDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string AddressId { get; set; } = string.Empty;
    public int VisitsPerYear { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CoveredServices { get; set; } = string.Empty;
}

public class CreateAmcContractDto
{
    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AddressId { get; set; } = string.Empty;

    [Range(1, 12)]
    public int VisitsPerYear { get; set; } = 2;

    public DateTime StartDate { get; set; }

    [Range(0, 999999)]
    public decimal Price { get; set; }

    [MaxLength(1000)]
    public string CoveredServices { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ExcludedParts { get; set; } = string.Empty;
}