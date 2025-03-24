using Microsoft.AspNetCore.Identity;
using Publisher.Application.Interfaces.Authentication;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _identityHasher = new();
    public string HashPassword(string password)
    {
        var dummyUser = new User
        {
            UserId = Guid.Empty,
            Username = "dummy",
            Email = "dummy@example.com",
            PasswordHash = "dummy",
            Role = "dummy",
        };
        return _identityHasher.HashPassword(dummyUser, password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var dummyUser = new User
        {
            UserId = Guid.Empty,
            Username = "dummy",
            Email = "dummy@example.com",
            PasswordHash = "dummy",
            Role = "dummy",
        };
        return _identityHasher.VerifyHashedPassword(dummyUser, hashedPassword, password) == PasswordVerificationResult.Success;
    }
}

