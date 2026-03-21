using System.ComponentModel.DataAnnotations;

namespace PropNest.AuthService.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Customer;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum UserRole 
{ 
    Customer, 
    Agent, 
    Admin 
}

public record RegisterRequest(
    string FullName, 
    string Email, 
    string Password, 
    UserRole Role = UserRole.Customer
);

public record LoginRequest(
    string Email, 
    string Password
);

public record AuthResponse(
    string Token,
    string FullName,
    string Email,
    string Role,
    DateTime ExpiresAt
);