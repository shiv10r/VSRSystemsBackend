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
public class TravelDeparturesController : ControllerBase
{
    private readonly ITravelDepartureService _departureService;

    public TravelDeparturesController(ITravelDepartureService departureService)
    {
        _departureService = departureService;
    }

    [HttpGet("departures")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelDepartureDto>>>> GetDepartures(
        [FromQuery] string? packageId = null,
        [FromQuery] string? departureCity = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _departureService.GetDeparturesAsync(packageId, departureCity, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelDepartureDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelDepartureDto>>.Ok(result.Value!));
    }

    [HttpGet("departures/active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelDepartureDto>>>> GetActiveDepartures(
        [FromQuery] string? packageId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _departureService.GetActiveDeparturesAsync(packageId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelDepartureDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelDepartureDto>>.Ok(result.Value!));
    }

    [HttpGet("departures/{id}")]
    public async Task<ActionResult<ApiResponse<TravelDepartureDto>>> GetDepartureById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _departureService.GetDepartureByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<TravelDepartureDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelDepartureDto>.Ok(result.Value!));
    }

    [HttpPost("departures")]
    public async Task<ActionResult<ApiResponse<TravelDepartureDto>>> CreateDeparture([FromBody] CreateTravelDepartureDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _departureService.CreateDepartureAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelDepartureDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetDepartureById), new { id = result.Value!.Id }, ApiResponse<TravelDepartureDto>.Ok(result.Value!));
    }
}