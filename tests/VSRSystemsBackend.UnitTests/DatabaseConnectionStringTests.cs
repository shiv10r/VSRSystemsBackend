using Microsoft.Extensions.Configuration;
using Npgsql;
using VSRSystemsBackend.Api.Infrastructure.Configuration;
using FluentAssertions;
using Xunit;

namespace VSRSystemsBackend.UnitTests;

public class DatabaseConnectionStringTests
{
    [Fact]
    public void Resolve_UsesValidDefaultConnection()
    {
        const string expected = "Host=db.example.com;Port=5432;Database=vsr;Username=app;Password=secret";
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = expected,
            ["DATABASE_URL"] = "postgresql://fallback:unused@fallback.example.com:5432/fallback"
        });

        var connectionString = DatabaseConnectionString.Resolve(configuration);

        connectionString.Should().Be(expected);
    }

    [Fact]
    public void Resolve_UsesDatabaseUrlWhenDefaultConnectionHasNoHost()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=;Database=postgres;Username=broken;Password=broken",
            ["DATABASE_URL"] = "postgresql://render%40user:p%40ssword@render-db.example.com:5433/vsr_prod?sslmode=require"
        });

        var connectionString = DatabaseConnectionString.Resolve(configuration);
        var parsed = new NpgsqlConnectionStringBuilder(connectionString);

        parsed.Host.Should().Be("render-db.example.com");
        parsed.Port.Should().Be(5433);
        parsed.Database.Should().Be("vsr_prod");
        parsed.Username.Should().Be("render@user");
        parsed.Password.Should().Be("p@ssword");
        parsed.SslMode.Should().Be(SslMode.Require);
    }

    [Fact]
    public void Resolve_RejectsConfigurationWithoutAUsableDatabaseHost()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=;Database=postgres;Username=broken;Password=broken"
        });

        var action = () => DatabaseConnectionString.Resolve(configuration);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*ConnectionStrings__DefaultConnection*DATABASE_URL*");
    }

    [Theory]
    [InlineData("verify-ca", SslMode.VerifyCA)]
    [InlineData("verify-full", SslMode.VerifyFull)]
    public void Resolve_MapsStandardPostgresSslModes(string value, SslMode expected)
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["DATABASE_URL"] = $"postgres://user:secret@db.example.com/vsr?sslmode={value}"
        });

        var connectionString = DatabaseConnectionString.Resolve(configuration);

        new NpgsqlConnectionStringBuilder(connectionString).SslMode.Should().Be(expected);
    }

    [Fact]
    public void Resolve_RejectsAnUnknownDatabaseUrlSslMode()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["DATABASE_URL"] = "postgres://user:secret@db.example.com/vsr?sslmode=unknown"
        });

        var action = () => DatabaseConnectionString.Resolve(configuration);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*ConnectionStrings__DefaultConnection*DATABASE_URL*");
    }

    private static IConfiguration BuildConfiguration(Dictionary<string, string?> values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values).Build();
}
