using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VSRSystemsBackend.Api.Platform.AI;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class AiGatewayServiceTests
{
    [Fact]
    public async Task UsesDeterministicFallbackWhenNoProviderIsConfigured()
    {
        var handler = new PlatformTestHttpHandler((_, _) => throw new InvalidOperationException());
        var service = CreateService(handler, []);

        var status = service.GetStatus();
        var reply = await service.ChatAsync("Hello", []);

        Assert.False(status.Configured);
        Assert.Equal("safe-fallback", status.Model);
        Assert.True(reply.Ok);
        Assert.False(reply.Configured);
        Assert.Equal(AiGatewayService.FallbackText, reply.Text);
        Assert.Equal("safe-fallback", reply.Model);
        Assert.Equal(0, handler.CallCount);
    }

    [Theory]
    [InlineData(HttpStatusCode.TooManyRequests)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task FallsThroughTransientHttpStatusInConfiguredOrder(HttpStatusCode firstStatus)
    {
        var call = 0;
        var handler = new PlatformTestHttpHandler((_, _) => Task.FromResult(
            handlerCallResponse()));
        HttpResponseMessage handlerCallResponse() => ++call == 1
            ? new HttpResponseMessage(firstStatus)
            : PlatformTestHttpHandler.Json("""
                {"model":"openrouter-model","choices":[{"message":{"content":"Second provider reply"}}],"usage":{"total_tokens":17}}
                """);
        var service = CreateService(handler, Providers());

        var reply = await service.ChatAsync("Hello", []);

        Assert.Equal("Second provider reply", reply.Text);
        Assert.Equal("openrouter-model", reply.Model);
        Assert.Equal(17, reply.Tokens);
        Assert.Equal(2, handler.CallCount);
        Assert.Equal("deepseek-key", handler.Requests[0].Headers.Authorization?.Parameter);
        Assert.Equal("openrouter-key", handler.Requests[1].Headers.Authorization?.Parameter);
    }

    [Fact]
    public async Task FallsThroughNetworkFailure()
    {
        var call = 0;
        var handler = new PlatformTestHttpHandler((_, _) =>
        {
            call++;
            return call == 1
                ? Task.FromException<HttpResponseMessage>(new HttpRequestException("offline"))
                : Task.FromResult(PlatformTestHttpHandler.Json(
                    """{"choices":[{"message":{"content":"Available"}}]}"""));
        });
        var service = CreateService(handler, Providers());

        var reply = await service.ChatAsync("Hello", []);

        Assert.Equal("Available", reply.Text);
        Assert.Equal(2, handler.CallCount);
    }

    [Fact]
    public async Task FallsThroughProviderTimeout()
    {
        var call = 0;
        var handler = new PlatformTestHttpHandler(async (_, cancellationToken) =>
        {
            call++;
            if (call == 1)
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return PlatformTestHttpHandler.Json(
                """{"choices":[{"message":{"content":"After timeout"}}]}""");
        });
        var service = CreateService(handler, Providers(), timeoutSeconds: 1);

        var reply = await service.ChatAsync("Hello", []);

        Assert.Equal("After timeout", reply.Text);
        Assert.Equal(2, handler.CallCount);
    }

    [Fact]
    public async Task DoesNotFallThroughNonTransientClientError()
    {
        var handler = new PlatformTestHttpHandler((_, _) => Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.BadRequest)));
        var service = CreateService(handler, Providers());

        var exception = await Assert.ThrowsAsync<AiProviderRejectedException>(() =>
            service.ChatAsync("Hello", []));

        Assert.Equal("deepseek-chat", exception.Model);
        Assert.Equal(1, handler.CallCount);
        Assert.DoesNotContain("key", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ReturnsFallbackWhenEveryConfiguredProviderIsUnavailable()
    {
        var handler = new PlatformTestHttpHandler((_, _) => Task.FromResult(
            new HttpResponseMessage(HttpStatusCode.BadGateway)));
        var service = CreateService(handler, Providers());

        var reply = await service.ChatAsync("Hello", []);

        Assert.True(reply.Ok);
        Assert.True(reply.Configured);
        Assert.Equal(AiGatewayService.FallbackText, reply.Text);
        Assert.Equal(2, handler.CallCount);
    }

    private static AiGatewayService CreateService(
        HttpMessageHandler handler,
        List<AiProviderOptions> providers,
        int timeoutSeconds = 2) =>
        new(
            new HttpClient(handler),
            Options.Create(new AiGatewayOptions
            {
                TimeoutSeconds = timeoutSeconds,
                Providers = providers
            }),
            NullLogger<AiGatewayService>.Instance);

    private static List<AiProviderOptions> Providers() =>
    [
        new()
        {
            Name = "DeepSeek",
            Endpoint = "https://api.deepseek.com/chat/completions",
            ApiKey = "deepseek-key",
            Model = "deepseek-chat"
        },
        new()
        {
            Name = "OpenRouter",
            Endpoint = "https://openrouter.ai/api/v1/chat/completions",
            ApiKey = "openrouter-key",
            Model = "openrouter-model"
        }
    ];
}
