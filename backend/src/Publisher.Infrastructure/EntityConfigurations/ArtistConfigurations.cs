using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityConfigurations;

public class ArtistConfigurations : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        // Fluent API
        builder.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(a => a.LastName).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Email).IsRequired().HasMaxLength(150);
        builder.Property(a => a.PortfolioUrl).HasMaxLength(255);

        // Seed data
        builder.HasData(
            new Artist
            {
                PersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000001"),
                FirstName = "Michael",
                LastName = "Johnson",
                Email = "michael.johnson@example.com",
                Phone = "555-222-3333",
                PortfolioUrl = "https://portfolio.michaeljohnson.com"
            },
            new Artist
            {
                PersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000002"),
                FirstName = "Sarah",
                LastName = "Williams",
                Email = "sarah.williams@example.com",
                Phone = "555-444-5555",
                PortfolioUrl = "https://sarahwilliams-art.com"
            }
        );
    }
}