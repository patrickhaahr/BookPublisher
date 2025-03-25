using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityConfigurations;

public class BookConfigurations : IEntityTypeConfiguration<Book>
{
    private static readonly JsonSerializerOptions _jsonOptions = new();

    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // Fluent API
        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Slug).IsRequired().HasMaxLength(200);
        builder.Property(b => b.BasePrice).HasColumnType("decimal(18,2)");

        // Slug
        builder.HasIndex(b => b.Slug).IsUnique();
        
        // Mediums
        builder.Property(b => b.Mediums)
            .HasConversion(
                v => JsonSerializer.Serialize(v ?? new List<Medium>(), _jsonOptions),
                v => JsonSerializer.Deserialize<List<Medium>>(v ?? "{}", _jsonOptions) ?? new List<Medium>(),
                new ValueComparer<List<Medium>>(
                    (a, b) => (a ?? new List<Medium>()).SequenceEqual(b ?? new List<Medium>()),
                    c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c == null ? new List<Medium>() : c.ToList()));
        
        // Genres
        builder.Property(b => b.Genres)
            .HasConversion(
                v => JsonSerializer.Serialize(v ?? new List<Genre>(), _jsonOptions),
                v => JsonSerializer.Deserialize<List<Genre>>(v ?? "{}", _jsonOptions) ?? new List<Genre>(),
                new ValueComparer<List<Genre>>(
                    (a, b) => (a ?? new List<Genre>()).SequenceEqual(b ?? new List<Genre>()),
                    c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c == null ? new List<Genre>() : c.ToList()));

        // Seed data
        var book1 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
            Title = "The Oddyssey",
            PublishDate = new DateTime(0001, 1, 1),
            BasePrice = 19.99m,
            Mediums = [Medium.Print, Medium.EBook, Medium.AudioBook],
            Genres = [Genre.Epic, Genre.Poetry]
        };
        book1.SetSlug("the-odyssey");

        var book2 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
            Title = "Berserk",
            PublishDate = new DateTime(1990, 10, 26),
            BasePrice = 9.99m,
            Mediums = [Medium.Manga, Medium.EBook, Medium.Print],
            Genres = [Genre.Adventure, Genre.DarkFantasy, Genre.Fiction]
        };
        book2.SetSlug("berserk");

        var book3 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
            Title = "Harry Potter",
            PublishDate = new DateTime(1997, 6, 26),
            BasePrice = 29.99m,
            Mediums = [Medium.Novel, Medium.EBook, Medium.Print],
            Genres = [Genre.Fantasy, Genre.Fiction]
        };
        book3.SetSlug("harry-potter");

        var book4 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
            Title = "One Piece",
            PublishDate = new DateTime(1999, 7, 22),
            BasePrice = 9.99m,
            Mediums = [Medium.Manga, Medium.EBook, Medium.Print],
            Genres = [Genre.Adventure, Genre.Fantasy, Genre.Fiction]
        };
        book4.SetSlug("one-piece");

        // The Pragmatic Programmer
        var book5 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
            Title = "The Pragmatic Programmer",
            PublishDate = new DateTime(1999, 10, 20),
            BasePrice = 29.99m,
            Mediums = [Medium.EBook, Medium.Print],
            Genres = [Genre.Programming, Genre.NonFiction]
        };
        book5.SetSlug("the-pragmatic-programmer");

        // Invincible
        var book6 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
            Title = "Invincible",
            PublishDate = new DateTime(2003, 10, 20),
            BasePrice = 9.99m,
            Mediums = [Medium.Comic, Medium.GraphicNovel, Medium.EBook, Medium.Print],
            Genres = [Genre.Adventure, Genre.Fantasy, Genre.Fiction]
        };
        book6.SetSlug("invincible");

        // Naruto
        var book7 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
            Title = "Naruto",
            PublishDate = new DateTime(2002, 9, 21),
            BasePrice = 9.99m,
            Mediums = [Medium.Manga, Medium.EBook, Medium.Print],
            Genres = [Genre.Adventure, Genre.Fantasy, Genre.Fiction]
        };
        book7.SetSlug("naruto");

        // Dune
        var book8 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
            Title = "Dune",
            PublishDate = new DateTime(1965, 10, 20),
            BasePrice = 9.99m,
            Mediums = [Medium.Novel, Medium.EBook, Medium.Print],
            Genres = [Genre.Adventure, Genre.Fantasy, Genre.Fiction]
        };
        book8.SetSlug("dune");

        // Cant hurt me
        var book9 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
            Title = "Cant hurt me",
            PublishDate = new DateTime(2018, 10, 15),
            BasePrice = 29.99m,
            Mediums = [Medium.EBook, Medium.Print, Medium.AudioBook],
            Genres = [Genre.SelfHelp, Genre.NonFiction]
        };
        book9.SetSlug("cant-hurt-me");

        // Never Finished
        var book10 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
            Title = "Never Finished",
            PublishDate = new DateTime(2022, 12, 4),
            BasePrice = 29.99m,
            Mediums = [Medium.EBook, Medium.Print, Medium.AudioBook],
            Genres = [Genre.SelfHelp, Genre.NonFiction]
        };
        book10.SetSlug("never-finished");
        
        // The Hobbit
        var book11 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
            Title = "The Hobbit",
            PublishDate = new DateTime(1937, 9, 21),
            BasePrice = 9.99m,
            Mediums = [Medium.Novel, Medium.EBook, Medium.Print],
            Genres = [Genre.Adventure, Genre.Fantasy, Genre.Fiction]
        };
        book11.SetSlug("the-hobbit");
        
        // The Lord of the Rings
        var book12 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
            Title = "The Lord of the Rings",
            PublishDate = new DateTime(1954, 7, 29),
            BasePrice = 9.99m,
            Mediums = [Medium.Novel, Medium.EBook, Medium.Print],
            Genres = [Genre.Adventure, Genre.Fantasy, Genre.Fiction]
        };
        book12.SetSlug("the-lord-of-the-rings");
        
        // Rich Dad Poor Dad
        var book13 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
            Title = "Rich Dad Poor Dad",
            PublishDate = new DateTime(1997, 9, 21),
            BasePrice = 9.99m,
            Mediums = [Medium.EBook, Medium.Print],
            Genres = [Genre.Finance, Genre.NonFiction]
        };
        book13.SetSlug("rich-dad-poor-dad");
        
        // 48 Laws of Power
        var book14 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
            Title = "48 Laws of Power",
            PublishDate = new DateTime(1998, 9, 21),
            BasePrice = 9.99m,
            Mediums = [Medium.EBook, Medium.Print, Medium.AudioBook],
            Genres = [Genre.SelfHelp, Genre.NonFiction]
        };
        book14.SetSlug("48-laws-of-power");
        
        // How to Scam People
        var book15 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000024"),
            Title = "How to Scam People in 8 Easy Steps",
            PublishDate = new DateTime(2023, 12, 29),
            BasePrice = 15.99m,
            Mediums = [Medium.EBook, Medium.Print],
            Genres = [Genre.NonFiction, Genre.SelfHelp]
        };
        book15.SetSlug("how-to-scam-people");

        // Bogen om C#
        var book16 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000025"),
            Title = "Bogen om C#",
            PublishDate = new DateTime(2021, 1, 1),
            BasePrice = 0m,
            Mediums = [Medium.EBook, Medium.Print],
            Genres = [Genre.Programming, Genre.NonFiction]
        };
        book16.SetSlug("bogen-om-c-sharp");

        builder.HasData(book1, book2, book3, book4, book5, book6, book7, book8, book9, book10, book11, book12, book13, book14, book15, book16);
    }
}