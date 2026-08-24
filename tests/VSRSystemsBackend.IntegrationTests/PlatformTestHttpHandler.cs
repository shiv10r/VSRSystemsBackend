namespace VSRSystemsBackend.IntegrationTests;

internal sealed class PlatformTestHttpHandler(
    Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responseFactory)
    : HttpMessageHandler
{
    public int CallCount { get; private set; }
    public List<HttpRequestMessage> Requests { get; } = [];

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        CallCount++;
        Requests.Add(request);
        return responseFactory(request, cancellationToken);
    }

    public static HttpResponseMessage Json(string json, System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK) =>
        new(statusCode)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
}
