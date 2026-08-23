using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/professionals")]
public class HomeServiceProfessionalsController : ControllerBase
{
    private readonly IProfessionalService _professionalService;
    private readonly IEarningsService _earningsService;
    private readonly IPayoutService _payoutService;

    public HomeServiceProfessionalsController(
        IProfessionalService professionalService,
        IEarningsService earningsService,
        IPayoutService payoutService)
    {
        _professionalService = professionalService;
        _earningsService = earningsService;
        _payoutService = payoutService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<ProfessionalDto>>>> GetAll(
        [FromQuery] string? status = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            SortBy = sortBy,
            SortDescending = sortDescending
        };

        var result = string.IsNullOrEmpty(status)
            ? await _professionalService.GetAllAsync(request, cancellationToken)
            : await _professionalService.GetByStatusAsync(status, request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<ProfessionalDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<ProfessionalDto>>.Ok(result.Value!));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProfessionalDetailDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _professionalService.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<ProfessionalDetailDto>.Fail(result.Error));

        return Ok(ApiResponse<ProfessionalDetailDto>.Ok(result.Value!));
    }

    [HttpGet("{id}/availability")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProfessionalAvailabilityDto>>>> GetAvailability(string id, CancellationToken cancellationToken = default)
    {
        var result = await _professionalService.GetAvailabilitiesAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ProfessionalAvailabilityDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ProfessionalAvailabilityDto>>.Ok(result.Value!));
    }

    [HttpGet("{id}/earnings")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProfessionalEarningDto>>>> GetEarnings(string id, CancellationToken cancellationToken = default)
    {
        var result = await _earningsService.GetByProfessionalAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ProfessionalEarningDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ProfessionalEarningDto>>.Ok(result.Value!));
    }

    [HttpGet("{id}/payouts")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PayoutDto>>>> GetPayouts(string id, CancellationToken cancellationToken = default)
    {
        var result = await _payoutService.GetByProfessionalAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<PayoutDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<PayoutDto>>.Ok(result.Value!));
    }
}