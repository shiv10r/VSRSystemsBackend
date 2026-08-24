namespace VSRSystemsBackend.Application.HomeServices.DTOs;

public sealed record BookingStatusChangedPayload(
    string BookingId,
    string Status,
    string? AssignedProfessionalId,
    DateTime? ScheduledStart);
