namespace VSRSystemsBackend.Api.Platform.AI;

public sealed class AiGatewayOptions
{
    public const string SectionName = "AI";

    public int TimeoutSeconds { get; set; } = 20;
    public string SystemPrompt { get; set; } = "You are a concise, helpful assistant for VSR Systems users.";
    public List<AiProviderOptions> Providers { get; set; } = [];
}

public sealed class AiProviderOptions
{
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}
