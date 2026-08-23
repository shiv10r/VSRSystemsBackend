using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/bookings")]
public class HomeServiceBookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IPriceQuoteService _priceQuoteService;
    private readonly IAssignmentService _assignmentService;

    public HomeServiceBookingsController(
        IBookingService bookingService,
        IPriceQuoteService priceQuoteService,
        IAssignmentService assignmentService)
    {
        _bookingService = bookingService;
        _priceQuoteService = priceQuoteService;
        _assignmentService = assignmentService;
    }

    [HttpPost("price-quotes")]
    public async Task<ActionResult<ApiResponse<PriceQuoteDto>>> CreatePriceQuote([FromBody] CreatePriceQuoteDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _priceQuoteService.CreateQuoteAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PriceQuoteDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetPriceQuoteById), new { id = result.Value!.Id }, ApiResponse<PriceQuoteDto>.Ok(result.Value));
    }

    [HttpGet("price-quotes/{id}")]
    public async Task<ActionResult<ApiResponse<PriceQuoteDto>>> GetPriceQuoteById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _priceQuoteService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PriceQuoteDto>.Fail(result.Error));

        return Ok(ApiResponse<PriceQuoteDto>.Ok(result.Value!));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Create([FromBody] CreateBookingDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BookingDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<BookingDto>.Ok(result.Value));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BookingDto>>>> GetByCustomer([FromQuery] string customerId, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.GetByCustomerAsync(customerId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<BookingDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<BookingDto>>.Ok(result.Value!));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BookingDetailDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<BookingDetailDto>.Fail(result.Error));

        return Ok(ApiResponse<BookingDetailDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Cancel(string id, [FromBody] CancelBookingDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.CancelAsync(id, dto.Reason, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BookingDto>.Fail(result.Error));

        return Ok(ApiResponse<BookingDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/assign")]
    public async Task<ActionResult<ApiResponse<BookingAssignmentDto>>> Assign(string id, [FromQuery] string professionalId, CancellationToken cancellationToken = default)
    {
        var result = await _assignmentService.AssignAsync(id, professionalId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BookingAssignmentDto>.Fail(result.Error));

        return Ok(ApiResponse<BookingAssignmentDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/confirm")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Confirm(string id, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.ConfirmAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BookingDto>.Fail(result.Error));

        return Ok(ApiResponse<BookingDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Start(string id, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.StartAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BookingDto>.Fail(result.Error));

        return Ok(ApiResponse<BookingDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/complete")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Complete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.CompleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BookingDto>.Fail(result.Error));

        return Ok(ApiResponse<BookingDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/reschedule")]
    public async Task<ActionResult<ApiResponse<BookingDto>>> Reschedule(string id, [FromBody] RescheduleBookingDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _bookingService.RescheduleAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<BookingDto>.Fail(result.Error));

        return Ok(ApiResponse<BookingDto>.Ok(result.Value!));
    }
}