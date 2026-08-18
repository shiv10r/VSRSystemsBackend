using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class SupportService : ISupportService
{
    private static readonly string[] ValidTicketStatuses = { "open", "in_progress", "resolved", "closed" };

    private readonly ISupportRepository _supportRepository;
    private readonly IDisputeRepository _disputeRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IBookingRepository _bookingRepository;

    public SupportService(
        ISupportRepository supportRepository,
        IDisputeRepository disputeRepository,
        INotificationRepository notificationRepository,
        IBookingRepository bookingRepository)
    {
        _supportRepository = supportRepository;
        _disputeRepository = disputeRepository;
        _notificationRepository = notificationRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<SupportTicketDto>> CreateTicketAsync(CreateSupportTicketDto dto, CancellationToken cancellationToken = default)
    {
        var ticket = new SupportTicket
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            TicketNumber = GenerateTicketNumber(),
            RaisedBy = "customer",
            Role = "customer",
            BookingId = dto.BookingId,
            Category = dto.Category,
            Subject = dto.Subject,
            Description = dto.Description,
            Status = "open",
            Priority = dto.Priority,
            CreatedAt = DateTime.UtcNow
        };

        await _supportRepository.AddAsync(ticket, cancellationToken);
        return Result<SupportTicketDto>.Success(MapTicketToDto(ticket));
    }

    public async Task<Result<SupportTicketDto>> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default)
    {
        var ticket = await _supportRepository.GetByTicketNumberAsync(ticketNumber, cancellationToken);
        if (ticket == null)
            return Result<SupportTicketDto>.Failure("Ticket not found");

        return Result<SupportTicketDto>.Success(MapTicketToDto(ticket));
    }

    public async Task<Result<IReadOnlyList<SupportTicketDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var tickets = await _supportRepository.GetByCustomerAsync(customerId, cancellationToken);
        var dtos = tickets.Select(MapTicketToDto).ToList();
        return Result<IReadOnlyList<SupportTicketDto>>.Success(dtos);
    }

    public async Task<Result<SupportTicketDto>> UpdateStatusAsync(string ticketNumber, string status, string? note, CancellationToken cancellationToken = default)
    {
        var ticket = await _supportRepository.GetByTicketNumberAsync(ticketNumber, cancellationToken);
        if (ticket == null)
            return Result<SupportTicketDto>.Failure("Ticket not found");
        if (!ValidTicketStatuses.Contains(status))
            return Result<SupportTicketDto>.Failure("Invalid status");

        ticket.Status = status;
        if (status == "resolved" && !string.IsNullOrWhiteSpace(note))
            ticket.Resolution = note;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _supportRepository.UpdateAsync(ticket, cancellationToken);
        return Result<SupportTicketDto>.Success(MapTicketToDto(ticket));
    }

    public async Task<Result<DisputeDto>> OpenDisputeAsync(CreateDisputeDto dto, CancellationToken cancellationToken = default)
    {
        var dispute = new Dispute
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            TicketId = dto.TicketId,
            BookingId = dto.BookingId,
            RaisedBy = "customer",
            Reason = dto.Reason,
            Details = dto.Details,
            Status = "open",
            CreatedAt = DateTime.UtcNow
        };

        await _disputeRepository.AddAsync(dispute, cancellationToken);
        return Result<DisputeDto>.Success(await MapDisputeToDtoAsync(dispute, cancellationToken));
    }

    public async Task<Result<DisputeDto>> ResolveDisputeAsync(string disputeId, string resolution, string resolutionNote, CancellationToken cancellationToken = default)
    {
        var dispute = await _disputeRepository.GetByIdAsync(disputeId, cancellationToken);
        if (dispute == null)
            return Result<DisputeDto>.Failure("Dispute not found");

        dispute.Status = "resolved";
        dispute.Resolution = resolution;
        dispute.ResolvedBy = "ops";
        dispute.ResolvedAt = DateTime.UtcNow;
        dispute.UpdatedAt = DateTime.UtcNow;

        await _disputeRepository.UpdateAsync(dispute, cancellationToken);
        return Result<DisputeDto>.Success(await MapDisputeToDtoAsync(dispute, cancellationToken));
    }

    public async Task<Result<IReadOnlyList<NotificationDto>>> GetNotificationsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.GetByUserAsync(userId, cancellationToken);
        var dtos = notifications.Select(MapNotificationToDto).ToList();
        return Result<IReadOnlyList<NotificationDto>>.Success(dtos);
    }

    public async Task<Result<int>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var count = await _notificationRepository.GetUnreadCountAsync(userId, cancellationToken);
        return Result<int>.Success(count);
    }

    public async Task<Result> MarkNotificationReadAsync(string notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken);
        if (notification == null)
            return Result.Failure("Notification not found");

        notification.ReadAt = DateTime.UtcNow;
        notification.UpdatedAt = DateTime.UtcNow;

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
        return Result.Success();
    }

    private static string GenerateTicketNumber()
    {
        return $"TKT-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 10000)}";
    }

    private static SupportTicketDto MapTicketToDto(SupportTicket ticket)
    {
        return new SupportTicketDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            RaisedBy = ticket.RaisedBy,
            Role = ticket.Role,
            BookingId = ticket.BookingId,
            Category = ticket.Category,
            Subject = ticket.Subject,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            AssignedTo = ticket.AssignedTo,
            Resolution = ticket.Resolution,
            CreatedAt = ticket.CreatedAt
        };
    }

    private async Task<DisputeDto> MapDisputeToDtoAsync(Dispute dispute, CancellationToken cancellationToken)
    {
        var bookingNumber = string.Empty;
        var booking = await _bookingRepository.GetByIdAsync(dispute.BookingId, cancellationToken);
        if (booking != null)
            bookingNumber = booking.BookingNumber;

        return new DisputeDto
        {
            Id = dispute.Id,
            TicketId = dispute.TicketId,
            BookingId = dispute.BookingId,
            BookingNumber = bookingNumber,
            RaisedBy = dispute.RaisedBy,
            Reason = dispute.Reason,
            Details = dispute.Details,
            Status = dispute.Status,
            Resolution = dispute.Resolution,
            ResolvedBy = dispute.ResolvedBy,
            ResolvedAt = dispute.ResolvedAt
        };
    }

    private static NotificationDto MapNotificationToDto(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Channel = notification.Channel,
            Template = notification.Template,
            PayloadJson = notification.PayloadJson,
            SentAt = notification.SentAt,
            ReadAt = notification.ReadAt
        };
    }
}