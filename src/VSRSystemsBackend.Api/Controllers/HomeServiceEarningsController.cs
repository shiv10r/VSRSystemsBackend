using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/professional")]
public class HomeServiceEarningsController : ControllerBase
{
    private readonly IEarningsService _earningsService;
    private readonly IPayoutService _payoutService;

    public HomeServiceEarningsController(IEarningsService earningsService, IPayoutService payoutService)
    {
        _earningsService = earningsService;
        _payoutService = payoutService;
    }

    [HttpGet("earnings")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProfessionalEarningDto>>>> GetEarnings([FromQuery] string professionalId, CancellationToken cancellationToken = default)
    {
        var result = await _earningsService.GetByProfessionalAsync(professionalId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ProfessionalEarningDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ProfessionalEarningDto>>.Ok(result.Value!));
    }

    [HttpGet("earnings/summary")]
    public async Task<ActionResult<ApiResponse<EarningsSummaryDto>>> GetEarningsSummary([FromQuery] string professionalId, CancellationToken cancellationToken = default)
    {
        var result = await _earningsService.GetSummaryAsync(professionalId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<EarningsSummaryDto>.Fail(result.Error));

        return Ok(ApiResponse<EarningsSummaryDto>.Ok(result.Value!));
    }

    [HttpGet("payouts")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PayoutDto>>>> GetPayouts([FromQuery] string professionalId, CancellationToken cancellationToken = default)
    {
        var result = await _payoutService.GetByProfessionalAsync(professionalId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<PayoutDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<PayoutDto>>.Ok(result.Value!));
    }

    [HttpGet("payouts/status")]
    public async Task<ActionResult<ApiResponse<PayoutSummaryDto>>> GetPayoutStatus([FromQuery] string professionalId, CancellationToken cancellationToken = default)
    {
        var result = await _payoutService.GetPayoutStatusAsync(professionalId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PayoutSummaryDto>.Fail(result.Error));

        return Ok(ApiResponse<PayoutSummaryDto>.Ok(result.Value!));
    }

    [HttpPost("payouts/{id}/mark-paid")]
    public async Task<ActionResult<ApiResponse<PayoutDto>>> MarkPayoutPaid(string id, [FromQuery] string? reference = null, CancellationToken cancellationToken = default)
    {
        var result = await _payoutService.MarkPaidAsync(id, reference, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PayoutDto>.Fail(result.Error));

        return Ok(ApiResponse<PayoutDto>.Ok(result.Value!));
    }
}