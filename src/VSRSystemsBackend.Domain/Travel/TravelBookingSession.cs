using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Travel;

public class TravelBookingSession : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DepartureId { get; set; } = string.Empty;

    [Required]
    public decimal QuotedAmount { get; set; }

    public decimal? DepositAmount { get; set; }

    public string? CouponCode { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? FinalAmount { get; set; }

    public string Status { get; set; } = "pending";

    public string HoldReference { get; set; } = string.Empty;

    public DateTime? HoldExpiresAt { get; set; }

    public string CustomerId { get; set; } = string.Empty;

    public int TravelerCount { get; set; }

    public string Currency { get; set; } = "USD";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}