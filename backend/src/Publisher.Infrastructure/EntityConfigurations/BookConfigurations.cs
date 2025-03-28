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
        builder.HasIndex(b => b.Slug).IsUnique();

        // Seed data
        var book1 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
            Title = "The Oddyssey",
            PublishDate = new DateTime(0001, 1, 1),
            BasePrice = 19.99m
        };
        book1.SetSlug("the-odyssey");

        var book2 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
            Title = "Berserk",
            PublishDate = new DateTime(1990, 10, 26),
            BasePrice = 9.99m
        };
        book2.SetSlug("berserk");

        var book3 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
            Title = "Harry Potter",
            PublishDate = new DateTime(1997, 6, 26),
            BasePrice = 29.99m
        };
        book3.SetSlug("harry-potter");

        var book4 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
            Title = "One Piece",
            PublishDate = new DateTime(1999, 7, 22),
            BasePrice = 9.99m
        };
        book4.SetSlug("one-piece");

        // The Pragmatic Programmer
        var book5 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
            Title = "The Pragmatic Programmer",
            PublishDate = new DateTime(1999, 10, 20),
            BasePrice = 29.99m
        };
        book5.SetSlug("the-pragmatic-programmer");

        // Invincible
        var book6 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
            Title = "Invincible",
            PublishDate = new DateTime(2003, 10, 20),
            BasePrice = 9.99m               
        };
        book6.SetSlug("invincible");

        // Naruto
        var book7 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
            Title = "Naruto",
            PublishDate = new DateTime(2002, 9, 21),
            BasePrice = 9.99m
        };
        book7.SetSlug("naruto");

        // Dune
        var book8 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
            Title = "Dune",
            PublishDate = new DateTime(1965, 10, 20),
            BasePrice = 9.99m       
        };
        book8.SetSlug("dune");

        // Cant hurt me
        var book9 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
            Title = "Cant hurt me",
            PublishDate = new DateTime(2018, 10, 15),
            BasePrice = 29.99m
        };
        book9.SetSlug("cant-hurt-me");

        // Never Finished
        var book10 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
            Title = "Never Finished",
            PublishDate = new DateTime(2022, 12, 4),
            BasePrice = 29.99m,
        };
        book10.SetSlug("never-finished");
        
        // The Hobbit
        var book11 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
            Title = "The Hobbit",
            PublishDate = new DateTime(1937, 9, 21),
            BasePrice = 9.99m
        };
        book11.SetSlug("the-hobbit");
        
        // The Lord of the Rings
        var book12 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
            Title = "The Lord of the Rings",
            PublishDate = new DateTime(1954, 7, 29),
            BasePrice = 9.99m
        };
        book12.SetSlug("the-lord-of-the-rings");
        
        // Rich Dad Poor Dad
        var book13 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
            Title = "Rich Dad Poor Dad",
            PublishDate = new DateTime(1997, 9, 21),
            BasePrice = 9.99m
        };
        book13.SetSlug("rich-dad-poor-dad");
        
        // 48 Laws of Power
        var book14 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
            Title = "48 Laws of Power",
            PublishDate = new DateTime(1998, 9, 21),
            BasePrice = 9.99m
        };
        book14.SetSlug("48-laws-of-power");
        
        // How to Scam People
        var book15 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000024"),
            Title = "How to Scam People in 8 Easy Steps",
            PublishDate = new DateTime(2023, 12, 29),
            BasePrice = 15.99m,
        };
        book15.SetSlug("how-to-scam-people");

        // Bogen om C#
        var book16 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000025"),
            Title = "Bogen om C#",
            PublishDate = new DateTime(2021, 1, 1),
            BasePrice = 0m,
        };
        book16.SetSlug("bogen-om-c-sharp");

        builder.HasData(book1, book2, book3, book4, book5, book6, book7, book8, book9, book10, book11, book12, book13, book14, book15, book16);
    }
}