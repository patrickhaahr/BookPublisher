using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // Fluent API
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.HasKey(g => g.GenreId);
        builder.Property(g => g.GenreId).ValueGeneratedOnAdd();

        // Seed data
        builder.HasData(
            new Genre { GenreId = 1, Name = "Science Fiction" },    
            new Genre { GenreId = 2, Name = "Mystery" },
            new Genre { GenreId = 3, Name = "Fantasy" },
            new Genre { GenreId = 4, Name = "Non-Fiction" },
            new Genre { GenreId = 5, Name = "Biography" }
        );
    }
}