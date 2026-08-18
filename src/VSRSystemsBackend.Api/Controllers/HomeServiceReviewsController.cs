using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/reviews")]
public class HomeServiceReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public HomeServiceReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ReviewDto>>>> Get(
        [FromQuery] string? professionalId = null,
        [FromQuery] string? serviceId = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(professionalId))
        {
            var result = await _reviewService.GetByProfessionalAsync(professionalId, cancellationToken);
            if (result.IsFailure)
                return BadRequest(ApiResponse<IReadOnlyList<ReviewDto>>.Fail(result.Error));

            return Ok(ApiResponse<IReadOnlyList<ReviewDto>>.Ok(result.Value!));
        }

        if (!string.IsNullOrEmpty(serviceId))
        {
            var result = await _reviewService.GetByServiceAsync(serviceId, cancellationToken);
            if (result.IsFailure)
                return BadRequest(ApiResponse<IReadOnlyList<ReviewDto>>.Fail(result.Error));

            return Ok(ApiResponse<IReadOnlyList<ReviewDto>>.Ok(result.Value!));
        }

        return BadRequest(ApiResponse<IReadOnlyList<ReviewDto>>.Fail("Either professionalId or serviceId is required"));
    }

    [HttpGet("by-booking/{bookingId}")]
    public async Task<ActionResult<ApiResponse<ReviewDto>>> GetByBooking(string bookingId, CancellationToken cancellationToken = default)
    {
        var result = await _reviewService.GetByBookingAsync(bookingId, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<ReviewDto>.Fail(result.Error));

        return Ok(ApiResponse<ReviewDto>.Ok(result.Value!));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReviewDto>>> Submit([FromBody] CreateReviewDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _reviewService.SubmitAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReviewDto>.Fail(result.Error));

        return Ok(ApiResponse<ReviewDto>.Ok(result.Value!));
    }

    [HttpPost("{id}/reply")]
    public async Task<ActionResult<ApiResponse<ReviewDto>>> Reply(string id, [FromQuery] string reply, CancellationToken cancellationToken = default)
    {
        var result = await _reviewService.ReplyAsync(id, reply, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ReviewDto>.Fail(result.Error));

        return Ok(ApiResponse<ReviewDto>.Ok(result.Value!));
    }
}