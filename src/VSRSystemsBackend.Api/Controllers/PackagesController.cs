using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/warehouse/packages")]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _service;

    public PackagesController(IPackageService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PackageDto>>> Create([FromBody] CreatePackageDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PackageDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<PackageDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PackageDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PackageDto>.Fail(result.Error));

        return Ok(ApiResponse<PackageDto>.Ok(result.Value!));
    }

    [HttpGet("package-id/{packageId}")]
    public async Task<ActionResult<ApiResponse<PackageDto>>> GetByPackageId(string packageId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByPackageIdAsync(packageId, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<PackageDto>.Fail(result.Error));

        return Ok(ApiResponse<PackageDto>.Ok(result.Value!));
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<PackageDto>>>> GetByOrder(
        string orderId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByOrderIdAsync(orderId, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PackageDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PackageDto>>.Ok(result.Value!));
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<ApiResponse<PagedResult<PackageDto>>>> GetByStatus(
        string status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _service.GetByStatusAsync(status, request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<PackageDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<PackageDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PackageDto>>> Update(string id, [FromBody] UpdatePackageDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PackageDto>.Fail(result.Error));

        return Ok(ApiResponse<PackageDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Package deleted successfully"));
    }
}