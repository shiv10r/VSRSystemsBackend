using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Travel;

public class TravelPayment : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingSessionId { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    public string Currency { get; set; } = "USD";

    [Required]
    public string PaymentMethod { get; set; } = string.Empty;

    public string? GatewayReference { get; set; }

    public string Status { get; set; } = "pending";

    public string? ProviderReference { get; set; }

    public DateTime? PaidAt { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}