using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace VSRSystemsBackend.Api.Platform.AI;

public sealed class AiGatewayService
{
    public const string FallbackText = "AI is temporarily unavailable. Please try again later.";
    private const string FallbackModel = "safe-fallback";
    private readonly HttpClient _httpClient;
    private readonly AiGatewayOptions _options;
    private readonly ILogger<AiGatewayService> _logger;

    public AiGatewayService(
        HttpClient httpClient,
        IOptions<AiGatewayOptions> options,
        ILogger<AiGatewayService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public AiStatus GetStatus()
    {
        var provider = ConfiguredProviders().FirstOrDefault();
        return provider is null
            ? new AiStatus(false, FallbackModel)
            : new AiStatus(true, provider.Model);
    }

    public async Task<AiReply> ChatAsync(
        string text,
        IReadOnlyList<AiChatTurn> history,
        CancellationToken cancellationToken = default)
    {
        var providers = ConfiguredProviders().ToArray();
        foreach (var provider in providers)
        {
            using var request = CreateRequest(provider, text, history);
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromSeconds(Math.Clamp(_options.TimeoutSeconds, 1, 120)));

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead,
                    timeout.Token);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogWarning(exception, "AI provider {Provider} could not be reached", provider.Name);
                continue;
            }
            catch (OperationCanceledException exception) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning(exception, "AI provider {Provider} timed out", provider.Name);
                continue;
            }

            using (response)
            {
                if (response.StatusCode == HttpStatusCode.TooManyRequests
                    || (int)response.StatusCode >= 500)
                {
                    _logger.LogWarning(
                        "AI provider {Provider} returned transient HTTP {StatusCode}",
                        provider.Name,
                        (int)response.StatusCode);
                    continue;
                }

                if (!response.IsSuccessStatusCode)
                    throw new AiProviderRejectedException(provider.Model, "The configured AI provider rejected the request.");

                try
                {
                    await using var stream = await response.Content.ReadAsStreamAsync(timeout.Token);
                    using var document = await JsonDocument.ParseAsync(stream, cancellationToken: timeout.Token);
                    var root = document.RootElement;
                    var content = root.GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();
                    if (string.IsNullOrWhiteSpace(content))
                        throw new JsonException("The AI response did not contain message content.");

                    var tokens = root.TryGetProperty("usage", out var usage)
                        && usage.TryGetProperty("total_tokens", out var totalTokens)
                        && totalTokens.TryGetInt32(out var tokenCount)
                            ? tokenCount
                            : 0;
                    var model = root.TryGetProperty("model", out var responseModel)
                        ? responseModel.GetString() ?? provider.Model
                        : provider.Model;
                    return new AiReply(true, true, model, content, tokens);
                }
                catch (JsonException exception)
                {
                    throw new AiProviderRejectedException(
                        provider.Model,
                        "The configured AI provider returned an invalid response.",
                        exception);
                }
                catch (Exception exception) when (exception is InvalidOperationException
                    or KeyNotFoundException
                    or IndexOutOfRangeException)
                {
                    throw new AiProviderRejectedException(
                        provider.Model,
                        "The configured AI provider returned an invalid response.",
                        exception);
                }
                catch (HttpRequestException exception)
                {
                    _logger.LogWarning(exception, "AI provider {Provider} response was interrupted", provider.Name);
                    continue;
                }
                catch (OperationCanceledException exception) when (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning(exception, "AI provider {Provider} timed out", provider.Name);
                    continue;
                }
            }
        }

        return new AiReply(true, providers.Length > 0, FallbackModel, FallbackText, 0);
    }

    private HttpRequestMessage CreateRequest(
        AiProviderOptions provider,
        string text,
        IReadOnlyList<AiChatTurn> history)
    {
        var messages = new List<object>
        {
            new { role = "system", content = _options.SystemPrompt }
        };
        messages.AddRange(history.Select(turn => (object)new
        {
            role = turn.Role.ToLowerInvariant(),
            content = turn.Content
        }));
        messages.Add(new { role = "user", content = text });

        var request = new HttpRequestMessage(HttpMethod.Post, provider.Endpoint)
        {
            Content = JsonContent.Create(new
            {
                model = provider.Model,
                messages,
                temperature = 0.2
            })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.ApiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return request;
    }

    private IEnumerable<AiProviderOptions> ConfiguredProviders() =>
        _options.Providers.Where(provider =>
            !string.IsNullOrWhiteSpace(provider.ApiKey)
            && !string.IsNullOrWhiteSpace(provider.Model)
            && Uri.TryCreate(provider.Endpoint, UriKind.Absolute, out var endpoint)
            && endpoint.Scheme == Uri.UriSchemeHttps);
}
