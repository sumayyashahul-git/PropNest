using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PropNest.AuthService.Data;
using PropNest.AuthService.Models;

namespace PropNest.AuthService.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<User?> GetUserByIdAsync(Guid id);
}

public class JwtAuthService : IAuthService
{
    private readonly AuthDbContext _db;
    private readonly IConfiguration _config;

    public JwtAuthService(AuthDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var emailExists = await _db.Users
            .AnyAsync(u => u.Email == request.Email);

        if (emailExists)
            return null;

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return GenerateToken(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email 
                                   && u.IsActive);

        if (user is null)
            return null;

        var passwordCorrect = BCrypt.Net.BCrypt
            .Verify(request.Password, user.PasswordHash);

        if (!passwordCorrect)
            return null;

        return GenerateToken(user);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _db.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    private AuthResponse GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddHours(8);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new AuthResponse(
            Token: new JwtSecurityTokenHandler().WriteToken(token),
            FullName: user.FullName,
            Email: user.Email,
            Role: user.Role.ToString(),
            ExpiresAt: expires
        );
    }
}