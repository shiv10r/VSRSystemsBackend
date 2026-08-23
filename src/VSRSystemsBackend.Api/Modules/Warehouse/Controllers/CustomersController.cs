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
[Route("api/warehouse/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomersController(ICustomerService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Create([FromBody] CreateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CustomerDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, ApiResponse<CustomerDto>.Ok(result.Value));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetById(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CustomerDto>.Fail(result.Error));

        return Ok(ApiResponse<CustomerDto>.Ok(result.Value!));
    }

    [HttpGet("gstin/{gstin}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetByGstin(string gstin, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByGstinAsync(gstin, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CustomerDto>.Fail(result.Error));

        return Ok(ApiResponse<CustomerDto>.Ok(result.Value!));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<CustomerDto>>>> GetAll(
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

        var result = await _service.GetAllAsync(request, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<PagedResult<CustomerDto>>.Fail(result.Error));

        return Ok(ApiResponse<PagedResult<CustomerDto>>.Ok(result.Value!));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CustomerDto>>>> GetActive(CancellationToken cancellationToken = default)
    {
        var result = await _service.GetActiveCustomersAsync(cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<CustomerDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<CustomerDto>>.Ok(result.Value!));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> Update(string id, [FromBody] UpdateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CustomerDto>.Fail(result.Error));

        return Ok(ApiResponse<CustomerDto>.Ok(result.Value!));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Customer deleted successfully"));
    }
}