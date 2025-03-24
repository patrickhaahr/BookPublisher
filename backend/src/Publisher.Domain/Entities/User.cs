namespace Publisher.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
        
    // Navigation properties
    public List<UserBookInteraction> UserBookInteractions { get; set; } = new();
} 