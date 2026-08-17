using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Travel;

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

    [Required]
    [MaxLength(200)]
    public string PackageName { get; set; } = string.Empty;

    public DateTime TravelDate { get; set; }

    public int NumberOfTravelers { get; set; } = 1;

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; } = 0;

    public decimal BalanceAmount { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "pending";

    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [MaxLength(1000)]
    public string? SpecialRequests { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}