using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Travel.DTOs;
using VSRSystemsBackend.Application.Travel.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/travel")]
public class TravelPaymentsController : ControllerBase
{
    private readonly ITravelPaymentService _paymentService;

    public TravelPaymentsController(ITravelPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("booking-sessions/{sessionId}/payment-order")]
    public async Task<ActionResult<ApiResponse<TravelPaymentOrderDto>>> CreatePaymentOrder(string sessionId, [FromBody] CreateTravelPaymentOrderDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.CreatePaymentOrderAsync(sessionId, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelPaymentOrderDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelPaymentOrderDto>.Ok(result.Value!));
    }

    [HttpPost("payments/verify")]
    public async Task<ActionResult<ApiResponse<TravelPaymentVerificationDto>>> VerifyPayment([FromBody] TravelPaymentVerificationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.VerifyPaymentAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelPaymentVerificationDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelPaymentVerificationDto>.Ok(result.Value!));
    }

    [HttpGet("bookings/{bookingId}/payments")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelPaymentDto>>>> GetPaymentsByBooking(string bookingId, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.GetPaymentsByBookingAsync(bookingId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelPaymentDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelPaymentDto>>.Ok(result.Value!));
    }

    [HttpPost("refunds")]
    public async Task<ActionResult<ApiResponse<TravelRefundDto>>> CreateRefund([FromBody] CreateTravelRefundDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.CreateRefundAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelRefundDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelRefundDto>.Ok(result.Value!));
    }

    [HttpGet("refunds/{refundId}")]
    public async Task<ActionResult<ApiResponse<TravelRefundDto>>> GetRefund(string refundId, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.GetRefundAsync(refundId, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<TravelRefundDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelRefundDto>.Ok(result.Value!));
    }
}