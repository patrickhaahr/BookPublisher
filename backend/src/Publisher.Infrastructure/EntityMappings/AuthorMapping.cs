using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class AuthorMapping : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        // Fluent API
        builder.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(a => a.LastName).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Email).IsRequired().HasMaxLength(150);
        builder.Property(a => a.RoyaltyRate).HasColumnType("decimal(5,2)");

        // Seed data
        builder.HasData(
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000001"),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "555-123-4567",
                RoyaltyRate = 15.5m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002"),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Phone = "555-987-6543",
                RoyaltyRate = 12.75m
            }
        );
    }
}