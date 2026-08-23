using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/areas")]
public class HomeServiceAreasController : ControllerBase
{
    private readonly ILocationService _service;

    public HomeServiceAreasController(ILocationService service)
    {
        _service = service;
    }

    [HttpGet("cities")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CityDto>>>> GetCities(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCitiesAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<CityDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<CityDto>>.Ok(result.Value!));
    }

    [HttpGet("cities/{cityId}/zones")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ZoneDto>>>> GetZones(string cityId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetZonesAsync(cityId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ZoneDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ZoneDto>>.Ok(result.Value!));
    }

    [HttpGet("cities/{cityId}/zones/{zoneId}/localities")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LocalityDto>>>> GetLocalities(string cityId, string zoneId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetLocalitiesAsync(zoneId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<LocalityDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<LocalityDto>>.Ok(result.Value!));
    }

    [HttpGet("serviceability")]
    public async Task<ActionResult<ApiResponse<ServiceabilityResultDto>>> CheckServiceability(
        [FromQuery] string pincode,
        [FromQuery] string? serviceId = null,
        CancellationToken cancellationToken = default)
    {
        var dto = new ServiceabilityRequestDto
        {
            Pincode = pincode,
            ServiceId = serviceId
        };

        var result = await _service.CheckServiceabilityAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ServiceabilityResultDto>.Fail(result.Error));

        return Ok(ApiResponse<ServiceabilityResultDto>.Ok(result.Value!));
    }
}