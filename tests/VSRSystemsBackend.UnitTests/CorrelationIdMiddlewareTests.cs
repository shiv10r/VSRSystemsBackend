using Microsoft.AspNetCore.Http;
using VSRSystemsBackend.Api.Infrastructure.Observability;
using Xunit;

namespace VSRSystemsBackend.UnitTests;

public sealed class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task PreservesValidIncomingCorrelationId()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "request-123";
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(context);

        Assert.Equal("request-123", context.TraceIdentifier);
        Assert.Equal("request-123", context.Response.Headers[CorrelationIdMiddleware.HeaderName]);
    }

    [Fact]
    public async Task ReplacesInvalidIncomingCorrelationId()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "invalid header value";
        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask);

        await middleware.InvokeAsync(context);

        Assert.NotEqual("invalid header value", context.TraceIdentifier);
        Assert.False(string.IsNullOrWhiteSpace(context.TraceIdentifier));
        Assert.Equal(context.TraceIdentifier, context.Response.Headers[CorrelationIdMiddleware.HeaderName]);
    }
}
