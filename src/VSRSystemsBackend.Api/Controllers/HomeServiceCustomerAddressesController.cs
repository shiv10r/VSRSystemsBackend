using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/home-services/customers/{customerId}/addresses")]
public class HomeServiceCustomerAddressesController : ControllerBase
{
    private readonly ICustomerAddressesService _service;

    public HomeServiceCustomerAddressesController(ICustomerAddressesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CustomerAddressDto>>>> GetAddresses(string customerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAddressesAsync(customerId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<IReadOnlyList<CustomerAddressDto>>.Fail(result.Error));

        return Ok(ApiResponse<IReadOnlyList<CustomerAddressDto>>.Ok(result.Value!));
    }

    [HttpGet("{addressId}")]
    public async Task<ActionResult<ApiResponse<CustomerAddressDto>>> GetAddress(string customerId, string addressId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAddressAsync(customerId, addressId, cancellationToken);
        if (result.IsFailure)
            return NotFound(ApiResponse<CustomerAddressDto>.Fail(result.Error));

        return Ok(ApiResponse<CustomerAddressDto>.Ok(result.Value!));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerAddressDto>>> Create(string customerId, [FromBody] CreateCustomerAddressDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAddressAsync(customerId, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CustomerAddressDto>.Fail(result.Error));

        return CreatedAtAction(nameof(GetAddress), new { customerId, addressId = result.Value!.Id }, ApiResponse<CustomerAddressDto>.Ok(result.Value!));
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult<ApiResponse<CustomerAddressDto>>> Update(string customerId, string addressId, [FromBody] UpdateCustomerAddressDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAddressAsync(customerId, addressId, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse<CustomerAddressDto>.Fail(result.Error));

        return Ok(ApiResponse<CustomerAddressDto>.Ok(result.Value!));
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult<ApiResponse>> Delete(string customerId, string addressId, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAddressAsync(customerId, addressId, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Address deleted successfully"));
    }

    [HttpPost("set-default")]
    public async Task<ActionResult<ApiResponse>> SetDefault(string customerId, [FromBody] SetDefaultAddressDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _service.SetDefaultAddressAsync(customerId, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(ApiResponse.Fail(result.Error));

        return Ok(ApiResponse.Ok("Default address updated successfully"));
    }
}
