using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class Payment : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PaymentNumber { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0;

    [MaxLength(20)]
    public string Method { get; set; } = "upi"; // upi/card/netbanking/wallet/cod

    [MaxLength(20)]
    public string Status { get; set; } = "initiated"; // initiated/authorized/captured/failed/refunded

    [MaxLength(200)]
    public string? GatewayRef { get; set; }

    public DateTime? PaidAt { get; set; }

    // Gateway fields (§171)
    [MaxLength(20)]
    public string? GatewayProvider { get; set; } // razorpay/stripe

    [MaxLength(200)]
    public string? GatewayOrderId { get; set; }

    [MaxLength(200)]
    public string? GatewayPaymentId { get; set; }

    [MaxLength(500)]
    public string? GatewaySignature { get; set; }

    public bool WebhookVerified { get; set; } = false;

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class Refund : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PaymentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0;

    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "requested"; // requested/approved/processed/rejected

    [MaxLength(50)]
    public string? ProcessedBy { get; set; }

    public DateTime? ProcessedAt { get; set; }

    [MaxLength(200)]
    public string? GatewayRefundId { get; set; }

    // Navigation
    [ForeignKey(nameof(PaymentId))]
    public virtual Payment Payment { get; set; } = null!;

    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class CreditTransaction : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0;

    [MaxLength(10)]
    public string Type { get; set; } = "credit"; // credit/debit

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ReferenceBookingId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceAfter { get; set; } = 0;

    // Navigation
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;
}

public class CommissionRule : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CategoryId { get; set; }

    [MaxLength(50)]
    public string? ServiceId { get; set; }

    [MaxLength(50)]
    public string? CityId { get; set; }

    [MaxLength(20)]
    public string? ProfessionalTier { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RatePercent { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal FlatFee { get; set; } = 0;

    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ProfessionalEarning : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaterialsExcludedAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal CommissionAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal AdjustmentAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxWithheldAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetAmount { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // pending/settled

    public DateTime? SettledAt { get; set; }

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;

    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class Payout : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // pending/processing/paid/failed

    public DateTime? PaidAt { get; set; }

    [MaxLength(1000)]
    public string? FailureReason { get; set; }

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}

public class ProfessionalAdjustment : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? BookingId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0;

    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}

public class ProfessionalIncentive : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string IncentiveType { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0;

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "accrued"; // accrued/paid

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}

public class PaymentGatewayWebhookEvent : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Provider { get; set; } = "razorpay";

    [Required]
    [MaxLength(100)]
    public string EventType { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string PayloadJson { get; set; } = "{}";

    public bool SignatureValid { get; set; } = false;
    public bool Processed { get; set; } = false;
    public DateTime? ProcessedAt { get; set; }

    [MaxLength(50)]
    public string? BookingId { get; set; }

    [MaxLength(1000)]
    public string? ProcessingError { get; set; }
}

public class PaymentGatewaySetting : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Provider { get; set; } = "razorpay";

    public bool IsActive { get; set; } = false;

    [MaxLength(10)]
    public string Mode { get; set; } = "test"; // test/live

    [MaxLength(500)]
    public string KeyId { get; set; } = string.Empty;

    [MaxLength(500)]
    public string KeySecretRef { get; set; } = string.Empty; // reference to secret store

    [MaxLength(500)]
    public string WebhookSecretRef { get; set; } = string.Empty;
}
