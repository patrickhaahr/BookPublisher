using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;

namespace Publisher.Infrastructure.EntityConfigurations;

public class BookGenreConfigurations : IEntityTypeConfiguration<BookGenre>
{
    public void Configure(EntityTypeBuilder<BookGenre> builder)
    {
        // Configure BookGenre (Book - Genre many-to-many)
        builder.HasKey(bg => new { bg.BookId, bg.GenreId });

        builder.HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId);

        builder.HasOne(bg => bg.Genre)
            .WithMany() // No navigation collection on GenreEntity
            .HasForeignKey(bg => bg.GenreId);

       // Seed BookGenre Relationships for "The Odyssey"
        var book1Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                GenreId = (int)GenreEnum.Epic
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                GenreId = (int)GenreEnum.Poetry
            },
        };

        // Seed BookGenre Relationships for "Berserk"
        var book2Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                GenreId = (int)GenreEnum.Adventure
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                GenreId = (int)GenreEnum.DarkFantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "Harry Potter"
        var book3Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                GenreId = (int)GenreEnum.Fantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "One Piece"
        var book4Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                GenreId = (int)GenreEnum.Adventure
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                GenreId = (int)GenreEnum.Fantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "The Pragmatic Programmer"
        var book5Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                GenreId = (int)GenreEnum.Programming
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                GenreId = (int)GenreEnum.NonFiction
            },
        };

        // Seed BookGenre Relationships for "Invincible"
        var book6Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                GenreId = (int)GenreEnum.Adventure
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                GenreId = (int)GenreEnum.Fantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "Naruto"
        var book7Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
                GenreId = (int)GenreEnum.Adventure
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
                GenreId = (int)GenreEnum.Fantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "Dune"
        var book8Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
                GenreId = (int)GenreEnum.Adventure
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
                GenreId = (int)GenreEnum.Fantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "Cant hurt me"
        var book9Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
                GenreId = (int)GenreEnum.SelfHelp
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
                GenreId = (int)GenreEnum.NonFiction
            },
        };

        // Seed BookGenre Relationships for "Never Finished"
        var book10Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
                GenreId = (int)GenreEnum.SelfHelp
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
                GenreId = (int)GenreEnum.NonFiction
            },
        };

        // Seed BookGenre Relationships for "The Hobbit"
        var book11Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                GenreId = (int)GenreEnum.Adventure
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                GenreId = (int)GenreEnum.Fantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "The Lord of the Rings"
        var book12Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                GenreId = (int)GenreEnum.Adventure
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                GenreId = (int)GenreEnum.Fantasy
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                GenreId = (int)GenreEnum.Fiction
            },
        };

        // Seed BookGenre Relationships for "Rich Dad Poor Dad"
        var book13Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                GenreId = (int)GenreEnum.Finance
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                GenreId = (int)GenreEnum.NonFiction
            },
        };

        // Seed BookGenre Relationships for "48 Laws of Power"
        var book14Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
                GenreId = (int)GenreEnum.SelfHelp
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
                GenreId = (int)GenreEnum.NonFiction
            },
        };

        // Seed BookGenre Relationships for "How to Scam People"
        var book15Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000024"),
                GenreId = (int)GenreEnum.NonFiction
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000024"),
                GenreId = (int)GenreEnum.SelfHelp
            },
        };

        // Seed BookGenre Relationships for "Bogen om C#"
        var book16Genres = new[]
        {
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000025"),
                GenreId = (int)GenreEnum.Programming
            },
            new BookGenre
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000025"),
                GenreId = (int)GenreEnum.NonFiction
            },
        };

        // Combine all arrays into a single collection
        var allBookGenres = new List<BookGenre>();
        allBookGenres.AddRange(book1Genres);
        allBookGenres.AddRange(book2Genres);
        allBookGenres.AddRange(book3Genres);
        allBookGenres.AddRange(book4Genres);
        allBookGenres.AddRange(book5Genres);
        allBookGenres.AddRange(book6Genres);
        allBookGenres.AddRange(book7Genres);
        allBookGenres.AddRange(book8Genres);
        allBookGenres.AddRange(book9Genres);
        allBookGenres.AddRange(book10Genres);
        allBookGenres.AddRange(book11Genres);
        allBookGenres.AddRange(book12Genres);
        allBookGenres.AddRange(book13Genres);
        allBookGenres.AddRange(book14Genres);
        allBookGenres.AddRange(book15Genres);
        allBookGenres.AddRange(book16Genres);
        
        // Seed all data at once
        builder.HasData(allBookGenres);
    }
}
