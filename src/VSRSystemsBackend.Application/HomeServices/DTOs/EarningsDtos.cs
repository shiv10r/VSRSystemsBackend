using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.HomeServices.DTOs;

// ── Earnings & Payouts (§93-§96/§162) ────────────────────────────────────────

public class ProfessionalEarningDto
{
    public string Id { get; set; } = string.Empty;
    public string ProfessionalId { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string BookingNumber { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal GrossAmount { get; set; }
    public decimal MaterialsExcludedAmount { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal AdjustmentAmount { get; set; }
    public decimal TaxWithheldAmount { get; set; }
    public decimal NetAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? SettledAt { get; set; }
}

public class EarningsSummaryDto
{
    public string ProfessionalId { get; set; } = string.Empty;
    public decimal TotalGross { get; set; }
    public decimal TotalCommission { get; set; }
    public decimal TotalTaxWithheld { get; set; }
    public decimal TotalAdjustments { get; set; }
    public decimal TotalNet { get; set; }
    public decimal AvailableForPayout { get; set; }
    public decimal PendingSettlement { get; set; }
    public int CompletedBookings { get; set; }
    public List<ProfessionalEarningDto> RecentEarnings { get; set; } = new();
}

public class PayoutDto
{
    public string Id { get; set; } = string.Empty;
    public string ProfessionalId { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PaidAt { get; set; }
    public string? FailureReason { get; set; }
}

public class PayoutSummaryDto
{
    public string ProfessionalId { get; set; } = string.Empty;
    public decimal AvailableBalance { get; set; }
    public decimal PendingBalance { get; set; }
    public decimal NextPayoutAmount { get; set; }
    public DateTime? NextPayoutDate { get; set; }
    public List<PayoutDto> PayoutHistory { get; set; } = new();
}

public class MarkPayoutPaidDto
{
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "paid";

    [MaxLength(1000)]
    public string? FailureReason { get; set; }
}

public class CreateProfessionalAdjustmentDto
{
    [Range(-99999, 99999)]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? BookingId { get; set; }
}

public class CreateProfessionalIncentiveDto
{
    [Required]
    [MaxLength(50)]
    public string IncentiveType { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal Amount { get; set; }

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

// ── Analytics (§161) ─────────────────────────────────────────────────────────

public class AnalyticsSummaryDto
{
    public int TotalBookings { get; set; }
    public int ActiveBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalCustomers { get; set; }
    public int ActiveProfessionals { get; set; }
    public decimal AvgBookingValue { get; set; }
    public double AssignmentSuccessRate { get; set; }
    public double RepeatCustomerRate { get; set; }
}

public class TrendPointDto
{
    public string Date { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Revenue { get; set; }
}

public class BookingsTrendDto
{
    public string Period { get; set; } = "day"; // day/week/month
    public List<TrendPointDto> Points { get; set; } = new();
}

public class RevenueTrendDto
{
    public string Period { get; set; } = "day";
    public List<TrendPointDto> Points { get; set; } = new();
}

public class TopItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Revenue { get; set; }
}

public class TopCategoriesDto
{
    public List<TopItemDto> Items { get; set; } = new();
}

public class TopServicesDto
{
    public List<TopItemDto> Items { get; set; } = new();
}

public class TopCitiesDto
{
    public List<TopItemDto> Items { get; set; } = new();
}

public class AssignmentSuccessDto
{
    public double SuccessRate { get; set; }
    public int TotalAssignments { get; set; }
    public int Accepted { get; set; }
    public int Declined { get; set; }
    public int Expired { get; set; }
}

public class CancellationReasonDto
{
    public string Reason { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class CancellationReasonsDto
{
    public List<CancellationReasonDto> Items { get; set; } = new();
}

public class RepeatRateDto
{
    public double RepeatRate { get; set; }
    public int OneTimeCustomers { get; set; }
    public int RepeatCustomers { get; set; }
    public int TotalCustomers { get; set; }
}

public class ProviderPerformanceItemDto
{
    public string ProfessionalId { get; set; } = string.Empty;
    public string ProfessionalName { get; set; } = string.Empty;
    public int JobsCompleted { get; set; }
    public double AvgRating { get; set; }
    public double OnTimeRate { get; set; }
    public decimal TotalEarnings { get; set; }
}

public class ProviderPerformanceDto
{
    public List<ProviderPerformanceItemDto> Items { get; set; } = new();
}

public class RefundDisputeDto
{
    public double RefundRate { get; set; }
    public double DisputeRate { get; set; }
    public int TotalPaidBookings { get; set; }
    public int RefundedBookings { get; set; }
    public int DisputedBookings { get; set; }
    public decimal TotalRefunded { get; set; }
}