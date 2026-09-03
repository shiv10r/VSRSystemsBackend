using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using VSRSystemsBackend.Application.HomeServices.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IDistributedCache _cache;
    private const string TokenKeyPrefix = "auth:token:";
    private const string AdminEmail = "admin.portal@vsrsystems.com";
    private const string AdminPassword = "nfeuTYjb7CEAnoK7EV";

    public AuthController(IDistributedCache cache)
    {
        _cache = cache;
    }

    private static string TokenKey(string token) => $"{TokenKeyPrefix}{token}";

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var token = Guid.NewGuid().ToString("N");
        var user = new AuthResponseDto
        {
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            User = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                Email = dto.Email,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Roles = new List<string> { "customer" }
            }
        };
        await StoreTokenAsync(token, user);
        return Ok(new { token, username = dto.FullName, role = "customer" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        if (dto.Email != AdminEmail || dto.Password != AdminPassword)
            return Unauthorized(new { message = "Invalid email or password." });

        var token = Guid.NewGuid().ToString("N");
        var user = new AuthResponseDto
        {
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            User = new UserDto
            {
                Id = "admin-demo",
                Email = dto.Email,
                FullName = "Admin",
                Phone = string.Empty,
                Roles = new List<string> { "admin" }
            }
        };
        await StoreTokenAsync(token, user);
        return Ok(new { token, username = "Admin", role = "admin" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return Unauthorized();

        var token = authHeader.Substring("Bearer ".Length);
        var user = await GetTokenAsync(token);
        if (user is null)
            return Unauthorized();

        return Ok(user);
    }

    private async Task StoreTokenAsync(string token, AuthResponseDto user)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = user.ExpiresAtUtc
        };
        await _cache.SetStringAsync(TokenKey(token), JsonSerializer.Serialize(user), options);
    }

    private async Task<AuthResponseDto?> GetTokenAsync(string token)
    {
        var json = await _cache.GetStringAsync(TokenKey(token));
        if (string.IsNullOrEmpty(json))
            return null;

        return JsonSerializer.Deserialize<AuthResponseDto>(json);
    }
}
