using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/admin/analytics")]
public class HomeServiceAnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public HomeServiceAnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<AnalyticsSummaryDto>>> GetSummary(CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetSummaryAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<AnalyticsSummaryDto>.Fail(result.Error));

        return Ok(ApiResponse<AnalyticsSummaryDto>.Ok(result.Value!));
    }

    [HttpGet("bookings-trend")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TrendPointDto>>>> GetBookingsTrend([FromQuery] int days = 30, CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetBookingsTrendAsync(days, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TrendPointDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TrendPointDto>>.Ok(result.Value!));
    }

    [HttpGet("revenue-trend")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TrendPointDto>>>> GetRevenueTrend([FromQuery] int days = 30, CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetRevenueTrendAsync(days, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TrendPointDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TrendPointDto>>.Ok(result.Value!));
    }

    [HttpGet("top-categories")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TopItemDto>>>> GetTopCategories([FromQuery] int limit = 10, CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetTopCategoriesAsync(limit, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TopItemDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TopItemDto>>.Ok(result.Value!));
    }

    [HttpGet("top-services")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TopItemDto>>>> GetTopServices([FromQuery] int limit = 10, CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetTopServicesAsync(limit, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TopItemDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TopItemDto>>.Ok(result.Value!));
    }

    [HttpGet("top-cities")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TopItemDto>>>> GetTopCities([FromQuery] int limit = 10, CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetTopCitiesAsync(limit, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TopItemDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TopItemDto>>.Ok(result.Value!));
    }

    [HttpGet("assignment-success")]
    public async Task<ActionResult<ApiResponse<AssignmentSuccessDto>>> GetAssignmentSuccess(CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetAssignmentSuccessAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<AssignmentSuccessDto>.Fail(result.Error));

        return Ok(ApiResponse<AssignmentSuccessDto>.Ok(result.Value!));
    }

    [HttpGet("cancellation-reasons")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CancellationReasonDto>>>> GetCancellationReasons(CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetCancellationReasonsAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<CancellationReasonDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<CancellationReasonDto>>.Ok(result.Value!));
    }

    [HttpGet("customer-repeat-rate")]
    public async Task<ActionResult<ApiResponse<RepeatRateDto>>> GetCustomerRepeatRate(CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetCustomerRepeatRateAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<RepeatRateDto>.Fail(result.Error));

        return Ok(ApiResponse<RepeatRateDto>.Ok(result.Value!));
    }

    [HttpGet("provider-performance")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProviderPerformanceItemDto>>>> GetProviderPerformance([FromQuery] int limit = 10, CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetProviderPerformanceAsync(limit, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ProviderPerformanceItemDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ProviderPerformanceItemDto>>.Ok(result.Value!));
    }

    [HttpGet("refund-dispute-rate")]
    public async Task<ActionResult<ApiResponse<RefundDisputeDto>>> GetRefundDisputeRate(CancellationToken cancellationToken = default)
    {
        var result = await _analyticsService.GetRefundDisputeRateAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<RefundDisputeDto>.Fail(result.Error));

        return Ok(ApiResponse<RefundDisputeDto>.Ok(result.Value!));
    }
}