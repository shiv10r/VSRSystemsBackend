using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class BookingService : IBookingService
{
    private static readonly HashSet<string> CancellableFrom = new() { "pending", "quote_approved", "scheduled", "assigned", "accepted" };
    private static readonly HashSet<string> ReschedulableFrom = new() { "scheduled", "accepted" };
    private static readonly HashSet<string> ConfirmableFrom = new() { "pending", "quote_approved" };
    private static readonly HashSet<string> AssignableFrom = new() { "scheduled", "accepted" };
    private static readonly HashSet<string> NoShowFrom = new() { "scheduled", "accepted" };
    private static readonly HashSet<string> ReworkFrom = new() { "completed", "cancelled" };

    private readonly IBookingRepository _bookingRepository;
    private readonly IServiceCatalogRepository _catalogRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPriceQuoteRepository _quoteRepository;

    public BookingService(
        IBookingRepository bookingRepository,
        IServiceCatalogRepository catalogRepository,
        ICustomerRepository customerRepository,
        IPriceQuoteRepository quoteRepository)
    {
        _bookingRepository = bookingRepository;
        _catalogRepository = catalogRepository;
        _customerRepository = customerRepository;
        _quoteRepository = quoteRepository;
    }

    public async Task<Result<BookingDto>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default)
    {
        var package = (await _catalogRepository.GetPackagesByServiceAsync(dto.ServiceId, cancellationToken))
            .FirstOrDefault(p => p.Id == dto.PackageId);
        if (package == null)
            return Result<BookingDto>.Failure("Package not found");

        var customer = await ResolveCustomerByAddressAsync(dto.AddressId, cancellationToken);
        if (customer == null)
            return Result<BookingDto>.Failure("Customer not found for the given address");

        if (!string.IsNullOrWhiteSpace(dto.PriceQuoteId))
        {
            var quote = await _quoteRepository.GetByIdAsync(dto.PriceQuoteId, cancellationToken);
            if (quote == null)
                return Result<BookingDto>.Failure("Price quote not found");
        }

        var addOns = (await _catalogRepository.GetAddOnsByServiceAsync(dto.ServiceId, cancellationToken))
            .Where(a => dto.AddOnIds.Contains(a.Id))
            .ToList();

        var scheduledStart = dto.ScheduledStart ?? DateTime.UtcNow.AddHours(1);
        var booking = new Booking
        {
            Id = NewId(),
            BookingNumber = GenerateBookingNumber(),
            CustomerId = customer.Id,
            AddressId = dto.AddressId,
            ServiceId = dto.ServiceId,
            PackageId = dto.PackageId,
            BookingType = string.IsNullOrWhiteSpace(dto.BookingType) ? "scheduled" : dto.BookingType,
            ScheduledStart = scheduledStart,
            ExpectedEnd = scheduledStart.AddMinutes(package.DurationMins),
            Status = "pending",
            PaymentStatus = "pending",
            PriceQuoteId = dto.PriceQuoteId,
            CurrentQuoteId = dto.PriceQuoteId,
            CustomerNotes = dto.CustomerNotes,
            CreatedAt = DateTime.UtcNow
        };

        booking.Items.Add(new BookingItem
        {
            Id = NewId(),
            BookingId = booking.Id,
            Description = $"{package.Name} package",
            Quantity = 1,
            UnitPrice = package.BasePrice,
            LineTotal = package.BasePrice
        });

        foreach (var addOn in addOns)
        {
            booking.AddOns.Add(new BookingAddOn
            {
                Id = NewId(),
                BookingId = booking.Id,
                AddOnId = addOn.Id,
                Name = addOn.Name,
                Price = addOn.Price
            });
        }

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, string.Empty, "pending", "customer", "Booking created", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDetailDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetWithDetailsAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDetailDto>.Failure("Booking not found");

        return Result<BookingDetailDto>.Success(ToBookingDetailDto(booking));
    }

    public async Task<Result<BookingDto>> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByBookingNumberAsync(bookingNumber, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<IReadOnlyList<BookingDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetByCustomerAsync(customerId, cancellationToken);
        return Result<IReadOnlyList<BookingDto>>.Success(bookings.Select(ToBookingDto).ToList());
    }

    public async Task<Result<IReadOnlyList<BookingDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetByProfessionalAsync(professionalId, cancellationToken);
        return Result<IReadOnlyList<BookingDto>>.Success(bookings.Select(ToBookingDto).ToList());
    }

    public async Task<Result<IReadOnlyList<BookingDto>>> GetUpcomingForProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetUpcomingForProfessionalAsync(professionalId, DateTime.UtcNow, cancellationToken);
        return Result<IReadOnlyList<BookingDto>>.Success(bookings.Select(ToBookingDto).ToList());
    }

    public async Task<Result<IReadOnlyList<BookingDto>>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetByStatusAsync(status, cancellationToken);
        return Result<IReadOnlyList<BookingDto>>.Success(bookings.Select(ToBookingDto).ToList());
    }

    public async Task<Result<BookingDto>> CancelAsync(string id, string reason, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (!CancellableFrom.Contains(booking.Status))
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to cancelled");

        var previous = booking.Status;
        booking.Status = "cancelled";
        booking.CancelReason = reason;
        booking.CancelledAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "cancelled", "customer", reason, cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> RescheduleAsync(string id, RescheduleBookingDto dto, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (!ReschedulableFrom.Contains(booking.Status))
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to rescheduled");

        var previous = booking.Status;
        booking.ScheduledStart = dto.ScheduledStart;

        var package = (await _catalogRepository.GetPackagesByServiceAsync(booking.ServiceId, cancellationToken))
            .FirstOrDefault(p => p.Id == booking.PackageId);
        if (package != null)
            booking.ExpectedEnd = dto.ScheduledStart.AddMinutes(package.DurationMins);

        booking.Status = "rescheduled";
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "rescheduled", "customer", dto.Reason, cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> ConfirmAsync(string id, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (!ConfirmableFrom.Contains(booking.Status))
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to scheduled");

        var previous = booking.Status;
        booking.Status = "scheduled";
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "scheduled", "customer", "Booking confirmed", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> AssignAsync(string id, string professionalId, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (!AssignableFrom.Contains(booking.Status))
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to assigned");

        var previous = booking.Status;
        booking.Status = "assigned";
        booking.AssignedProfessionalId = professionalId;
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);

        await _bookingRepository.AddAssignmentAsync(new BookingAssignment
        {
            Id = NewId(),
            BookingId = booking.Id,
            ProfessionalId = professionalId,
            OfferedAt = DateTime.UtcNow,
            Response = "offered"
        }, cancellationToken);

        await AppendHistoryAsync(booking.Id, previous, "assigned", "ops", "Professional assigned", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> AcceptAsync(string id, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (booking.Status != "assigned")
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to accepted");

        var previous = booking.Status;
        booking.Status = "accepted";
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "accepted", "professional", "Professional accepted the assignment", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> DeclineAsync(string id, string reason, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (booking.Status != "assigned")
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to cancelled");

        var previous = booking.Status;
        booking.Status = "cancelled";
        booking.CancelReason = reason;
        booking.CancelledAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "cancelled", "professional", reason, cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> StartAsync(string id, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (booking.Status != "accepted")
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to in_progress");

        var previous = booking.Status;
        booking.Status = "in_progress";
        booking.ActualStartAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "in_progress", "professional", "Service started", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> CompleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (booking.Status != "in_progress")
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to completed");

        var previous = booking.Status;
        booking.Status = "completed";
        booking.ActualEndAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "completed", "professional", "Service completed", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> MarkNoShowAsync(string id, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (!NoShowFrom.Contains(booking.Status))
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to no_show");

        var previous = booking.Status;
        booking.Status = "no_show";
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "no_show", "ops", "Customer marked as no-show", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> RequestAdditionalWorkAsync(string id, ApproveAdditionalWorkDto dto, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetWithDetailsAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (booking.Status != "in_progress")
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to additional_work_requested");

        var previous = booking.Status;
        foreach (var item in dto.Items)
        {
            booking.Materials.Add(new BookingMaterial
            {
                Id = NewId(),
                BookingId = booking.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.UnitPrice * item.Quantity,
                PhotoUrl = item.PhotoUrl,
                ApprovedByCustomer = false
            });
        }

        booking.Status = "additional_work_requested";
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, "additional_work_requested", "professional", "Additional work requested", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<BookingDto>> ApproveAdditionalWorkAsync(string id, bool approved, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetWithDetailsAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (booking.Status != "additional_work_requested")
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to {(approved ? "additional_work_approved" : "in_progress")}");

        var previous = booking.Status;
        foreach (var material in booking.Materials)
        {
            material.ApprovedByCustomer = approved;
            if (approved)
                material.ApprovedAt = DateTime.UtcNow;
        }

        booking.Status = approved ? "additional_work_approved" : "in_progress";
        booking.UpdatedAt = DateTime.UtcNow;
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await AppendHistoryAsync(booking.Id, previous, booking.Status, "customer", approved ? "Additional work approved" : "Additional work rejected", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(booking));
    }

    public async Task<Result<IReadOnlyList<BookingStatusHistoryDto>>> GetStatusHistoryAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var history = await _bookingRepository.GetStatusHistoryAsync(bookingId, cancellationToken);
        var dtos = history
            .OrderByDescending(h => h.ChangedAt)
            .Select(h => new BookingStatusHistoryDto
            {
                Id = h.Id,
                BookingId = h.BookingId,
                PreviousStatus = h.PreviousStatus,
                NewStatus = h.NewStatus,
                ChangedBy = h.ChangedBy,
                ChangedAt = h.ChangedAt,
                Reason = h.Reason
            })
            .ToList();

        return Result<IReadOnlyList<BookingStatusHistoryDto>>.Success(dtos);
    }

    public async Task<Result<BookingDto>> RebookAsync(string id, CancellationToken cancellationToken = default)
    {
        var booking = await GetBookingAsync(id, cancellationToken);
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        if (!ReworkFrom.Contains(booking.Status))
            return Result<BookingDto>.Failure($"Invalid status transition from {booking.Status} to pending");

        var rebook = new Booking
        {
            Id = NewId(),
            BookingNumber = GenerateBookingNumber(),
            CustomerId = booking.CustomerId,
            AddressId = booking.AddressId,
            ServiceId = booking.ServiceId,
            PackageId = booking.PackageId,
            BookingType = booking.BookingType,
            ScheduledStart = DateTime.UtcNow.AddDays(1),
            ExpectedEnd = booking.ExpectedEnd,
            Status = "pending",
            PaymentStatus = "pending",
            CustomerNotes = booking.CustomerNotes,
            IsRework = true,
            OriginalBookingId = booking.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _bookingRepository.AddAsync(rebook, cancellationToken);
        await AppendHistoryAsync(rebook.Id, string.Empty, "pending", "customer", $"Rebooked from booking {booking.Id}", cancellationToken);

        return Result<BookingDto>.Success(ToBookingDto(rebook));
    }

    private async Task<Booking?> GetBookingAsync(string id, CancellationToken cancellationToken)
        => await _bookingRepository.GetByIdAsync(id, cancellationToken);

    private async Task<Customer?> ResolveCustomerByAddressAsync(string addressId, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.FindAsync(c => c.Addresses.Any(a => a.Id == addressId), cancellationToken);
        return customers.FirstOrDefault();
    }

    private async Task AppendHistoryAsync(string bookingId, string previous, string newStatus, string changedBy, string reason, CancellationToken cancellationToken)
    {
        await _bookingRepository.AddStatusHistoryAsync(new BookingStatusHistory
        {
            Id = NewId(),
            BookingId = bookingId,
            PreviousStatus = previous,
            NewStatus = newStatus,
            ChangedBy = changedBy,
            Reason = reason,
            ChangedAt = DateTime.UtcNow
        }, cancellationToken);
    }

    private static BookingDto ToBookingDto(Booking b)
    {
        var dto = new BookingDto();
        PopulateBookingFields(dto, b);
        return dto;
    }

    private static BookingDetailDto ToBookingDetailDto(Booking b)
    {
        var dto = new BookingDetailDto
        {
            CustomerNotes = b.CustomerNotes,
            OpsNotes = b.OpsNotes,
            IsRework = b.IsRework,
            OriginalBookingId = b.OriginalBookingId
        };
        PopulateBookingFields(dto, b);

        dto.Items = b.Items.Select(i => new BookingItemDto
        {
            Id = i.Id,
            BookingId = i.BookingId,
            Description = i.Description,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            LineTotal = i.LineTotal
        }).ToList();

        dto.AddOns = b.AddOns.Select(a => new BookingAddOnDto
        {
            Id = a.Id,
            BookingId = a.BookingId,
            AddOnId = a.AddOnId,
            Name = a.Name,
            Price = a.Price
        }).ToList();

        dto.Materials = b.Materials.Select(m => new BookingMaterialDto
        {
            Id = m.Id,
            BookingId = m.BookingId,
            Name = m.Name,
            Quantity = m.Quantity,
            UnitPrice = m.UnitPrice,
            LineTotal = m.LineTotal,
            PhotoUrl = m.PhotoUrl,
            ApprovedByCustomer = m.ApprovedByCustomer
        }).ToList();

        dto.Assignments = b.Assignments.Select(a => new BookingAssignmentDto
        {
            Id = a.Id,
            BookingId = a.BookingId,
            ProfessionalId = a.ProfessionalId,
            ProfessionalName = a.Professional?.DisplayName ?? string.Empty,
            OfferedAt = a.OfferedAt,
            RespondedAt = a.RespondedAt,
            Response = a.Response,
            DeclineReason = a.DeclineReason
        }).ToList();

        dto.StatusHistory = b.StatusHistory.Select(h => new BookingStatusHistoryDto
        {
            Id = h.Id,
            BookingId = h.BookingId,
            PreviousStatus = h.PreviousStatus,
            NewStatus = h.NewStatus,
            ChangedBy = h.ChangedBy,
            ChangedAt = h.ChangedAt,
            Reason = h.Reason
        }).ToList();

        dto.Notes = b.Notes.Select(n => new BookingNoteDto
        {
            Id = n.Id,
            BookingId = n.BookingId,
            AuthorId = n.AuthorId,
            Note = n.Note,
            Visibility = n.Visibility
        }).ToList();

        return dto;
    }

    private static void PopulateBookingFields(BookingDto dto, Booking b)
    {
        dto.Id = b.Id;
        dto.BookingNumber = b.BookingNumber;
        dto.CustomerId = b.CustomerId;
        dto.CustomerName = b.Customer?.DisplayName ?? string.Empty;
        dto.AddressId = b.AddressId;
        dto.ServiceId = b.ServiceId;
        dto.ServiceName = b.Service?.Name ?? string.Empty;
        dto.PackageId = b.PackageId;
        dto.PackageName = b.Package?.Name ?? string.Empty;
        dto.BookingType = b.BookingType;
        dto.ScheduledStart = b.ScheduledStart;
        dto.ExpectedEnd = b.ExpectedEnd;
        dto.Status = b.Status;
        dto.AssignedProfessionalId = b.AssignedProfessionalId;
        dto.PaymentStatus = b.PaymentStatus;
        dto.ActualStartAt = b.ActualStartAt;
        dto.ActualEndAt = b.ActualEndAt;
        dto.CancelledAt = b.CancelledAt;
        dto.CancelReason = b.CancelReason;
    }

    private static string NewId() => Guid.NewGuid().ToString("N")[..20];

    private static string GenerateBookingNumber()
        => $"HS-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(0, 10000):D4}";
}