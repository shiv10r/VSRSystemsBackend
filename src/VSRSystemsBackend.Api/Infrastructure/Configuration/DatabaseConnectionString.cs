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

        databaseUrl = configuration["SUPABASE_CONNECTION_STRING"];
        if (TryConvertDatabaseUrl(databaseUrl, out connectionString))
        {
            return connectionString;
        }

        throw new InvalidOperationException(
            "Database configuration is invalid. Set ConnectionStrings__DefaultConnection " +
            "to a PostgreSQL connection string with a Host, or set SUPABASE_CONNECTION_STRING " +
            "or DATABASE_URL to a valid PostgreSQL URL.");
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
        if (!TryCreateDatabaseUri(databaseUrl, out var uri) ||
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

    private static bool TryCreateDatabaseUri(string? databaseUrl, out Uri uri)
    {
        uri = null!;
        if (string.IsNullOrWhiteSpace(databaseUrl))
        {
            return false;
        }

        var schemeEnd = databaseUrl.IndexOf("://", StringComparison.Ordinal);
        var userInfoStart = schemeEnd + 3;
        var pathStart = databaseUrl.IndexOf('/', userInfoStart);
        var queryStart = databaseUrl.IndexOf('?', userInfoStart);
        var authorityEnd = new[] { pathStart, queryStart }
            .Where(index => index >= 0)
            .DefaultIfEmpty(databaseUrl.Length)
            .Min();
        var userInfoEnd = databaseUrl[..authorityEnd].LastIndexOf('@');
        var passwordSeparator = databaseUrl.IndexOf(':', userInfoStart);
        if (schemeEnd <= 0 || passwordSeparator < userInfoStart || userInfoEnd <= passwordSeparator)
        {
            return false;
        }

        var scheme = databaseUrl[..schemeEnd];
        if (!scheme.Equals("postgres", StringComparison.OrdinalIgnoreCase) &&
            !scheme.Equals("postgresql", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var username = databaseUrl[userInfoStart..passwordSeparator];
        var password = databaseUrl[(passwordSeparator + 1)..userInfoEnd];
        var normalizedUrl = $"{scheme}://{Uri.EscapeDataString(Uri.UnescapeDataString(username))}:" +
                            $"{Uri.EscapeDataString(Uri.UnescapeDataString(password))}{databaseUrl[userInfoEnd..]}";

        return Uri.TryCreate(normalizedUrl, UriKind.Absolute, out uri!);
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
