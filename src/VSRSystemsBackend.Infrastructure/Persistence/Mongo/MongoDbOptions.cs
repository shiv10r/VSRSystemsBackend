namespace VSRSystemsBackend.Infrastructure.Persistence.Mongo;

public sealed class MongoDbOptions
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = "vsr_systems";

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ConnectionString)
        && !string.IsNullOrWhiteSpace(DatabaseName);
}
