using System.Text.Json;
using Microsoft.Extensions.Options;
using VSRSystemsBackend.Api.Platform.Storage;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class SupabaseStorageServiceTests
{
    [Fact]
    public async Task CreatesSignedUploadWithoutExposingServiceKey()
    {
        const string serviceKey = "backend-service-role-secret";
        string? authorization = null;
        string? apiKey = null;
        Uri? requestUri = null;
        var handler = new PlatformTestHttpHandler((request, _) =>
        {
            authorization = request.Headers.Authorization?.Parameter;
            apiKey = request.Headers.GetValues("apikey").Single();
            requestUri = request.RequestUri;
            return Task.FromResult(PlatformTestHttpHandler.Json(
                """{"url":"/object/upload/sign/uploads/projects/project-1/photo.jpg?token=signed-token"}"""));
        });
        var service = CreateService(handler, serviceKey: serviceKey);

        var result = await service.CreateSignedUploadAsync(
            new SignedUploadRequest("uploads", "projects/project-1/photo.jpg", "image/jpeg", true));

        Assert.Equal(serviceKey, authorization);
        Assert.Equal(serviceKey, apiKey);
        Assert.Equal(
            "https://test.supabase.co/storage/v1/object/upload/sign/uploads/projects/project-1/photo.jpg",
            requestUri?.GetLeftPart(UriPartial.Path));
        Assert.Equal(
            "https://test.supabase.co/storage/v1/object/upload/sign/uploads/projects/project-1/photo.jpg?token=signed-token",
            result.SignedUrl);
        Assert.Equal(7_200, result.ExpiresInSeconds);
        Assert.DoesNotContain(serviceKey, JsonSerializer.Serialize(result));
    }

    [Fact]
    public async Task CreatesSignedDownloadWithoutExposingServiceKey()
    {
        const string serviceKey = "backend-service-role-secret";
        string? authorization = null;
        string? apiKey = null;
        string? requestBody = null;
        HttpMethod? method = null;
        Uri? requestUri = null;
        var handler = new PlatformTestHttpHandler(async (request, cancellationToken) =>
        {
            method = request.Method;
            requestUri = request.RequestUri;
            authorization = request.Headers.Authorization?.Parameter;
            apiKey = request.Headers.GetValues("apikey").Single();
            requestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return PlatformTestHttpHandler.Json(
                """{"signedURL":"/object/sign/uploads/projects/project-1/private.pdf?token=download-token"}""");
        });
        var service = CreateService(handler, serviceKey: serviceKey, signedDownloadSeconds: 900);

        var result = await service.CreateSignedDownloadAsync(
            new StorageObjectRequest("uploads", "projects/project-1/private.pdf"));

        Assert.Equal(HttpMethod.Post, method);
        Assert.Equal(
            "https://test.supabase.co/storage/v1/object/sign/uploads/projects/project-1/private.pdf",
            requestUri?.AbsoluteUri);
        Assert.Equal(900, JsonDocument.Parse(requestBody!).RootElement.GetProperty("expiresIn").GetInt32());
        Assert.Equal(serviceKey, authorization);
        Assert.Equal(serviceKey, apiKey);
        Assert.Equal(
            "https://test.supabase.co/storage/v1/object/sign/uploads/projects/project-1/private.pdf?token=download-token",
            result.SignedUrl);
        Assert.Equal(900, result.ExpiresInSeconds);
        Assert.DoesNotContain(serviceKey, JsonSerializer.Serialize(result));
    }

    [Fact]
    public async Task DeletesExactObjectWithoutExposingServiceKey()
    {
        const string serviceKey = "backend-service-role-secret";
        string? authorization = null;
        string? apiKey = null;
        string? requestBody = null;
        HttpMethod? method = null;
        Uri? requestUri = null;
        var handler = new PlatformTestHttpHandler(async (request, cancellationToken) =>
        {
            method = request.Method;
            requestUri = request.RequestUri;
            authorization = request.Headers.Authorization?.Parameter;
            apiKey = request.Headers.GetValues("apikey").Single();
            requestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return PlatformTestHttpHandler.Json("[]");
        });
        var service = CreateService(handler, serviceKey: serviceKey);

        var result = await service.DeleteObjectAsync(
            new StorageObjectRequest("uploads", "projects/project-1/private.pdf"));

        Assert.Equal(HttpMethod.Delete, method);
        Assert.Equal("https://test.supabase.co/storage/v1/object/uploads", requestUri?.AbsoluteUri);
        var prefixes = JsonDocument.Parse(requestBody!).RootElement.GetProperty("prefixes");
        Assert.Equal("projects/project-1/private.pdf", prefixes[0].GetString());
        Assert.Equal(1, prefixes.GetArrayLength());
        Assert.Equal(serviceKey, authorization);
        Assert.Equal(serviceKey, apiKey);
        Assert.True(result.Deleted);
        Assert.Equal("uploads", result.Bucket);
        Assert.Equal("projects/project-1/private.pdf", result.Path);
        Assert.DoesNotContain(serviceKey, JsonSerializer.Serialize(result));
    }

    [Theory]
    [InlineData("../secret.pdf")]
    [InlineData("projects//secret.pdf")]
    [InlineData("/projects/secret.pdf")]
    [InlineData("projects\\secret.pdf")]
    [InlineData("projects/secret.pdf?download=1")]
    [InlineData("projects/space name.pdf")]
    public async Task RejectsUnsafeObjectPaths(string path)
    {
        var handler = new PlatformTestHttpHandler((_, _) => throw new InvalidOperationException());
        var service = CreateService(handler);

        await Assert.ThrowsAsync<StorageValidationException>(() => service.CreateSignedUploadAsync(
            new SignedUploadRequest("uploads", path, "application/pdf")));

        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task RejectsBucketAndContentTypeOutsideAllowlists()
    {
        var handler = new PlatformTestHttpHandler((_, _) => throw new InvalidOperationException());
        var service = CreateService(handler);

        await Assert.ThrowsAsync<StorageValidationException>(() => service.CreateSignedUploadAsync(
            new SignedUploadRequest("private", "project/file.pdf", "application/pdf")));
        await Assert.ThrowsAsync<StorageValidationException>(() => service.CreateSignedUploadAsync(
            new SignedUploadRequest("uploads", "project/file.exe", "application/x-msdownload")));

        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task RejectsUploadWithoutCloudCostConfirmation()
    {
        var handler = new PlatformTestHttpHandler((_, _) => throw new InvalidOperationException());
        var service = CreateService(handler);

        var exception = await Assert.ThrowsAsync<StorageValidationException>(() => service.CreateSignedUploadAsync(
            new SignedUploadRequest("uploads", "project/file.pdf", "application/pdf")));

        Assert.Contains("confirmation", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task VerifiesUploadedObjectWithAuthenticatedHeadRequest()
    {
        HttpMethod? method = null;
        Uri? requestUri = null;
        var handler = new PlatformTestHttpHandler((request, _) =>
        {
            method = request.Method;
            requestUri = request.RequestUri;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK));
        });
        var service = CreateService(handler);

        await service.VerifyObjectExistsAsync(new StorageObjectRequest("uploads", "project/file.pdf"));

        Assert.Equal(HttpMethod.Head, method);
        Assert.Equal("https://test.supabase.co/storage/v1/object/uploads/project/file.pdf", requestUri?.AbsoluteUri);
    }

    [Fact]
    public async Task DownloadAndDeleteValidateBucketAndPathBeforeProviderCall()
    {
        var handler = new PlatformTestHttpHandler((_, _) => throw new InvalidOperationException());
        var service = CreateService(handler);

        await Assert.ThrowsAsync<StorageValidationException>(() => service.CreateSignedDownloadAsync(
            new StorageObjectRequest("private", "project/file.pdf")));
        await Assert.ThrowsAsync<StorageValidationException>(() => service.CreateSignedDownloadAsync(
            new StorageObjectRequest("uploads", "../file.pdf")));
        await Assert.ThrowsAsync<StorageValidationException>(() => service.DeleteObjectAsync(
            new StorageObjectRequest("private", "project/file.pdf")));
        await Assert.ThrowsAsync<StorageValidationException>(() => service.DeleteObjectAsync(
            new StorageObjectRequest("uploads", "project//file.pdf")));

        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task DoesNotCallProviderWithoutBackendConfiguration()
    {
        var handler = new PlatformTestHttpHandler((_, _) => throw new InvalidOperationException());
        var service = CreateService(handler, url: string.Empty, serviceKey: string.Empty);

        await Assert.ThrowsAsync<StorageNotConfiguredException>(() => service.CreateSignedUploadAsync(
            new SignedUploadRequest("uploads", "project/file.pdf", "application/pdf", true)));

        Assert.Equal(0, handler.CallCount);
    }

    private static SupabaseStorageService CreateService(
        HttpMessageHandler handler,
        string url = "https://test.supabase.co",
        string serviceKey = "test-service-key",
        int signedDownloadSeconds = 3_600) =>
        new(
            new HttpClient(handler),
            Options.Create(new SupabaseStorageOptions
            {
                Url = url,
                ServiceRoleKey = serviceKey,
                AllowedBuckets = ["uploads"],
                AllowedContentTypes = ["image/jpeg", "application/pdf"],
                SignedDownloadSeconds = signedDownloadSeconds
            }));
}
