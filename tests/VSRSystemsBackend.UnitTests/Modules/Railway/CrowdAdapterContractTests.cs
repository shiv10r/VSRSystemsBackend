using System.Security.Cryptography;
using System.Text;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;
using Xunit;

namespace VSRSystemsBackend.UnitTests.Modules.Railway;

public sealed class CrowdAdapterContractTests
{
    private static readonly byte[] Body = "{\"observations\":[]}"u8.ToArray();

    [Fact]
    public void Valid_signature_is_accepted()
    {
        var now = DateTimeOffset.UtcNow;
        var source = CreateSource("current");

        var result = CreateAuthenticator().Authenticate(source, now, "nonce-1", Sign("current", now, "nonce-1"), Body, now);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Invalid_signature_and_expired_timestamp_are_rejected()
    {
        var now = DateTimeOffset.UtcNow;
        var source = CreateSource("current");
        var authenticator = CreateAuthenticator();

        Assert.Equal("invalid_signature", authenticator.Authenticate(source, now, "nonce-1", Sign("wrong", now, "nonce-1"), Body, now).FailureCode);
        Assert.Equal("expired_timestamp", authenticator.Authenticate(source, now.AddMinutes(-6), "nonce-1", Sign("current", now.AddMinutes(-6), "nonce-1"), Body, now).FailureCode);
    }

    [Fact]
    public void Previous_secret_is_accepted_only_during_rotation_overlap()
    {
        var now = DateTimeOffset.UtcNow;
        var source = CreateSource("old");
        source.RotateSigningSecret("new", now.AddMinutes(5));
        var authenticator = CreateAuthenticator();

        Assert.True(authenticator.Authenticate(source, now, "nonce-1", Sign("old", now, "nonce-1"), Body, now).Succeeded);
        Assert.False(authenticator.Authenticate(source, now.AddMinutes(6), "nonce-2", Sign("old", now.AddMinutes(6), "nonce-2"), Body, now.AddMinutes(6)).Succeeded);
    }

    private static CrowdSource CreateSource(string secret) =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Gate", "manual-json", secret);

    private static CrowdAdapterAuthenticator CreateAuthenticator() => new(new IdentityProtector());

    private static string Sign(string secret, DateTimeOffset timestamp, string nonce)
    {
        var digest = Convert.ToHexString(SHA256.HashData(Body)).ToLowerInvariant();
        var message = Encoding.UTF8.GetBytes($"{timestamp.ToUnixTimeSeconds()}.{nonce}.{digest}");
        return Convert.ToBase64String(HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), message));
    }

    private sealed class IdentityProtector : ICrowdSourceSecretProtector
    {
        public string Protect(string secret) => secret;
        public string Unprotect(string ciphertext) => ciphertext;
    }
}
