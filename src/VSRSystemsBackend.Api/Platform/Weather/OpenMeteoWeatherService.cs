using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace VSRSystemsBackend.Api.Platform.Weather;

public sealed class OpenMeteoWeatherService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> CacheLocks = new();
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly WeatherOptions _options;

    public OpenMeteoWeatherService(
        HttpClient httpClient,
        IDistributedCache cache,
        IOptions<WeatherOptions> options)
    {
        _httpClient = httpClient;
        _cache = cache;
        _options = options.Value;
    }

    public async Task<ProjectWeather> GetAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        var coordinateKey = string.Create(CultureInfo.InvariantCulture, $"{latitude:F3}:{longitude:F3}");
        var cacheKey = $"weather:open-meteo:{coordinateKey}";
        var cached = await GetCachedAsync(cacheKey, cancellationToken);
        if (cached is not null)
            return cached;

        var cacheLock = CacheLocks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
        await cacheLock.WaitAsync(cancellationToken);
        try
        {
            cached = await GetCachedAsync(cacheKey, cancellationToken);
            if (cached is not null)
                return cached;

            var forecastDays = Math.Clamp(_options.ForecastDays, 1, 16);
            var path = FormattableString.Invariant(
                $"v1/forecast?latitude={latitude:F5}&longitude={longitude:F5}") +
                "&current=temperature_2m,apparent_temperature,relative_humidity_2m,precipitation,weather_code,wind_speed_10m,is_day" +
                "&hourly=precipitation_probability" +
                "&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max" +
                $"&forecast_days={forecastDays}&timezone=auto";

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(path, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (HttpRequestException exception)
            {
                throw new WeatherProviderException("Open-Meteo could not be reached.", exception);
            }
            catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
            {
                throw new WeatherProviderException("Open-Meteo timed out.", exception);
            }

            using (response)
            {
                if (!response.IsSuccessStatusCode)
                    throw new WeatherProviderException("Open-Meteo returned an unsuccessful response.");

                ProjectWeather weather;
                try
                {
                    await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                    weather = Parse(document.RootElement);
                }
                catch (JsonException exception)
                {
                    throw new WeatherProviderException("Open-Meteo returned invalid weather data.", exception);
                }
                catch (Exception exception) when (exception is InvalidOperationException
                    or KeyNotFoundException
                    or FormatException
                    or IndexOutOfRangeException
                    or ArgumentOutOfRangeException)
                {
                    throw new WeatherProviderException("Open-Meteo returned incomplete weather data.", exception);
                }

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(weather, SerializerOptions),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Math.Max(1, _options.CacheMinutes))
                    },
                    cancellationToken);
                return weather;
            }
        }
        finally
        {
            cacheLock.Release();
        }
    }

    private async Task<ProjectWeather?> GetCachedAsync(string cacheKey, CancellationToken cancellationToken)
    {
        var json = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<ProjectWeather>(json, SerializerOptions);
        }
        catch (JsonException)
        {
            await _cache.RemoveAsync(cacheKey, cancellationToken);
            return null;
        }
    }

    private static ProjectWeather Parse(JsonElement root)
    {
        var current = root.GetProperty("current");
        var currentTime = current.GetProperty("time").GetString() ?? throw new InvalidOperationException();
        var daily = ParseForecast(root.GetProperty("daily"));
        return new ProjectWeather(
            current.GetProperty("temperature_2m").GetDouble(),
            current.GetProperty("apparent_temperature").GetDouble(),
            current.GetProperty("relative_humidity_2m").GetInt32(),
            current.GetProperty("wind_speed_10m").GetDouble(),
            GetCurrentRainProbability(root, currentTime),
            current.GetProperty("precipitation").GetDouble(),
            current.GetProperty("weather_code").GetInt32(),
            current.GetProperty("is_day").GetInt32() == 1,
            GetCondition(current.GetProperty("weather_code").GetInt32()),
            daily,
            DateTimeOffset.UtcNow);
    }

    private static IReadOnlyList<DailyForecast> ParseForecast(JsonElement daily)
    {
        var dates = daily.GetProperty("time");
        var codes = daily.GetProperty("weather_code");
        var max = daily.GetProperty("temperature_2m_max");
        var min = daily.GetProperty("temperature_2m_min");
        var rain = daily.GetProperty("precipitation_probability_max");
        var result = new List<DailyForecast>(dates.GetArrayLength());
        for (var i = 0; i < dates.GetArrayLength(); i++)
        {
            result.Add(new DailyForecast(
                DateOnly.ParseExact(dates[i].GetString()!, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                codes[i].GetInt32(),
                max[i].GetDouble(),
                min[i].GetDouble(),
                rain[i].ValueKind == JsonValueKind.Null ? 0 : rain[i].GetInt32()));
        }
        return result;
    }

    private static int GetCurrentRainProbability(JsonElement root, string currentTime)
    {
        if (!root.TryGetProperty("hourly", out var hourly)
            || !hourly.TryGetProperty("time", out var times)
            || !hourly.TryGetProperty("precipitation_probability", out var probabilities))
            return 0;

        for (var i = 0; i < times.GetArrayLength(); i++)
        {
            if (times[i].GetString() == currentTime)
                return probabilities[i].ValueKind == JsonValueKind.Null ? 0 : probabilities[i].GetInt32();
        }
        return 0;
    }

    private static string GetCondition(int code) => code switch
    {
        0 => "Clear sky",
        1 => "Mainly clear",
        2 => "Partly cloudy",
        3 => "Overcast",
        45 or 48 => "Fog",
        51 or 53 or 55 => "Drizzle",
        56 or 57 => "Freezing drizzle",
        61 or 63 or 65 => "Rain",
        66 or 67 => "Freezing rain",
        71 or 73 or 75 or 77 => "Snow",
        80 or 81 or 82 => "Rain showers",
        85 or 86 => "Snow showers",
        95 or 96 or 99 => "Thunderstorm",
        _ => "Unknown"
    };
}
