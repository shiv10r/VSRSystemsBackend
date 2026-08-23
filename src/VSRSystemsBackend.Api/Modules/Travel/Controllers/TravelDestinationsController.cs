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
public class TravelDestinationsController : ControllerBase
{
    private readonly ITravelDestinationService _destinationService;

    public TravelDestinationsController(ITravelDestinationService destinationService)
    {
        _destinationService = destinationService;
    }

    [HttpGet("destinations")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelDestinationDto>>>> GetDestinations(CancellationToken cancellationToken = default)
    {
        var result = await _destinationService.GetDestinationsAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelDestinationDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelDestinationDto>>.Ok(result.Value!));
    }

    [HttpGet("destinations/active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelDestinationDto>>>> GetActiveDestinations(CancellationToken cancellationToken = default)
    {
        var result = await _destinationService.GetActiveDestinationsAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelDestinationDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelDestinationDto>>.Ok(result.Value!));
    }

    [HttpGet("destinations/{id}")]
    public async Task<ActionResult<ApiResponse<TravelDestinationDto>>> GetDestinationById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _destinationService.GetDestinationByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<TravelDestinationDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelDestinationDto>.Ok(result.Value!));
    }

    [HttpPost("destinations")]
    public async Task<ActionResult<ApiResponse<TravelDestinationDto>>> CreateDestination([FromBody] CreateTravelDestinationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _destinationService.CreateDestinationAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelDestinationDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetDestinationBySlug), new { id = result.Value!.Id }, ApiResponse<TravelDestinationDto>.Ok(result.Value!));
    }

    [HttpGet("destinations/slug/{slug}")]
    public async Task<ActionResult<ApiResponse<TravelDestinationDto>>> GetDestinationBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var result = await _destinationService.GetDestinationBySlugAsync(slug, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<TravelDestinationDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelDestinationDto>.Ok(result.Value!));
    }
}