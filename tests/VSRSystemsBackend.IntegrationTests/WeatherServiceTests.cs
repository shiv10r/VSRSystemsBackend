using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VSRSystemsBackend.Api.Platform.Weather;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class WeatherServiceTests
{
    private const string ProviderResponse = """
        {
          "current": {
            "time": "2026-08-24T14:00",
            "temperature_2m": 27.4,
            "apparent_temperature": 29.1,
            "relative_humidity_2m": 73,
            "precipitation": 0.2,
            "weather_code": 61,
            "wind_speed_10m": 12.5,
            "is_day": 1
          },
          "hourly": {
            "time": ["2026-08-24T13:00", "2026-08-24T14:00"],
            "precipitation_probability": [20, 65]
          },
          "daily": {
            "time": ["2026-08-24", "2026-08-25"],
            "weather_code": [61, 2],
            "temperature_2m_max": [30.2, 31.0],
            "temperature_2m_min": [22.1, 21.8],
            "precipitation_probability_max": [80, 25]
          }
        }
        """;

    [Fact]
    public async Task MapsFrontendContractAndCachesRoundedCoordinates()
    {
        var handler = new PlatformTestHttpHandler((_, _) =>
            Task.FromResult(PlatformTestHttpHandler.Json(ProviderResponse)));
        var service = CreateService(handler);

        var first = await service.GetAsync(12.9716001, 77.5946001);
        var second = await service.GetAsync(12.9716002, 77.5946002);

        Assert.Equal(27.4, first.Temperature);
        Assert.Equal(29.1, first.FeelsLike);
        Assert.Equal(73, first.Humidity);
        Assert.Equal(12.5, first.WindSpeed);
        Assert.Equal(65, first.RainProbability);
        Assert.Equal(0.2, first.Precipitation);
        Assert.Equal(61, first.WeatherCode);
        Assert.True(first.IsDay);
        Assert.Equal("Rain", first.Condition);
        Assert.Equal(2, first.Forecast.Count);
        Assert.Equal(new DateOnly(2026, 8, 24), first.Forecast[0].Date);
        Assert.Equal(80, first.Forecast[0].RainProbability);
        Assert.Equal(first.Temperature, second.Temperature);
        Assert.Equal(first.UpdatedAt, second.UpdatedAt);
        Assert.Equal(first.Forecast.ToArray(), second.Forecast.ToArray());
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task ProviderFailureIsMappedToWeatherException()
    {
        var handler = new PlatformTestHttpHandler((_, _) => Task.FromResult(
            new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)));
        var service = CreateService(handler);

        await Assert.ThrowsAsync<WeatherProviderException>(() => service.GetAsync(12.97, 77.59));
    }

    [Fact]
    public async Task ControllerRejectsCoordinatesOutsideProviderRange()
    {
        var handler = new PlatformTestHttpHandler((_, _) =>
            Task.FromResult(PlatformTestHttpHandler.Json(ProviderResponse)));
        var controller = new WeatherController(CreateService(handler));

        var result = await controller.Get(91, 77, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task ControllerRejectsMissingCoordinates()
    {
        var handler = new PlatformTestHttpHandler((_, _) =>
            Task.FromResult(PlatformTestHttpHandler.Json(ProviderResponse)));
        var controller = new WeatherController(CreateService(handler));

        var result = await controller.Get(null, 77, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(0, handler.CallCount);
    }

    private static OpenMeteoWeatherService CreateService(HttpMessageHandler handler)
    {
        var services = new ServiceCollection();
        services.AddDistributedMemoryCache();
        var cache = services.BuildServiceProvider().GetRequiredService<IDistributedCache>();
        return new OpenMeteoWeatherService(
            new HttpClient(handler) { BaseAddress = new Uri("https://api.open-meteo.com/") },
            cache,
            Options.Create(new WeatherOptions { CacheMinutes = 15, ForecastDays = 7 }));
    }
}
