using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSRSystemsBackend.Api.Platform.Weather;

[ApiController]
[Authorize]
[Route("api/weather")]
public sealed class WeatherController(OpenMeteoWeatherService weatherService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] double? latitude,
        [FromQuery] double? longitude,
        CancellationToken cancellationToken = default)
    {
        if (latitude is null || !double.IsFinite(latitude.Value) || latitude is < -90 or > 90
            || longitude is null || !double.IsFinite(longitude.Value) || longitude is < -180 or > 180)
            return BadRequest(new { ok = false, message = "Coordinates are outside the valid range." });

        try
        {
            var weather = await weatherService.GetAsync(latitude.Value, longitude.Value, cancellationToken);
            return Ok(new { ok = true, weather });
        }
        catch (WeatherProviderException exception)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new { ok = false, message = exception.Message });
        }
    }
}
