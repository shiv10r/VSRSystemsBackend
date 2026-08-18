using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class Booking : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AddressId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [MaxLength(20)]
    public string BookingType { get; set; } = "scheduled"; // instant/scheduled/emergency/recurring/amc

    public DateTime ScheduledStart { get; set; }
    public DateTime ExpectedEnd { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "draft"; // draft/awaiting_payment/confirmed/searching_provider/assigned/
    // provider_accepted/on_the_way/arrived/awaiting_start_verification/in_service/
    // awaiting_customer_approval/awaiting_additional_payment/service_completed/completed/
    // cancelled/refund_pending/refunded/disputed/closed

    [MaxLength(50)]
    public string? AssignedProfessionalId { get; set; }

    [MaxLength(50)]
    public string? PriceQuoteId { get; set; }

    [MaxLength(50)]
    public string? CurrentQuoteId { get; set; }

    [MaxLength(30)]
    public string PaymentStatus { get; set; } = "pending"; // pending/paid/partial_refund/refunded/failed

    [MaxLength(2000)]
    public string CustomerNotes { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string OpsNotes { get; set; } = string.Empty;

    public DateTime? ActualStartAt { get; set; }
    public DateTime? ActualEndAt { get; set; }
    public DateTime? CancelledAt { get; set; }

    [MaxLength(200)]
    public string? CancelReason { get; set; }

    public bool IsRework { get; set; } = false;
    public string? OriginalBookingId { get; set; }

    // Navigation
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;

    [ForeignKey(nameof(PackageId))]
    public virtual ServicePackage Package { get; set; } = null!;

    public virtual ICollection<BookingItem> Items { get; set; } = new List<BookingItem>();
    public virtual ICollection<BookingAddOn> AddOns { get; set; } = new List<BookingAddOn>();
    public virtual ICollection<BookingMaterial> Materials { get; set; } = new List<BookingMaterial>();
    public virtual ICollection<BookingAssignment> Assignments { get; set; } = new List<BookingAssignment>();
    public virtual ICollection<BookingStatusHistory> StatusHistory { get; set; } = new List<BookingStatusHistory>();
    public virtual ICollection<BookingNote> Notes { get; set; } = new List<BookingNote>();
}

public class BookingItem : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; } = 0;

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class BookingAddOn : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AddOnId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey(nameof(AddOnId))]
    public virtual ServiceAddOn AddOn { get; set; } = null!;
}

public class BookingMaterial : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; } = 0;

    [MaxLength(1000)]
    public string? PhotoUrl { get; set; }

    public bool ApprovedByCustomer { get; set; } = false;
    public DateTime? ApprovedAt { get; set; }

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class BookingAssignment : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    public DateTime OfferedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAt { get; set; }

    [MaxLength(20)]
    public string Response { get; set; } = "pending"; // accepted/declined/expired/reassigned

    [MaxLength(500)]
    public string? DeclineReason { get; set; }

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}

public class BookingStatusHistory : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string PreviousStatus { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string NewStatus { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string MetadataJson { get; set; } = "{}";

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class BookingNote : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string AuthorId { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Note { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Visibility { get; set; } = "internal"; // internal/customer

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class RecurringBooking : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

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
    public string Frequency { get; set; } = "monthly"; // weekly/biweekly/monthly

    public DateTime NextRunAt { get; set; }

    [MaxLength(50)]
    public string? PreferredProfessionalId { get; set; }

    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}

public class AmcContract : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AddressId { get; set; } = string.Empty;

    public int VisitsPerYear { get; set; } = 2;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "active"; // active/expired/cancelled

    [MaxLength(1000)]
    public string CoveredServices { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ExcludedParts { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}
