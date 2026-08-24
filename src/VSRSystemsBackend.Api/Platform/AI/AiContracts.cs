namespace VSRSystemsBackend.Api.Platform.AI;

public sealed record AiChatTurn(string Role, string Content);

public sealed record AiChatRequest(string Text, IReadOnlyList<AiChatTurn>? History);

public sealed record AiStatus(bool Configured, string Model);

public sealed record AiReply(
    bool Ok,
    bool Configured,
    string Model,
    string Text,
    int Tokens,
    string? Error = null);

public sealed class AiProviderRejectedException : Exception
{
    public AiProviderRejectedException(string model, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        Model = model;
    }

    public string Model { get; }
}
