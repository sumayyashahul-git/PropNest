using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropNest.AuthService.Models;
using PropNest.AuthService.Services;

namespace PropNest.AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        if (result is null)
            return Conflict(new { message = "Email already registered." });

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result is null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(idClaim, out var userId))
            return Unauthorized();

        var user = await _authService.GetUserByIdAsync(userId);

        if (user is null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email,
            user.Role,
            user.CreatedAt
        });
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "Auth Service is running", timestamp = DateTime.UtcNow });
    }
}