using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace VSRSystemsBackend.Api.Platform.Storage;

public sealed record UploadNotificationResult(bool Sent, string Message);

public sealed class UploadNotificationService
{
    private readonly HttpClient _httpClient;
    private readonly UploadNotificationOptions _options;
    private readonly ILogger<UploadNotificationService> _logger;

    public UploadNotificationService(
        HttpClient httpClient,
        IOptions<UploadNotificationOptions> options,
        ILogger<UploadNotificationService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<UploadNotificationResult> SendAsync(
        string accountEmail,
        UploadCompletedRequest upload,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ResendApiKey))
            return new(false, "Upload completed, but email notifications are not configured.");

        var recipient = string.IsNullOrWhiteSpace(_options.RecipientEmail)
            ? accountEmail
            : _options.RecipientEmail.Trim();
        var safeFileName = WebUtility.HtmlEncode(upload.FileName);
        var safeBucket = WebUtility.HtmlEncode(upload.Bucket);
        using var request = new HttpRequestMessage(HttpMethod.Post, "emails");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ResendApiKey);
        request.Content = JsonContent.Create(new
        {
            from = _options.FromEmail,
            to = new[] { recipient },
            subject = $"VSR Systems upload completed: {upload.FileName}",
            html = $"<h2>Upload completed</h2><p><strong>{safeFileName}</strong> was uploaded successfully to private storage.</p><p>Bucket: {safeBucket}<br>Size: {FormatBytes(upload.SizeBytes)}<br>Time: {DateTimeOffset.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>"
        });

        try
        {
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
                return new(true, $"Upload completed and a confirmation email was sent to {MaskEmail(recipient)}.");

            _logger.LogWarning("Upload email provider returned status {StatusCode}", (int)response.StatusCode);
            return new(false, "Upload completed, but the confirmation email could not be sent.");
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
        {
            _logger.LogWarning(exception, "Upload confirmation email delivery failed");
            return new(false, "Upload completed, but the confirmation email could not be sent.");
        }
    }

    private static string FormatBytes(long value) =>
        value < 1_048_576 ? $"{Math.Max(1, value / 1024)} KB" : $"{value / 1_048_576d:F1} MB";

    private static string MaskEmail(string value)
    {
        var separator = value.IndexOf('@');
        return separator <= 1 ? "your account email" : $"{value[0]}***{value[separator..]}";
    }
}
