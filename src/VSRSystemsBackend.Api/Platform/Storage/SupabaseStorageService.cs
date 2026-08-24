using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace VSRSystemsBackend.Api.Platform.Storage;

public sealed partial class SupabaseStorageService
{
    private const int SignedUploadLifetimeSeconds = 7_200;
    private readonly HttpClient _httpClient;
    private readonly SupabaseStorageOptions _options;

    public SupabaseStorageService(HttpClient httpClient, IOptions<SupabaseStorageOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<SignedUploadResponse> CreateSignedUploadAsync(
        SignedUploadRequest request,
        CancellationToken cancellationToken = default)
    {
        Validate(request);
        var serviceUri = GetServiceUri();

        var encodedPath = string.Join('/', request.Path.Split('/').Select(Uri.EscapeDataString));
        var endpoint = $"{_options.Url.TrimEnd('/')}/storage/v1/object/upload/sign/{Uri.EscapeDataString(request.Bucket)}/{encodedPath}";
        using var providerRequest = CreateProviderRequest(HttpMethod.Post, endpoint, JsonContent.Create(new { }));
        using var response = await SendAsync(
            providerRequest,
            "Supabase Storage could not create a signed upload URL.",
            cancellationToken);

        var signedUrl = await ReadSignedUrlAsync(response, serviceUri, "url", cancellationToken);
        return new SignedUploadResponse(
            request.Bucket,
            request.Path,
            request.ContentType,
            signedUrl,
            SignedUploadLifetimeSeconds);
    }

    public async Task<SignedDownloadResponse> CreateSignedDownloadAsync(
        StorageObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateObject(request.Bucket, request.Path);
        var serviceUri = GetServiceUri();
        var expiresIn = Math.Clamp(_options.SignedDownloadSeconds, 60, 86_400);
        var encodedPath = string.Join('/', request.Path.Split('/').Select(Uri.EscapeDataString));
        var endpoint = $"{_options.Url.TrimEnd('/')}/storage/v1/object/sign/{Uri.EscapeDataString(request.Bucket)}/{encodedPath}";
        using var providerRequest = CreateProviderRequest(
            HttpMethod.Post,
            endpoint,
            JsonContent.Create(new { expiresIn }));
        using var response = await SendAsync(
            providerRequest,
            "Supabase Storage could not create a signed download URL.",
            cancellationToken);

        var signedUrl = await ReadSignedUrlAsync(response, serviceUri, "signedURL", cancellationToken);
        return new SignedDownloadResponse(
            request.Bucket,
            request.Path,
            signedUrl,
            expiresIn);
    }

    public async Task<DeleteObjectResponse> DeleteObjectAsync(
        StorageObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateObject(request.Bucket, request.Path);
        _ = GetServiceUri();
        var endpoint = $"{_options.Url.TrimEnd('/')}/storage/v1/object/{Uri.EscapeDataString(request.Bucket)}";
        using var providerRequest = CreateProviderRequest(
            HttpMethod.Delete,
            endpoint,
            JsonContent.Create(new { prefixes = new[] { request.Path } }));
        using var response = await SendAsync(
            providerRequest,
            "Supabase Storage could not delete the object.",
            cancellationToken);

        return new DeleteObjectResponse(request.Bucket, request.Path, true);
    }

    public void Validate(SignedUploadRequest request)
    {
        ValidateObject(request.Bucket, request.Path);
        if (string.IsNullOrWhiteSpace(request.ContentType)
            || !_options.AllowedContentTypes.Contains(request.ContentType, StringComparer.OrdinalIgnoreCase))
            throw new StorageValidationException("Content type is not allowed.");
    }

    public void ValidateObject(string bucket, string path)
    {
        if (string.IsNullOrWhiteSpace(bucket)
            || !SafeBucketName().IsMatch(bucket)
            || !_options.AllowedBuckets.Contains(bucket, StringComparer.Ordinal))
            throw new StorageValidationException("Bucket is not allowed.");

        if (string.IsNullOrWhiteSpace(path)
            || path.Length > Math.Clamp(_options.MaxPathLength, 1, 2_048)
            || path.StartsWith('/')
            || path.EndsWith('/')
            || path.Contains('\\')
            || path.Contains("//", StringComparison.Ordinal)
            || path.Contains('?')
            || path.Contains('#'))
            throw new StorageValidationException("Object path is invalid.");

        var segments = path.Split('/');
        if (segments.Any(segment => segment is "." or ".." || !SafePathSegment().IsMatch(segment)))
            throw new StorageValidationException("Object path is invalid.");
    }

    private async Task<string> ReadSignedUrlAsync(
        HttpResponseMessage response,
        Uri serviceUri,
        string propertyName,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            var url = document.RootElement.GetProperty(propertyName).GetString();
            if (string.IsNullOrWhiteSpace(url))
                throw new JsonException("Missing signed URL.");

            var signedUri = BuildSignedUri(serviceUri, url);
            if (signedUri.Scheme != Uri.UriSchemeHttps
                || !string.Equals(signedUri.Host, serviceUri.Host, StringComparison.OrdinalIgnoreCase))
                throw new JsonException("Invalid signed URL origin.");
            return signedUri.AbsoluteUri;
        }
        catch (JsonException exception)
        {
            throw new StorageProviderException("Supabase Storage returned an invalid signing response.", exception);
        }
        catch (Exception exception) when (exception is InvalidOperationException or KeyNotFoundException)
        {
            throw new StorageProviderException("Supabase Storage returned an invalid signing response.", exception);
        }
    }

    private Uri GetServiceUri()
    {
        if (string.IsNullOrWhiteSpace(_options.Url)
            || string.IsNullOrWhiteSpace(_options.ServiceRoleKey)
            || !Uri.TryCreate(_options.Url, UriKind.Absolute, out var serviceUri)
            || serviceUri.Scheme != Uri.UriSchemeHttps)
            throw new StorageNotConfiguredException();
        return serviceUri;
    }

    private HttpRequestMessage CreateProviderRequest(HttpMethod method, string endpoint, HttpContent content)
    {
        var request = new HttpRequestMessage(method, endpoint) { Content = content };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ServiceRoleKey);
        request.Headers.Add("apikey", _options.ServiceRoleKey);
        return request;
    }

    private async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        string failureMessage,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            if (response.IsSuccessStatusCode)
                return response;

            response.Dispose();
            throw new StorageProviderException(failureMessage);
        }
        catch (StorageProviderException)
        {
            throw;
        }
        catch (HttpRequestException exception)
        {
            throw new StorageProviderException("Supabase Storage could not be reached.", exception);
        }
        catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            throw new StorageProviderException("Supabase Storage timed out.", exception);
        }
    }

    private static Uri BuildSignedUri(Uri serviceUri, string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var absolute))
            return absolute;

        var origin = serviceUri.GetLeftPart(UriPartial.Authority);
        if (url.StartsWith("/storage/v1/", StringComparison.Ordinal))
            return new Uri(origin + url);
        if (url.StartsWith("/object/", StringComparison.Ordinal))
            return new Uri(origin + "/storage/v1" + url);
        return new Uri(origin + "/storage/v1/" + url.TrimStart('/'));
    }

    [GeneratedRegex("^[A-Za-z0-9][A-Za-z0-9._-]{0,127}$", RegexOptions.CultureInvariant)]
    private static partial Regex SafePathSegment();

    [GeneratedRegex("^[a-z0-9][a-z0-9._-]{0,62}$", RegexOptions.CultureInvariant)]
    private static partial Regex SafeBucketName();
}
