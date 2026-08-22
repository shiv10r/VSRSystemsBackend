using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Travel.DTOs;
using VSRSystemsBackend.Application.Travel.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/travel")]
public class TravelBookingsController : ControllerBase
{
    private readonly ITravelBookingService _bookingService;

    public TravelBookingsController(ITravelBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("booking-sessions")]
    public async Task<ActionResult<ApiResponse<TravelBookingSessionDto>>> CreateBookingSession([FromBody] CreateTravelBookingSessionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.CreateBookingSessionAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelBookingSessionDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetBookingSession), new { id = result.Value!.Id }, ApiResponse<TravelBookingSessionDto>.Ok(result.Value!));
    }

    [HttpGet("booking-sessions/{id}")]
    public async Task<ActionResult<ApiResponse<TravelBookingSessionDto>>> GetBookingSession(string id, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.GetBookingSessionAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<TravelBookingSessionDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelBookingSessionDto>.Ok(result.Value!));
    }

    [HttpPost("bookings")]
    public async Task<ActionResult<ApiResponse<TravelBookingDto>>> CreateBooking([FromBody] CreateTravelBookingDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.CreateBookingAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelBookingDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value!.Id }, ApiResponse<TravelBookingDto>.Ok(result.Value!));
    }

    [HttpGet("bookings/{id}")]
    public async Task<ActionResult<ApiResponse<TravelBookingDto>>> GetBooking(string id, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.GetBookingAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<TravelBookingDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelBookingDto>.Ok(result.Value!));
    }

    [HttpGet("me/bookings")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelBookingDto>>>> GetMyBookings(CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.GetMyBookingsAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelBookingDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelBookingDto>>.Ok(result.Value!));
    }

    [HttpPost("bookings/{id}/cancel")]
    public async Task<ActionResult<ApiResponse>> CancelBooking(string id, [FromBody] CancelTravelBookingDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.CancelBookingAsync(id, dto.Reason, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Booking cancelled successfully"));
    }
}