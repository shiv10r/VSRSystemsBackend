using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Modules.Railway.API.Contracts;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

[ApiController]
[Authorize]
[Route("api/railway/master-data")]
public sealed class RailwayMasterDataController(
    IRailwayScopeAccessor scopeAccessor,
    MasterDataHandlers handlers) : ControllerBase
{
    [HttpGet("assets", Name = "railway.master-data.assets.list")]
    public async Task<ActionResult<RailwayPage<AssetSummary>>> ListAssets(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePage(ref page, ref pageSize)) return BadRequest();
        try
        {
            return Ok(await handlers.ListAssetsAsync(scopeAccessor.GetRequiredScope(), page, pageSize, cancellationToken));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("routes", Name = "railway.master-data.routes.list")]
    public async Task<ActionResult<RailwayPage<RouteSummary>>> ListRoutes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePage(ref page, ref pageSize)) return BadRequest();
        try
        {
            return Ok(await handlers.ListRoutesAsync(scopeAccessor.GetRequiredScope(), page, pageSize, cancellationToken));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("stations", Name = "railway.master-data.stations.list")]
    public async Task<ActionResult<RailwayPage<StationSummary>>> ListStations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePage(ref page, ref pageSize)) return BadRequest();
        try
        {
            return Ok(await handlers.ListStationsAsync(scopeAccessor.GetRequiredScope(), page, pageSize, cancellationToken));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("types/{kind}", Name = "railway.master-data.types.list")]
    public async Task<ActionResult<RailwayPage<MasterRecordSummary>>> List(
        string kind,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        if (!ValidatePage(ref page, ref pageSize)) return BadRequest();
        try
        {
            return Ok(await handlers.ListAsync(scopeAccessor.GetRequiredScope(), kind, page, pageSize, cancellationToken));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException exception)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: exception.Message);
        }
    }

    [HttpGet("~/api/railway/timetable-services", Name = "railway.timetable-services.list")]
    public Task<ActionResult<RailwayPage<MasterRecordSummary>>> ListTimetableServices(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default) =>
        List("timetable-services", page, pageSize, cancellationToken);

    [HttpPost("types/{kind}", Name = "railway.master-data.types.create")]
    public async Task<ActionResult<MasterRecordSummary>> Create(
        string kind,
        CreateRailwayMasterRecordRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Idempotency-Key is required.");
        try
        {
            var result = await handlers.CreateAsync(scopeAccessor.GetRequiredScope(), kind, request, cancellationToken);
            return CreatedAtAction(nameof(List), new { kind }, result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException exception)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: exception.Message);
        }
    }

    [HttpPut("types/{kind}/{id:guid}", Name = "railway.master-data.types.update")]
    public async Task<ActionResult<MasterRecordSummary>> Update(
        string kind,
        Guid id,
        UpdateRailwayMasterRecordRequest request,
        [FromHeader(Name = "If-Match")] string ifMatch,
        CancellationToken cancellationToken)
    {
        if (!TryParseVersion(ifMatch, out var expectedVersion))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "A valid If-Match version is required.");
        try
        {
            return Ok(await handlers.UpdateAsync(scopeAccessor.GetRequiredScope(), kind, id, request, expectedVersion, cancellationToken));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException exception)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: exception.Message);
        }
        catch (ArgumentException exception)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: exception.Message);
        }
    }

    [HttpDelete("types/{kind}/{id:guid}", Name = "railway.master-data.types.retire")]
    public async Task<IActionResult> Retire(
        string kind,
        Guid id,
        [FromHeader(Name = "If-Match")] string ifMatch,
        CancellationToken cancellationToken)
    {
        if (!TryParseVersion(ifMatch, out var expectedVersion))
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "A valid If-Match version is required.");
        try
        {
            await handlers.RetireAsync(scopeAccessor.GetRequiredScope(), kind, id, expectedVersion, DateTimeOffset.UtcNow, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException exception)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: exception.Message);
        }
        catch (ArgumentException exception)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: exception.Message);
        }
    }

    private static bool ValidatePage(ref int page, ref int pageSize)
    {
        if (page < 1 || pageSize < 1) return false;
        pageSize = Math.Min(pageSize, 200);
        return true;
    }

    private static bool TryParseVersion(string? ifMatch, out long version) =>
        long.TryParse(ifMatch?.Trim().Trim('"'), out version) && version >= 0;
}
