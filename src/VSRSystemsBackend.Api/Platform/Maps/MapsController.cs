using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSRSystemsBackend.Api.Platform.Maps;

[ApiController]
[Authorize]
[Route("api/maps")]
public sealed class MapsController : ControllerBase
{
    private readonly GeoapifyService _maps;

    public MapsController(GeoapifyService maps)
    {
        _maps = maps;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<MapLocation>>> Search(
        [FromQuery] string query,
        [FromQuery] int limit = 6,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2 || query.Length > 200)
            return BadRequest(new { error = "Query must contain between 2 and 200 characters." });
        if (limit is < 1 or > 10)
            return BadRequest(new { error = "Limit must be between 1 and 10." });

        try
        {
            var locations = await _maps.SearchAsync(query, cancellationToken);
            return Ok(locations.Take(limit));
        }
        catch (MapsNotConfiguredException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status503ServiceUnavailable);
        }
        catch (MapsQuotaExceededException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status429TooManyRequests);
        }
        catch (MapsProviderException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status502BadGateway);
        }
    }

    [HttpGet("reverse")]
    public async Task<ActionResult<MapLocation>> Reverse(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        CancellationToken cancellationToken = default)
    {
        if (latitude is < -90 or > 90 || longitude is < -180 or > 180)
            return BadRequest(new { error = "Coordinates are outside the valid range." });

        try
        {
            var locations = await _maps.ReverseGeocodeAsync(latitude, longitude, cancellationToken);
            return locations.Count == 0 ? NotFound() : Ok(locations[0]);
        }
        catch (MapsNotConfiguredException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status503ServiceUnavailable);
        }
        catch (MapsQuotaExceededException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status429TooManyRequests);
        }
        catch (MapsProviderException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status502BadGateway);
        }
    }
}
