using Npgsql;

namespace VSRSystemsBackend.Api.Infrastructure.Configuration;

public static class DatabaseConnectionString
{
    public static string Resolve(IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");
        if (HasHost(defaultConnection))
        {
            return defaultConnection!;
        }

        var databaseUrl = configuration["DATABASE_URL"];
        if (TryConvertDatabaseUrl(databaseUrl, out var connectionString))
        {
            return connectionString;
        }

        throw new InvalidOperationException(
            "Database configuration is invalid. Set ConnectionStrings__DefaultConnection " +
            "to a PostgreSQL connection string with a Host, or set DATABASE_URL to a valid PostgreSQL URL.");
    }

    private static bool HasHost(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return false;
        }

        try
        {
            return !string.IsNullOrWhiteSpace(new NpgsqlConnectionStringBuilder(connectionString).Host);
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static bool TryConvertDatabaseUrl(string? databaseUrl, out string connectionString)
    {
        connectionString = string.Empty;
        if (!Uri.TryCreate(databaseUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != "postgres" && uri.Scheme != "postgresql") ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            return false;
        }

        var credentials = uri.UserInfo.Split(':', 2);
        if (credentials.Length != 2)
        {
            return false;
        }

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
            Username = Uri.UnescapeDataString(credentials[0]),
            Password = Uri.UnescapeDataString(credentials[1])
        };

        foreach (var parameter in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var pair = parameter.Split('=', 2);
            if (pair.Length == 2 &&
                pair[0].Equals("sslmode", StringComparison.OrdinalIgnoreCase))
            {
                if (!TryParseSslMode(Uri.UnescapeDataString(pair[1]), out var sslMode))
                {
                    return false;
                }

                builder.SslMode = sslMode;
            }
        }

        connectionString = builder.ConnectionString;
        return true;
    }

    private static bool TryParseSslMode(string value, out SslMode sslMode)
    {
        switch (value.ToLowerInvariant())
        {
            case "disable":
                sslMode = SslMode.Disable;
                return true;
            case "allow":
                sslMode = SslMode.Allow;
                return true;
            case "prefer":
                sslMode = SslMode.Prefer;
                return true;
            case "require":
                sslMode = SslMode.Require;
                return true;
            case "verify-ca":
                sslMode = SslMode.VerifyCA;
                return true;
            case "verify-full":
                sslMode = SslMode.VerifyFull;
                return true;
            default:
                sslMode = default;
                return false;
        }
    }
}
