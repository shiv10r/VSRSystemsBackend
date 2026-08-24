using System.Collections.Concurrent;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace VSRSystemsBackend.Api.Platform.Maps;

public sealed class GeoapifyService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> CacheLocks = new();
    private static readonly SemaphoreSlim QuotaLock = new(1, 1);
    private static readonly SemaphoreSlim ProviderRateLock = new(1, 1);
    private static DateTimeOffset _nextProviderCall = DateTimeOffset.MinValue;

    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly GeoapifyOptions _options;
    private readonly ILogger<GeoapifyService> _logger;

    public GeoapifyService(
        HttpClient httpClient,
        IDistributedCache cache,
        IOptions<GeoapifyOptions> options,
        ILogger<GeoapifyService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _options = options.Value;
        _logger = logger;
    }

    public Task<IReadOnlyList<MapLocation>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var cleanQuery = CleanQuery(query);
        var cacheKey = $"maps:geoapify:geocode:{Hash(cleanQuery.ToLowerInvariant())}";
        var requestPath = $"v1/geocode/search?text={Uri.EscapeDataString(cleanQuery)}&format=json&limit={Math.Clamp(_options.MaxResults, 1, 10)}";
        if (!string.IsNullOrWhiteSpace(_options.CountryBias))
            requestPath += $"&bias=countrycode:{Uri.EscapeDataString(_options.CountryBias.Trim().ToLowerInvariant())}";
        return GetLocationsAsync(cacheKey, requestPath, cancellationToken);
    }

    public Task<IReadOnlyList<MapLocation>> ReverseGeocodeAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        var coordinates = string.Create(CultureInfo.InvariantCulture, $"{latitude:F5},{longitude:F5}");
        var cacheKey = $"maps:geoapify:reverse:{coordinates}";
        var requestPath = $"v1/geocode/reverse?lat={latitude.ToString("F5", CultureInfo.InvariantCulture)}&lon={longitude.ToString("F5", CultureInfo.InvariantCulture)}&format=json&limit={Math.Clamp(_options.MaxResults, 1, 10)}";
        return GetLocationsAsync(cacheKey, requestPath, cancellationToken);
    }

    private async Task<IReadOnlyList<MapLocation>> GetLocationsAsync(
        string cacheKey,
        string requestPath,
        CancellationToken cancellationToken)
    {
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

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new MapsNotConfiguredException();

            await WaitForProviderSlotAsync(cancellationToken);
            await ConsumeProviderCallAsync(cancellationToken);

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{requestPath}&apiKey={Uri.EscapeDataString(_options.ApiKey)}");
            request.Headers.Accept.ParseAdd("application/json");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (HttpRequestException exception)
            {
                throw new MapsProviderException("Geoapify could not be reached.", exception);
            }
            catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
            {
                throw new MapsProviderException("Geoapify timed out.", exception);
            }

            using (response)
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Geoapify returned HTTP {StatusCode}", (int)response.StatusCode);
                    throw new MapsProviderException("Geoapify returned an unsuccessful response.");
                }

                IReadOnlyList<MapLocation> locations;
                try
                {
                    await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                    locations = ParseLocations(document.RootElement)
                        .Take(Math.Clamp(_options.MaxResults, 1, 10))
                        .ToArray();
                }
                catch (JsonException exception)
                {
                    throw new MapsProviderException("Geoapify returned invalid JSON.", exception);
                }

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(locations, SerializerOptions),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(Math.Max(1, _options.CacheHours))
                    },
                    cancellationToken);

                return locations;
            }
        }
        finally
        {
            cacheLock.Release();
        }
    }

    private async Task<IReadOnlyList<MapLocation>?> GetCachedAsync(string cacheKey, CancellationToken cancellationToken)
    {
        var json = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<MapLocation[]>(json, SerializerOptions);
        }
        catch (JsonException)
        {
            await _cache.RemoveAsync(cacheKey, cancellationToken);
            return null;
        }
    }

    private async Task ConsumeProviderCallAsync(CancellationToken cancellationToken)
    {
        await QuotaLock.WaitAsync(cancellationToken);
        try
        {
            var now = DateTimeOffset.UtcNow;
            var cacheKey = $"maps:geoapify:usage:{now:yyyyMMdd}";
            var rawCount = await _cache.GetStringAsync(cacheKey, cancellationToken);
            _ = int.TryParse(rawCount, NumberStyles.None, CultureInfo.InvariantCulture, out var count);

            var limit = Math.Max(1, _options.DailyProviderCallLimit);
            if (count >= limit)
                throw new MapsQuotaExceededException();

            count++;
            var warningCount = Math.Clamp(_options.UsageWarningCalls, 1, limit);
            if (count == warningCount)
                _logger.LogWarning("Geoapify usage reached {Count} of {Limit} provider calls today", count, limit);

            var expiry = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero)
                .AddDays(1)
                .AddDays(2);
            await _cache.SetStringAsync(
                cacheKey,
                count.ToString(CultureInfo.InvariantCulture),
                new DistributedCacheEntryOptions { AbsoluteExpiration = expiry },
                cancellationToken);
        }
        finally
        {
            QuotaLock.Release();
        }
    }

    private async Task WaitForProviderSlotAsync(CancellationToken cancellationToken)
    {
        await ProviderRateLock.WaitAsync(cancellationToken);
        try
        {
            var now = DateTimeOffset.UtcNow;
            if (_nextProviderCall > now)
                await Task.Delay(_nextProviderCall - now, cancellationToken);

            var interval = TimeSpan.FromSeconds(1d / Math.Max(1, _options.RequestsPerSecond));
            _nextProviderCall = DateTimeOffset.UtcNow.Add(interval);
        }
        finally
        {
            ProviderRateLock.Release();
        }
    }

    private static IEnumerable<MapLocation> ParseLocations(JsonElement root)
    {
        if (!TryGetResults(root, out var results))
            yield break;

        var index = 0;
        foreach (var result in results.EnumerateArray())
        {
            if (!TryGetCoordinates(result, out var latitude, out var longitude))
                continue;

            var label = GetString(result, "formatted")
                ?? GetString(result, "formattedAddress")
                ?? GetString(result, "description")
                ?? GetString(result, "name");
            if (string.IsNullOrWhiteSpace(label))
                continue;

            var id = GetString(result, "place_id")
                ?? GetString(result, "placeId")
                ?? GetString(result, "id")
                ?? $"geoapify-{latitude:F6}-{longitude:F6}-{index}";
            index++;
            yield return new MapLocation(id, label, latitude, longitude);
        }
    }

    private static bool TryGetResults(JsonElement root, out JsonElement results)
    {
        foreach (var propertyName in new[] { "results", "features", "geocodingResults", "predictions" })
        {
            if (root.TryGetProperty(propertyName, out results) && results.ValueKind == JsonValueKind.Array)
                return true;
        }

        results = default;
        return false;
    }

    private static bool TryGetCoordinates(JsonElement result, out double latitude, out double longitude)
    {
        latitude = 0;
        longitude = 0;
        if (TryGetDouble(result, "lat", out latitude)
            && (TryGetDouble(result, "lon", out longitude) || TryGetDouble(result, "lng", out longitude)))
            return true;

        if (result.TryGetProperty("properties", out var properties)
            && TryGetDouble(properties, "lat", out latitude)
            && (TryGetDouble(properties, "lon", out longitude) || TryGetDouble(properties, "lng", out longitude)))
            return true;

        if (!result.TryGetProperty("geometry", out var geometry)
            || !geometry.TryGetProperty("location", out var location))
            return false;

        return TryGetDouble(location, "lat", out latitude)
            && (TryGetDouble(location, "lng", out longitude) || TryGetDouble(location, "lon", out longitude));
    }

    private static bool TryGetDouble(JsonElement element, string propertyName, out double value)
    {
        value = 0;
        if (!element.TryGetProperty(propertyName, out var property))
            return false;
        if (property.ValueKind == JsonValueKind.Number)
            return property.TryGetDouble(out value);
        return property.ValueKind == JsonValueKind.String
            && double.TryParse(property.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private static string? GetString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;

    private static string CleanQuery(string query) =>
        string.Join(' ', query.Trim().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
}
