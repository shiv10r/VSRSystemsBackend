using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsRepository _analyticsRepository;

    public AnalyticsService(IAnalyticsRepository analyticsRepository)
    {
        _analyticsRepository = analyticsRepository;
    }

    public async Task<Result<AnalyticsSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var totalBookings = await _analyticsRepository.CountBookingsAsync(cancellationToken);
        var completedBookings = await _analyticsRepository.CountBookingsAsync("completed", cancellationToken);
        var cancelledBookings = await _analyticsRepository.CountBookingsAsync("cancelled", cancellationToken);
        var totalRevenue = await _analyticsRepository.SumRevenueAsync(cancellationToken);
        var totalCustomers = await _analyticsRepository.CountCustomersAsync(cancellationToken);
        var activeProfessionals = await _analyticsRepository.CountProfessionalsAsync("verified", cancellationToken);
        var assignmentStats = await _analyticsRepository.GetAssignmentStatsAsync(cancellationToken);
        var repeatStats = await _analyticsRepository.GetCustomerRepeatStatsAsync(cancellationToken);

        var summary = new AnalyticsSummaryDto
        {
            TotalBookings = totalBookings,
            CompletedBookings = completedBookings,
            CancelledBookings = cancelledBookings,
            ActiveBookings = Math.Max(0, totalBookings - completedBookings - cancelledBookings),
            TotalRevenue = totalRevenue,
            TotalCustomers = totalCustomers,
            ActiveProfessionals = activeProfessionals,
            AvgBookingValue = totalBookings > 0 ? totalRevenue / totalBookings : 0,
            AssignmentSuccessRate = assignmentStats.Total > 0 ? (double)assignmentStats.Accepted / assignmentStats.Total * 100 : 0,
            RepeatCustomerRate = repeatStats.Total > 0 ? (double)repeatStats.Repeat / repeatStats.Total * 100 : 0
        };

        return Result<AnalyticsSummaryDto>.Success(summary);
    }

    public async Task<Result<IReadOnlyList<TrendPointDto>>> GetBookingsTrendAsync(int days, CancellationToken cancellationToken = default)
    {
        var from = DateTime.UtcNow.AddDays(-days);
        var stats = await _analyticsRepository.GetDailyBookingStatsAsync(from, DateTime.UtcNow, cancellationToken);
        var points = stats
            .Select(s => new TrendPointDto
            {
                Date = s.Date.ToString("yyyy-MM-dd"),
                Count = s.Count,
                Revenue = s.Revenue
            })
            .ToList();

        return Result<IReadOnlyList<TrendPointDto>>.Success(points);
    }

    public async Task<Result<IReadOnlyList<TrendPointDto>>> GetRevenueTrendAsync(int days, CancellationToken cancellationToken = default)
    {
        var from = DateTime.UtcNow.AddDays(-days);
        var stats = await _analyticsRepository.GetDailyBookingStatsAsync(from, DateTime.UtcNow, cancellationToken);
        var points = stats
            .Select(s => new TrendPointDto
            {
                Date = s.Date.ToString("yyyy-MM-dd"),
                Count = s.Count,
                Revenue = s.Revenue
            })
            .ToList();

        return Result<IReadOnlyList<TrendPointDto>>.Success(points);
    }

    public async Task<Result<IReadOnlyList<TopItemDto>>> GetTopCategoriesAsync(int limit, CancellationToken cancellationToken = default)
    {
        var items = await _analyticsRepository.GetTopByCategoryAsync(limit, cancellationToken);
        var dtos = items
            .Select(i => new TopItemDto
            {
                Name = i.Name,
                Count = i.Count,
                Revenue = i.Revenue
            })
            .ToList();

        return Result<IReadOnlyList<TopItemDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<TopItemDto>>> GetTopServicesAsync(int limit, CancellationToken cancellationToken = default)
    {
        var items = await _analyticsRepository.GetTopByServiceAsync(limit, cancellationToken);
        var dtos = items
            .Select(i => new TopItemDto
            {
                Name = i.Name,
                Count = i.Count,
                Revenue = i.Revenue
            })
            .ToList();

        return Result<IReadOnlyList<TopItemDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<TopItemDto>>> GetTopCitiesAsync(int limit, CancellationToken cancellationToken = default)
    {
        var items = await _analyticsRepository.GetTopByCityAsync(limit, cancellationToken);
        var dtos = items
            .Select(i => new TopItemDto
            {
                Name = i.Name,
                Count = i.Count
            })
            .ToList();

        return Result<IReadOnlyList<TopItemDto>>.Success(dtos);
    }

    public async Task<Result<AssignmentSuccessDto>> GetAssignmentSuccessAsync(CancellationToken cancellationToken = default)
    {
        var stats = await _analyticsRepository.GetAssignmentStatsAsync(cancellationToken);
        var dto = new AssignmentSuccessDto
        {
            SuccessRate = stats.Total > 0 ? (double)stats.Accepted / stats.Total * 100 : 0,
            TotalAssignments = stats.Total,
            Accepted = stats.Accepted,
            Declined = stats.Declined,
            Expired = stats.Expired
        };

        return Result<AssignmentSuccessDto>.Success(dto);
    }

    public async Task<Result<IReadOnlyList<CancellationReasonDto>>> GetCancellationReasonsAsync(CancellationToken cancellationToken = default)
    {
        var reasons = await _analyticsRepository.GetCancellationReasonsAsync(cancellationToken);
        var dtos = reasons
            .Select(r => new CancellationReasonDto
            {
                Reason = r.Reason,
                Count = r.Count
            })
            .ToList();

        return Result<IReadOnlyList<CancellationReasonDto>>.Success(dtos);
    }

    public async Task<Result<RepeatRateDto>> GetCustomerRepeatRateAsync(CancellationToken cancellationToken = default)
    {
        var stats = await _analyticsRepository.GetCustomerRepeatStatsAsync(cancellationToken);
        var dto = new RepeatRateDto
        {
            RepeatRate = stats.Total > 0 ? (double)stats.Repeat / stats.Total * 100 : 0,
            OneTimeCustomers = stats.OneTime,
            RepeatCustomers = stats.Repeat,
            TotalCustomers = stats.Total
        };

        return Result<RepeatRateDto>.Success(dto);
    }

    public async Task<Result<IReadOnlyList<ProviderPerformanceItemDto>>> GetProviderPerformanceAsync(int limit, CancellationToken cancellationToken = default)
    {
        var items = await _analyticsRepository.GetProviderPerformanceAsync(limit, cancellationToken);
        var dtos = items
            .Select(i => new ProviderPerformanceItemDto
            {
                ProfessionalId = i.ProfessionalId,
                ProfessionalName = i.Name,
                JobsCompleted = i.Completed,
                AvgRating = i.Rating,
                OnTimeRate = i.OnTime,
                TotalEarnings = i.Earnings
            })
            .ToList();

        return Result<IReadOnlyList<ProviderPerformanceItemDto>>.Success(dtos);
    }

    public async Task<Result<RefundDisputeDto>> GetRefundDisputeRateAsync(CancellationToken cancellationToken = default)
    {
        var stats = await _analyticsRepository.GetRefundDisputeStatsAsync(cancellationToken);
        var dto = new RefundDisputeDto
        {
            RefundRate = stats.PaidBookings > 0 ? (double)stats.Refunded / stats.PaidBookings * 100 : 0,
            DisputeRate = stats.PaidBookings > 0 ? (double)stats.Disputed / stats.PaidBookings * 100 : 0,
            TotalPaidBookings = stats.PaidBookings,
            RefundedBookings = stats.Refunded,
            DisputedBookings = stats.Disputed,
            TotalRefunded = stats.RefundedAmount
        };

        return Result<RefundDisputeDto>.Success(dto);
    }
}