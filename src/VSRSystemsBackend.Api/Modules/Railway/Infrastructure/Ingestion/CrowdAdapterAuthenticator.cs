using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;

public interface ICrowdSourceSecretProtector
{
    string Protect(string secret);
    string Unprotect(string ciphertext);
}

public sealed class CrowdSourceSecretProtector(IDataProtectionProvider provider) : ICrowdSourceSecretProtector
{
    private readonly IDataProtector protector = provider.CreateProtector("railway.crowd-source-signing.v1");
    public string Protect(string secret) => protector.Protect(secret);
    public string Unprotect(string ciphertext) => protector.Unprotect(ciphertext);
}

public sealed record CrowdAuthenticationResult(bool Succeeded, string? FailureCode = null);

public sealed class CrowdAdapterAuthenticator(ICrowdSourceSecretProtector secretProtector)
{
    private static readonly TimeSpan MaximumClockSkew = TimeSpan.FromMinutes(5);

    public CrowdAuthenticationResult Authenticate(CrowdSource source, DateTimeOffset timestamp, string nonce,
        string signature, ReadOnlySpan<byte> body, DateTimeOffset now)
    {
        if (!source.Enabled) return new(false, "source_disabled");
        if (string.IsNullOrWhiteSpace(nonce) || nonce.Length > 160) return new(false, "invalid_nonce");
        if ((now - timestamp).Duration() > MaximumClockSkew) return new(false, "expired_timestamp");

        var bodyDigest = Convert.ToHexString(SHA256.HashData(body)).ToLowerInvariant();
        var signedMessage = Encoding.UTF8.GetBytes($"{timestamp.ToUnixTimeSeconds()}.{nonce}.{bodyDigest}");
        if (Matches(source.SigningSecretCiphertext, signature, signedMessage)) return new(true);
        if (source.PreviousSigningSecretCiphertext is not null && source.PreviousSecretValidUntil >= now &&
            Matches(source.PreviousSigningSecretCiphertext, signature, signedMessage)) return new(true);
        return new(false, "invalid_signature");
    }

    private bool Matches(string ciphertext, string signature, byte[] signedMessage)
    {
        byte[] supplied;
        try { supplied = Convert.FromBase64String(signature); }
        catch (FormatException) { return false; }
        var secret = Encoding.UTF8.GetBytes(secretProtector.Unprotect(ciphertext));
        var expected = HMACSHA256.HashData(secret, signedMessage);
        return supplied.Length == expected.Length && CryptographicOperations.FixedTimeEquals(supplied, expected);
    }
}
