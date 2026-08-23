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
public class TravelPackagesController : ControllerBase
{
    private readonly ITravelPackageService _packageService;

    public TravelPackagesController(ITravelPackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpGet("packages")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelPackageDto>>>> GetPackages(
        [FromQuery] string? destinationId = null,
        [FromQuery] string? theme = null,
        [FromQuery] string? sort = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _packageService.GetPackagesAsync(destinationId, theme, sort, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelPackageDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelPackageDto>>.Ok(result.Value!));
    }

    [HttpGet("packages/active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TravelPackageDto>>>> GetActivePackages(
        [FromQuery] string? destinationId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _packageService.GetActivePackagesAsync(destinationId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<TravelPackageDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<TravelPackageDto>>.Ok(result.Value!));
    }

    [HttpGet("packages/{slug}")]
    public async Task<ActionResult<ApiResponse<TravelPackageDto>>> GetPackageBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var result = await _packageService.GetPackageBySlugAsync(slug, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<TravelPackageDto>.Fail(result.Error));

        return Ok(ApiResponse<TravelPackageDto>.Ok(result.Value!));
    }

    [HttpPost("packages")]
    public async Task<ActionResult<ApiResponse<TravelPackageDto>>> CreatePackage([FromBody] CreateTravelPackageDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _packageService.CreatePackageAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<TravelPackageDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetPackageBySlug), new { slug = result.Value!.Slug }, ApiResponse<TravelPackageDto>.Ok(result.Value!));
    }
}