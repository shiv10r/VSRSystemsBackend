using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services")]
public class HomeServiceCategoriesController : ControllerBase
{
    private readonly IServiceCatalogService _catalogService;
    private readonly ILocationService _locationService;

    public HomeServiceCategoriesController(IServiceCatalogService catalogService, ILocationService locationService)
    {
        _catalogService = catalogService;
        _locationService = locationService;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ServiceCategoryDto>>>> GetCategories(CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetCategoriesAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ServiceCategoryDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ServiceCategoryDto>>.Ok(result.Value!));
    }

    [HttpGet("categories/active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ServiceCategoryDto>>>> GetActiveCategories(CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetActiveCategoriesAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ServiceCategoryDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ServiceCategoryDto>>.Ok(result.Value!));
    }

    [HttpGet("categories/{slug}")]
    public async Task<ActionResult<ApiResponse<ServiceCategoryDto>>> GetCategoryBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetCategoryBySlugAsync(slug, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<ServiceCategoryDto>.Fail(result.Error));

        return Ok(ApiResponse<ServiceCategoryDto>.Ok(result.Value!));
    }

    [HttpPost("categories")]
    public async Task<ActionResult<ApiResponse<ServiceCategoryDto>>> CreateCategory([FromBody] CreateServiceCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.CreateCategoryAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ServiceCategoryDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetCategoryBySlug), new { slug = result.Value!.Slug }, ApiResponse<ServiceCategoryDto>.Ok(result.Value!));
    }

    [HttpGet("services")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ServiceDto>>>> GetServices(
        [FromQuery] string? categoryId = null,
        [FromQuery] string? cityId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetServicesAsync(categoryId, cityId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ServiceDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ServiceDto>>.Ok(result.Value!));
    }

    [HttpGet("services/{slug}")]
    public async Task<ActionResult<ApiResponse<ServiceDto>>> GetServiceBySlug(string slug, [FromQuery] string? cityId = null, CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetServiceBySlugAsync(slug, cityId, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<ServiceDto>.Fail(result.Error));

        return Ok(ApiResponse<ServiceDto>.Ok(result.Value!));
    }

    [HttpGet("services/{serviceId}/packages")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ServicePackageDto>>>> GetPackages(string serviceId, CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetPackagesAsync(serviceId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ServicePackageDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ServicePackageDto>>.Ok(result.Value!));
    }

    [HttpGet("services/{serviceId}/add-ons")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ServiceAddOnDto>>>> GetAddOns(string serviceId, CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetAddOnsAsync(serviceId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ServiceAddOnDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ServiceAddOnDto>>.Ok(result.Value!));
    }

    [HttpGet("services/{serviceId}/problems")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ServiceProblemDto>>>> GetProblems(string serviceId, CancellationToken cancellationToken = default)
    {
        var result = await _catalogService.GetProblemsAsync(serviceId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ServiceProblemDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ServiceProblemDto>>.Ok(result.Value!));
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<SearchCatalogResultDto>>> Search(
        [FromQuery] string q = "",
        [FromQuery] string? categoryId = null,
        [FromQuery] string? pincode = null,
        [FromQuery] bool? emergencyOnly = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchCatalogQueryDto
        {
            Q = q,
            CategoryId = categoryId,
            Pincode = pincode,
            EmergencyOnly = emergencyOnly,
            Page = page,
            PageSize = pageSize
        };

        var result = await _catalogService.SearchAsync(query, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<SearchCatalogResultDto>.Fail(result.Error));

        return Ok(ApiResponse<SearchCatalogResultDto>.Ok(result.Value!));
    }

    [HttpGet("cities")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CityDto>>>> GetCities(CancellationToken cancellationToken = default)
    {
        var result = await _locationService.GetCitiesAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<CityDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<CityDto>>.Ok(result.Value!));
    }

    [HttpGet("cities/{cityId}/zones")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ZoneDto>>>> GetZones(string cityId, CancellationToken cancellationToken = default)
    {
        var result = await _locationService.GetZonesAsync(cityId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<ZoneDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<ZoneDto>>.Ok(result.Value!));
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

        var result = await _locationService.CheckServiceabilityAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<ServiceabilityResultDto>.Fail(result.Error));

        return Ok(ApiResponse<ServiceabilityResultDto>.Ok(result.Value!));
    }
}