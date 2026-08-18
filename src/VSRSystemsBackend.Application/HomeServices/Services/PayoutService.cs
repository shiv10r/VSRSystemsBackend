using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class PayoutService : IPayoutService
{
    private readonly IPayoutRepository _payoutRepository;
    private readonly IEarningsRepository _earningsRepository;

    public PayoutService(IPayoutRepository payoutRepository, IEarningsRepository earningsRepository)
    {
        _payoutRepository = payoutRepository;
        _earningsRepository = earningsRepository;
    }

    public async Task<Result<IReadOnlyList<PayoutDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var payouts = await _payoutRepository.GetByProfessionalAsync(professionalId, cancellationToken);
        var dtos = payouts.OrderByDescending(p => p.PeriodEnd).Select(MapToDto).ToList();
        return Result<IReadOnlyList<PayoutDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<PayoutDto>>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        var payouts = await _payoutRepository.GetByStatusAsync(status, cancellationToken);
        var dtos = payouts.OrderByDescending(p => p.PeriodEnd).Select(MapToDto).ToList();
        return Result<IReadOnlyList<PayoutDto>>.Success(dtos);
    }

    public async Task<Result<PayoutDto>> MarkProcessingAsync(string id, CancellationToken cancellationToken = default)
    {
        var payout = await _payoutRepository.GetByIdAsync(id, cancellationToken);
        if (payout == null)
            return Result<PayoutDto>.Failure("Payout not found");

        if (payout.Status != "pending")
            return Result<PayoutDto>.Failure("Only pending payouts can be marked as processing");

        payout.Status = "processing";
        payout.UpdatedAt = DateTime.UtcNow;
        await _payoutRepository.UpdateAsync(payout, cancellationToken);

        return Result<PayoutDto>.Success(MapToDto(payout));
    }

    public async Task<Result<PayoutDto>> MarkPaidAsync(string id, string? reference, CancellationToken cancellationToken = default)
    {
        var payout = await _payoutRepository.GetByIdAsync(id, cancellationToken);
        if (payout == null)
            return Result<PayoutDto>.Failure("Payout not found");

        if (payout.Status != "processing" && payout.Status != "pending")
            return Result<PayoutDto>.Failure("Payout cannot be marked as paid");

        payout.Status = "paid";
        payout.PaidAt = DateTime.UtcNow;
        payout.UpdatedAt = DateTime.UtcNow;
        await _payoutRepository.UpdateAsync(payout, cancellationToken);

        return Result<PayoutDto>.Success(MapToDto(payout));
    }

    public async Task<Result<PayoutDto>> MarkFailedAsync(string id, string reason, CancellationToken cancellationToken = default)
    {
        var payout = await _payoutRepository.GetByIdAsync(id, cancellationToken);
        if (payout == null)
            return Result<PayoutDto>.Failure("Payout not found");

        if (payout.Status != "processing")
            return Result<PayoutDto>.Failure("Only processing payouts can be marked as failed");

        payout.Status = "failed";
        payout.FailureReason = reason;
        payout.UpdatedAt = DateTime.UtcNow;
        await _payoutRepository.UpdateAsync(payout, cancellationToken);

        return Result<PayoutDto>.Success(MapToDto(payout));
    }

    public async Task<Result<PayoutSummaryDto>> GetPayoutStatusAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var earnings = await _earningsRepository.GetByProfessionalAsync(professionalId, cancellationToken);
        var payouts = await _payoutRepository.GetByProfessionalAsync(professionalId, cancellationToken);

        var settledEarnings = earnings.Where(e => e.Status == "settled").Sum(e => e.NetAmount);
        var pendingEarnings = earnings.Where(e => e.Status == "pending").Sum(e => e.NetAmount);

        var paidPayouts = payouts.Where(p => p.Status == "paid").Sum(p => p.TotalAmount);
        var processingPayouts = payouts.Where(p => p.Status == "processing").Sum(p => p.TotalAmount);
        var nonFailedPayouts = payouts.Where(p => p.Status != "failed").Sum(p => p.TotalAmount);

        var summary = new PayoutSummaryDto
        {
            ProfessionalId = professionalId,
            AvailableBalance = Math.Max(0m, settledEarnings - paidPayouts),
            PendingBalance = processingPayouts + pendingEarnings,
            NextPayoutAmount = Math.Max(0m, settledEarnings - nonFailedPayouts),
            NextPayoutDate = GetNextPayoutDate(DateTime.UtcNow),
            PayoutHistory = payouts.OrderByDescending(p => p.PeriodEnd).Select(MapToDto).ToList()
        };

        return Result<PayoutSummaryDto>.Success(summary);
    }

    private static PayoutDto MapToDto(Payout payout)
    {
        return new PayoutDto
        {
            Id = payout.Id,
            ProfessionalId = payout.ProfessionalId,
            PeriodStart = payout.PeriodStart,
            PeriodEnd = payout.PeriodEnd,
            TotalAmount = payout.TotalAmount,
            Status = payout.Status,
            PaidAt = payout.PaidAt,
            FailureReason = payout.FailureReason
        };
    }

    private static DateTime GetNextPayoutDate(DateTime from)
    {
        var monthStart = new DateTime(from.Year, from.Month, 1);
        var monthMid = new DateTime(from.Year, from.Month, 15);
        return from.Date < monthMid.Date ? monthMid : monthStart.AddMonths(1);
    }
}