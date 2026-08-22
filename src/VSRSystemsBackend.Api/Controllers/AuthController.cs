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
    private static readonly Dictionary<string, AuthResponseDto> _tokens = new();

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
        // Very simple lookup - accept any email/password for demo
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