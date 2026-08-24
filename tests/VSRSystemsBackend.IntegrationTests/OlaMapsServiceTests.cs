using System.Net;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VSRSystemsBackend.Api.Platform.Maps;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class OlaMapsServiceTests
{
    private const string GeocodeResponse = """
        {
          "geocodingResults": [
            {
              "formattedAddress": "MG Road, Bengaluru, Karnataka",
              "placeId": "ola-place-1",
              "geometry": { "location": { "lat": 12.9757, "lng": 77.6068 } }
            }
          ]
        }
        """;

    [Fact]
    public async Task SearchNormalizesQueryAndUsesCachedResponse()
    {
        var handler = new StubHttpHandler(_ => JsonResponse(GeocodeResponse));
        var service = CreateService(handler);

        var first = await service.SearchAsync("  MG   Road ");
        var second = await service.SearchAsync("mg road");

        Assert.Single(first);
        Assert.Equal("MG Road, Bengaluru, Karnataka", first[0].Label);
        Assert.Equal(12.9757, first[0].Latitude);
        Assert.Equal(first, second);
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task SearchStopsBeforeCallingProviderWhenMonthlyLimitIsReached()
    {
        var handler = new StubHttpHandler(_ => JsonResponse(GeocodeResponse));
        var service = CreateService(handler, monthlyLimit: 1);

        await service.SearchAsync("MG Road");
        await Assert.ThrowsAsync<MapsQuotaExceededException>(() => service.SearchAsync("Brigade Road"));

        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task SearchMapsUnsuccessfulProviderResponseToProviderException()
    {
        var handler = new StubHttpHandler(_ => new HttpResponseMessage(HttpStatusCode.TooManyRequests));
        var service = CreateService(handler);

        await Assert.ThrowsAsync<MapsProviderException>(() => service.SearchAsync("MG Road"));
    }

    [Fact]
    public async Task SearchDoesNotCallProviderWithoutApiKey()
    {
        var handler = new StubHttpHandler(_ => JsonResponse(GeocodeResponse));
        var service = CreateService(handler, apiKey: string.Empty);

        await Assert.ThrowsAsync<MapsNotConfiguredException>(() => service.SearchAsync("MG Road"));

        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task ReverseGeocodeRoundsCoordinatesForCacheReuse()
    {
        var handler = new StubHttpHandler(_ => JsonResponse("""
            {
              "results": [
                {
                  "formattedAddress": "Vidhana Soudha, Bengaluru",
                  "placeId": "ola-place-2",
                  "geometry": { "location": { "lat": "12.9796", "lng": "77.5907" } }
                }
              ]
            }
            """));
        var service = CreateService(handler);

        var first = await service.ReverseGeocodeAsync(12.9796001, 77.5907001);
        var second = await service.ReverseGeocodeAsync(12.9796002, 77.5907002);

        Assert.Single(first);
        Assert.Equal("Vidhana Soudha, Bengaluru", first[0].Label);
        Assert.Equal(first, second);
        Assert.Equal(1, handler.CallCount);
    }

    private static OlaMapsService CreateService(
        HttpMessageHandler handler,
        int monthlyLimit = 100,
        string apiKey = "test-key")
    {
        var services = new ServiceCollection();
        services.AddDistributedMemoryCache();
        var cache = services.BuildServiceProvider().GetRequiredService<IDistributedCache>();
        var options = Options.Create(new OlaMapsOptions
        {
            ApiKey = apiKey,
            MonthlyProviderCallLimit = monthlyLimit,
            CacheHours = 1,
            MaxResults = 6
        });
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://api.olamaps.io/") };
        return new OlaMapsService(client, cache, options, NullLogger<OlaMapsService>.Instance);
    }

    private static HttpResponseMessage JsonResponse(string json) => new(HttpStatusCode.OK)
    {
        Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
    };

    private sealed class StubHttpHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;

        public StubHttpHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(_responseFactory(request));
        }
    }
}
