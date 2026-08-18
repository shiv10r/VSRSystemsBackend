using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ICustomerRepository _customerRepository;

    public AssignmentService(
        IBookingRepository bookingRepository,
        IProfessionalRepository professionalRepository,
        ICustomerRepository customerRepository)
    {
        _bookingRepository = bookingRepository;
        _professionalRepository = professionalRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Result<BookingAssignmentDto>> AssignAsync(string bookingId, string professionalId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
            return Result<BookingAssignmentDto>.Failure("Booking not found");

        var professional = await _professionalRepository.GetByIdAsync(professionalId, cancellationToken);
        if (professional == null)
            return Result<BookingAssignmentDto>.Failure("Professional not found");

        if (professional.OnboardingStatus != "verified")
            return Result<BookingAssignmentDto>.Failure("Professional is not verified and cannot be assigned");

        var slotEnd = booking.ExpectedEnd != default ? booking.ExpectedEnd : booking.ScheduledStart.AddMinutes(60);
        var duration = slotEnd - booking.ScheduledStart;
        if (duration <= TimeSpan.Zero)
            duration = TimeSpan.FromMinutes(60);

        if (!await _professionalRepository.IsAvailableAtAsync(professionalId, booking.ScheduledStart, duration, cancellationToken))
            return Result<BookingAssignmentDto>.Failure("Professional is not available at the booking slot");

        var assignment = new BookingAssignment
        {
            Id = NewId(),
            BookingId = booking.Id,
            ProfessionalId = professionalId,
            OfferedAt = DateTime.UtcNow,
            Response = "offered"
        };
        await _bookingRepository.AddAssignmentAsync(assignment, cancellationToken);

        if (booking.Status != "assigned")
        {
            var previous = booking.Status;
            booking.Status = "assigned";
            booking.AssignedProfessionalId = professionalId;
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking, cancellationToken);
            await _bookingRepository.AddStatusHistoryAsync(new BookingStatusHistory
            {
                Id = NewId(),
                BookingId = booking.Id,
                PreviousStatus = previous,
                NewStatus = "assigned",
                ChangedBy = "ops",
                Reason = "Professional offered assignment",
                ChangedAt = DateTime.UtcNow
            }, cancellationToken);
        }
        else
        {
            booking.AssignedProfessionalId = professionalId;
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }

        return Result<BookingAssignmentDto>.Success(ToAssignmentDto(assignment, professional));
    }

    public async Task<Result<IReadOnlyList<BookingAssignmentDto>>> GetAssignmentsAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var assignments = await _bookingRepository.GetAssignmentsAsync(bookingId, cancellationToken);
        var dtos = assignments
            .OrderByDescending(a => a.OfferedAt)
            .Select(a => ToAssignmentDto(a, a.Professional))
            .ToList();

        return Result<IReadOnlyList<BookingAssignmentDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<ProfessionalDto>>> GetEligibleProfessionalsAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking == null)
            return Result<IReadOnlyList<ProfessionalDto>>.Failure("Booking not found");

        var address = await _customerRepository.GetAddressAsync(booking.CustomerId, booking.AddressId, cancellationToken);

        IReadOnlyList<Professional> professionals;
        if (address != null && !string.IsNullOrWhiteSpace(address.CityId) && !string.IsNullOrWhiteSpace(address.ZoneId))
        {
            professionals = await _professionalRepository.GetEligibleProfessionalsAsync(
                booking.ServiceId, address.CityId, address.ZoneId, cancellationToken);
        }
        else
        {
            professionals = await _professionalRepository.GetVerifiedProfessionalsByServiceAsync(booking.ServiceId, cancellationToken);
        }

        var dtos = professionals
            .OrderByDescending(p => p.QualityScore)
            .Select(ToProfessionalDto)
            .ToList();

        return Result<IReadOnlyList<ProfessionalDto>>.Success(dtos);
    }

    private static BookingAssignmentDto ToAssignmentDto(BookingAssignment a, Professional? professional) => new()
    {
        Id = a.Id,
        BookingId = a.BookingId,
        ProfessionalId = a.ProfessionalId,
        ProfessionalName = professional?.DisplayName ?? string.Empty,
        OfferedAt = a.OfferedAt,
        RespondedAt = a.RespondedAt,
        Response = a.Response,
        DeclineReason = a.DeclineReason
    };

    private static ProfessionalDto ToProfessionalDto(Professional p) => new()
    {
        Id = p.Id,
        UserId = p.UserId,
        DisplayName = p.DisplayName,
        Gender = p.Gender,
        Dob = p.Dob,
        OnboardingStatus = p.OnboardingStatus,
        QualityScore = p.QualityScore,
        Tier = p.Tier,
        JoinedAt = p.JoinedAt,
        Phone = p.Phone,
        Email = p.Email,
        JobsCompleted = p.Performances.LastOrDefault()?.JobsCompleted ?? 0,
        AvgRating = p.Performances.LastOrDefault()?.AvgRating ?? 0,
        Skills = p.Skills.Select(s => s.ServiceId).ToList(),
        ServiceAreaNames = p.ServiceAreas.Select(sa => sa.ZoneId).ToList()
    };

    private static string NewId() => Guid.NewGuid().ToString("N")[..20];
}