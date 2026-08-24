using MongoDB.Driver;

namespace VSRSystemsBackend.Infrastructure.Persistence.Mongo;

public sealed class MongoDbContext
{
    public MongoDbContext(
        MongoDbOptions options,
        IMongoClient? client,
        IMongoDatabase? database,
        string? configurationError = null)
    {
        Options = options;
        Client = client;
        Database = database;
        ConfigurationError = configurationError;
    }

    public MongoDbOptions Options { get; }
    public IMongoClient? Client { get; }
    public IMongoDatabase? Database { get; }
    public string? ConfigurationError { get; }
    public bool IsConfigured => Database is not null;

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        if (Database is null)
            throw new InvalidOperationException("MongoDB is not configured.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A collection name is required.", nameof(name));

        return Database.GetCollection<T>(name);
    }
}
