using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.HomeServices.DTOs;

// ── Price Quotes (§88/§89) ───────────────────────────────────────────────────

public class PriceQuoteDto
{
    public string Id { get; set; } = string.Empty;
    public string QuoteNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public string? AddressId { get; set; }
    public decimal BasePrice { get; set; }
    public decimal AddOnsTotal { get; set; }
    public decimal MaterialsTotal { get; set; }
    public decimal FeesTotal { get; set; }
    public decimal TravelCharge { get; set; }
    public decimal UrgentCharge { get; set; }
    public decimal PlatformFee { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public string? CouponCode { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int Version { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<QuoteLineItemDto> LineItems { get; set; } = new();
}

public class QuoteLineItemDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // base/addon/material/fee/discount/tax
}

public class CreatePriceQuoteDto
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

    public List<string> AddOnIds { get; set; } = new();

    [MaxLength(20)]
    [RegularExpression("^(scheduled|emergency)$")]
    public string BookingType { get; set; } = "scheduled";

    [MaxLength(50)]
    public string? CouponCode { get; set; }

    [MaxLength(50)]
    public string? MembershipPlanId { get; set; }
}

public class QuoteRevisionDto
{
    public string Id { get; set; } = string.Empty;
    public string PriceQuoteId { get; set; } = string.Empty;
    public int RevisionNumber { get; set; }
    public string Reason { get; set; } = string.Empty;
    public decimal PreviousTotal { get; set; }
    public decimal NewTotal { get; set; }
    public string? CreatedBy { get; set; }
}

public class AcceptPriceQuoteDto
{
    public bool Accept { get; set; }
}

// ── Payments (§87/§171) ──────────────────────────────────────────────────────

public class PaymentDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string PaymentNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? GatewayRef { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? GatewayProvider { get; set; }
    public string? GatewayOrderId { get; set; }
    public bool WebhookVerified { get; set; }
}

public class CreatePaymentDto
{
    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [MaxLength(20)]
    [RegularExpression("^(upi|card|netbanking|wallet|cod)$")]
    public string Method { get; set; } = "upi";
}

public class PaymentInitiationResponseDto
{
    public string PaymentId { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    public string GatewayProvider { get; set; } = "razorpay";
    public string GatewayOrderId { get; set; } = string.Empty;
    public string GatewayKeyId { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
}

public class RazorpayPaymentCaptureDto
{
    [Required]
    [MaxLength(200)]
    public string RazorpayPaymentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string RazorpayOrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string RazorpaySignature { get; set; } = string.Empty;
}

public class RefundDto
{
    public string Id { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ProcessedAt { get; set; }
    public string? GatewayRefundId { get; set; }
}

public class CreateRefundDto
{
    [Required]
    [MaxLength(50)]
    public string PaymentId { get; set; } = string.Empty;

    [Range(1, 999999)]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;
}

public class ProcessRefundDto
{
    [Required]
    [MaxLength(20)]
    [RegularExpression("^(approved|rejected)$")]
    public string Decision { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Note { get; set; }
}

public class CreditTransactionDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string? ReferenceBookingId { get; set; }
    public decimal BalanceAfter { get; set; }
}

public class WalletDto
{
    public string CustomerId { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public List<CreditTransactionDto> Transactions { get; set; } = new();
}