using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Publisher.Infrastructure.EntityConfigurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Fluent API
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Role).IsRequired().HasMaxLength(20);
        builder.Property(u => u.RefreshToken).IsRequired(false);
        builder.Property(u => u.RefreshTokenExpiryTime).IsRequired(false);

        // Indexes
        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        // Seed data
        var passwordHasher = new PasswordHasher<User>();
        var user1Id = Guid.Parse("c0a80121-0001-4000-0000-000000000030");
        var user2Id = Guid.Parse("c0a80121-0001-4000-0000-000000000031");
        var adminId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef");

        builder.HasData(
            new User
            {
                UserId = user1Id,
                Username = "user1",
                Email = "user1@example.com",
                PasswordHash = passwordHasher.HashPassword(null!, "password"),
                Role = "User",
                RefreshToken = null,
                RefreshTokenExpiryTime = null
            },
            new User
            {
                UserId = user2Id,
                Username = "user2",
                Email = "user2@example.com",
                PasswordHash = passwordHasher.HashPassword(null!, "password"),
                Role = "User",
                RefreshToken = null,
                RefreshTokenExpiryTime = null
            },
            new User
            {
                UserId = adminId,
                Username = "admin",
                Email = "admin@publisher.com",
                PasswordHash = passwordHasher.HashPassword(null!, "admin"),
                Role = "Admin",
                RefreshToken = null,
                RefreshTokenExpiryTime = null
            }
        );
    }
}