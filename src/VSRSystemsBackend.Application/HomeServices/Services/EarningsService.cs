using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class EarningsService : IEarningsService
{
    private readonly IEarningsRepository _earningsRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IServiceCatalogRepository _catalogRepository;

    public EarningsService(
        IEarningsRepository earningsRepository,
        IBookingRepository bookingRepository,
        IServiceCatalogRepository catalogRepository)
    {
        _earningsRepository = earningsRepository;
        _bookingRepository = bookingRepository;
        _catalogRepository = catalogRepository;
    }

    public async Task<Result<ProfessionalEarningDto>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var earning = await _earningsRepository.GetByBookingAsync(bookingId, cancellationToken);
        if (earning == null)
            return Result<ProfessionalEarningDto>.Failure("Earning not found");

        var dto = await ToDtoAsync(earning, cancellationToken);
        return Result<ProfessionalEarningDto>.Success(dto);
    }

    public async Task<Result<IReadOnlyList<ProfessionalEarningDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var earnings = await _earningsRepository.GetByProfessionalAsync(professionalId, cancellationToken);
        var dtos = new List<ProfessionalEarningDto>();
        foreach (var earning in earnings.OrderByDescending(e => e.CreatedAt))
        {
            dtos.Add(await ToDtoAsync(earning, cancellationToken));
        }
        return Result<IReadOnlyList<ProfessionalEarningDto>>.Success(dtos);
    }

    public async Task<Result<EarningsSummaryDto>> GetSummaryAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var earnings = await _earningsRepository.GetByProfessionalAsync(professionalId, cancellationToken);

        var recentEarnings = new List<ProfessionalEarningDto>();
        foreach (var earning in earnings.OrderByDescending(e => e.CreatedAt).Take(10))
        {
            recentEarnings.Add(await ToDtoAsync(earning, cancellationToken));
        }

        return Result<EarningsSummaryDto>.Success(new EarningsSummaryDto
        {
            ProfessionalId = professionalId,
            TotalGross = earnings.Sum(e => e.GrossAmount),
            TotalCommission = earnings.Sum(e => e.CommissionAmount),
            TotalTaxWithheld = earnings.Sum(e => e.TaxWithheldAmount),
            TotalAdjustments = earnings.Sum(e => e.AdjustmentAmount),
            TotalNet = earnings.Sum(e => e.NetAmount),
            AvailableForPayout = earnings.Where(e => e.Status == "settled").Sum(e => e.NetAmount),
            PendingSettlement = earnings.Where(e => e.Status == "pending").Sum(e => e.NetAmount),
            CompletedBookings = earnings.Count,
            RecentEarnings = recentEarnings
        });
    }

    private async Task<ProfessionalEarningDto> ToDtoAsync(ProfessionalEarning earning, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(earning.BookingId, cancellationToken);

        var serviceName = string.Empty;
        if (booking != null)
        {
            var service = await _catalogRepository.GetByIdAsync(booking.ServiceId, cancellationToken);
            serviceName = service?.Name ?? string.Empty;
        }

        return new ProfessionalEarningDto
        {
            Id = earning.Id,
            ProfessionalId = earning.ProfessionalId,
            BookingId = earning.BookingId,
            BookingNumber = booking?.BookingNumber ?? string.Empty,
            ServiceName = serviceName,
            GrossAmount = earning.GrossAmount,
            MaterialsExcludedAmount = earning.MaterialsExcludedAmount,
            CommissionAmount = earning.CommissionAmount,
            AdjustmentAmount = earning.AdjustmentAmount,
            TaxWithheldAmount = earning.TaxWithheldAmount,
            NetAmount = earning.NetAmount,
            Status = earning.Status,
            SettledAt = earning.SettledAt
        };
    }
}