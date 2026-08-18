using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/payments")]
public class HomeServicePaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public HomeServicePaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-order")]
    public async Task<ActionResult<ApiResponse<PaymentInitiationResponseDto>>> CreateOrder([FromBody] CreatePaymentDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.CreateOrderAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PaymentInitiationResponseDto>.Fail(result.Error));

        return Ok(ApiResponse<PaymentInitiationResponseDto>.Ok(result.Value!));
    }

    [HttpGet("by-booking/{bookingId}")]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> GetByBooking(string bookingId, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.GetByBookingAsync(bookingId, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PaymentDto>.Fail(result.Error));

        return Ok(ApiResponse<PaymentDto>.Ok(result.Value!));
    }

    [HttpPost("{bookingId}/refund")]
    public async Task<ActionResult<ApiResponse<RefundDto>>> Refund(
        string bookingId,
        [FromQuery] string reason,
        [FromQuery] decimal? amount = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.RefundAsync(bookingId, reason, amount, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<RefundDto>.Fail(result.Error));

        return Ok(ApiResponse<RefundDto>.Ok(result.Value!));
    }

    [HttpGet("wallet")]
    public async Task<ActionResult<ApiResponse<WalletDto>>> GetWallet([FromQuery] string customerId, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.GetWalletAsync(customerId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<WalletDto>.Fail(result.Error));

        return Ok(ApiResponse<WalletDto>.Ok(result.Value!));
    }

    [HttpGet("by-customer")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PaymentDto>>>> GetByCustomer([FromQuery] string customerId, CancellationToken cancellationToken = default)
    {
        var result = await _paymentService.GetByCustomerAsync(customerId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<PaymentDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<PaymentDto>>.Ok(result.Value!));
    }
}