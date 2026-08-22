using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.HomeServices.DTOs;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    // In-memory store for demo (replace with real DB logic later)
    private static readonly Dictionary<string, AuthResponseDto> _tokens = new()
    {
        // Seeded admin credentials from old system
        ["admin"] = new AuthResponseDto
        {
            Token = "admin-token-demo",
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            User = new UserDto
            {
                Id = "admin-demo",
                Email = "admin@vsrsystems.com",
                FullName = "Admin",
                Phone = "",
                Roles = new List<string> { "admin" }
            }
        }
    };

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequestDto dto)
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
        _tokens[token] = user;
        return Ok(new { token, username = dto.FullName, role = "customer" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto dto)
    {
        // Simple demo authentication - accept admin credentials or any email/password
        string fullName;
        if (dto.Email == "admin@vsrsystems.com" && dto.Password == "admin123")
        {
            fullName = "Admin";
        }
        else
        {
            // For demo: accept any email/password
            fullName = dto.FullName ?? dto.Email;
        }

        var token = Guid.NewGuid().ToString("N");
        var user = new AuthResponseDto
        {
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            User = new UserDto
            {
                Id = "admin-demo",
                Email = dto.Email,
                FullName = fullName,
                Phone = dto.Phone,
                Roles = new List<string> { "admin" }
            }
        };
        _tokens[token] = user;
        return Ok(new { token, username = fullName, role = "admin" });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return Unauthorized();

        var token = authHeader.Substring("Bearer ".Length);
        if (_tokens.TryGetValue(token, out var user))
            return Ok(user);

        return Unauthorized();
    }
}