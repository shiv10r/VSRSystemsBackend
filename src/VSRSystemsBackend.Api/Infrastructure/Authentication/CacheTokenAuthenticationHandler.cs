using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace VSRSystemsBackend.Api.Infrastructure.Authentication;

public sealed class CacheTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "CacheToken";
    private const string TokenKeyPrefix = "auth:token:";
    private readonly IDistributedCache _cache;

    public CacheTokenAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IDistributedCache cache)
        : base(options, logger, encoder)
    {
        _cache = cache;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();
        var token = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? authorization["Bearer ".Length..].Trim()
            : string.Empty;

        // Browsers cannot set an Authorization header for WebSocket and SSE handshakes.
        if (string.IsNullOrEmpty(token) && Request.Path.StartsWithSegments("/hubs"))
            token = Request.Query["access_token"].FirstOrDefault() ?? string.Empty;

        if (string.IsNullOrEmpty(token))
            return AuthenticateResult.NoResult();

        var sessionJson = await _cache.GetStringAsync(TokenKeyPrefix + token, Context.RequestAborted);
        if (string.IsNullOrEmpty(sessionJson))
            return AuthenticateResult.Fail("The bearer token is invalid or expired.");

        try
        {
            using var session = JsonDocument.Parse(sessionJson);
            var user = session.RootElement.GetProperty("User");
            var name = GetString(user, "FullName") ?? GetString(user, "Email") ?? "user";
            var claims = new List<Claim> { new(ClaimTypes.Name, name) };
            var userId = GetString(user, "Id");
            if (!string.IsNullOrEmpty(userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                claims.Add(new Claim(VSRSystemsBackend.Shared.Constants.AppConstants.ClaimTypes.UserId, userId));
            }

            var email = GetString(user, "Email");
            if (!string.IsNullOrEmpty(email))
                claims.Add(new Claim(ClaimTypes.Email, email));

            if (user.TryGetProperty("Roles", out var roles) && roles.ValueKind == JsonValueKind.Array)
            {
                claims.AddRange(roles.EnumerateArray()
                    .Where(role => role.ValueKind == JsonValueKind.String)
                    .Select(role => new Claim(ClaimTypes.Role, role.GetString()!)));
            }

            var identity = new ClaimsIdentity(claims, SchemeName);
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName));
        }
        catch (JsonException)
        {
            return AuthenticateResult.Fail("The bearer session is invalid.");
        }
        catch (KeyNotFoundException)
        {
            return AuthenticateResult.Fail("The bearer session is invalid.");
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }

    private static string? GetString(JsonElement element, string propertyName) =>
        element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;
}
