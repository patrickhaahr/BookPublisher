using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class BookGenreMapping : IEntityTypeConfiguration<BookGenres>
{
    public void Configure(EntityTypeBuilder<BookGenres> builder)
    {
        // Fluent API
        builder.ToTable("BookGenres");
        builder.HasKey(bg => new { bg.BookId, bg.GenreId });

        // Seed data
        builder.HasData(
            new BookGenres
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                GenreId = 3 // Fantasy
            },
            new BookGenres
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                GenreId = 2 // Mystery
            },
            new BookGenres
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                GenreId = 1 // Science Fiction
            },
            new BookGenres
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                GenreId = 4 // Non-Fiction
            }
        );
    }
}