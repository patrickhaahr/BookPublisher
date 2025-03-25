using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityConfigurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Fluent API
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(20).HasDefaultValue("user");

        // Seed data
        builder.HasData(
            new User
            {
                UserId = Guid.Parse("c0a80121-0001-4000-0000-000000000030"),
                Username = "user1",
                Email = "user1@example.com",
                PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", // password: "password"
                Role = "User"
            },
            new User
            {
                UserId = Guid.Parse("c0a80121-0001-4000-0000-000000000031"),
                Username = "user2",
                Email = "user2@example.com",
                PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", // password: "password"
                Role = "User"
            }
        );

        // Indexes
        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
    }
}