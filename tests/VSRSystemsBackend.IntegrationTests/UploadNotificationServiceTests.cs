using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VSRSystemsBackend.Api.Platform.Storage;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class UploadNotificationServiceTests
{
    [Fact]
    public async Task SendsUploadEmailToConfiguredOwnerAddress()
    {
        string? authorization = null;
        string? body = null;
        var handler = new PlatformTestHttpHandler(async (request, cancellationToken) =>
        {
            authorization = request.Headers.Authorization?.Parameter;
            body = await request.Content!.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var service = CreateService(handler, "resend-secret", "owner@gmail.com");

        var result = await service.SendAsync(
            "uploader@example.com",
            new UploadCompletedRequest("project-media", "operations/file-1", "plan.pdf", "application/pdf", 2048));

        Assert.True(result.Sent);
        Assert.Equal("resend-secret", authorization);
        using var payload = JsonDocument.Parse(body!);
        Assert.Equal("owner@gmail.com", payload.RootElement.GetProperty("to")[0].GetString());
        Assert.Contains("plan.pdf", payload.RootElement.GetProperty("subject").GetString());
    }

    [Fact]
    public async Task DoesNotCallProviderWhenEmailIsNotConfigured()
    {
        var handler = new PlatformTestHttpHandler((_, _) => throw new InvalidOperationException());
        var service = CreateService(handler, string.Empty);

        var result = await service.SendAsync(
            "owner@gmail.com",
            new UploadCompletedRequest("project-media", "operations/file-1", "plan.pdf", "application/pdf", 2048));

        Assert.False(result.Sent);
        Assert.Equal(0, handler.CallCount);
    }

    private static UploadNotificationService CreateService(HttpMessageHandler handler, string apiKey, string recipientEmail = "") =>
        new(
            new HttpClient(handler) { BaseAddress = new Uri("https://api.resend.com/") },
            Options.Create(new UploadNotificationOptions { ResendApiKey = apiKey, RecipientEmail = recipientEmail }),
            NullLogger<UploadNotificationService>.Instance);
}
