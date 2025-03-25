using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityConfigurations;

public class AuthorConfigurations : IEntityTypeConfiguration<Author>
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
                FirstName = "Homer",
                LastName = "",
                Email = "homer@example.com",
                Phone = "",
                RoyaltyRate = 15.5m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002"),
                FirstName = "Kentaro",
                LastName = "Miura",
                Email = "miura.kentaro@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000003"),
                FirstName = "J.K.",
                LastName = "Rowling",
                Email = "jk.rowling@example.com",
                Phone = "1234567890",
                RoyaltyRate = 5.5m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000004"),
                FirstName = "Eiichiro",
                LastName = "Oda",
                Email = "eiichiro.oda@example.com",
                Phone = "4321432143",
                RoyaltyRate = 14m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000005"),
                FirstName = "Andy",
                LastName = "Hunt",
                Email = "andy.hunt@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000006"),
                FirstName = "Dave",
                LastName = "Thomas",
                Email = "dave.thomas@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000007"),
                FirstName = "Robert",
                LastName = "Kirkman",
                Email = "robert.kirkman@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000008"),
                FirstName = "Masashi",
                LastName = "Kishimoto",
                Email = "masashi.kishimoto@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000009"),
                FirstName = "Frank",
                LastName = "Herbert",
                Email = "frank.herbert@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                FirstName = "David",
                LastName = "Goggins",
                Email = "david.goggins@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                FirstName = "J.R.R.",
                LastName = "Tolkien",
                Email = "jr.tolkien@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                FirstName = "Robert",
                LastName = "Kiyosaki",
                Email = "robert.kiyosaki@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                FirstName = "Sharon",
                LastName = "Lechter",
                Email = "sharon.lechter@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                FirstName = "Robert",
                LastName = "Greene",
                Email = "robert.greene@example.com",
                Phone = "",
                RoyaltyRate = 10m
            },
            new Author
            {
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                FirstName = "Michell",
                LastName = "Cronberg",
                Email = "michell.cronberg@example.com",
                Phone = "",
                RoyaltyRate = 10m
            }
        );
    }
}