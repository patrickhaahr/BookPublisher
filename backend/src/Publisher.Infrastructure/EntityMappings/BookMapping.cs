using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class BookMapping : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // Fluent API
        builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Slug).IsRequired().HasMaxLength(200);
        builder.Property(b => b.BasePrice).HasColumnType("decimal(18,2)");

        // Slug
        builder.HasIndex(b => b.Slug).IsUnique();

        // Genres
        builder.Property(b => b.Genres)
            .HasConversion(
                v => JsonSerializer.Serialize(v ?? new List<Genre>(), new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<List<Genre>>(v ?? "{}", new JsonSerializerOptions()) ?? new List<Genre>(),
                new ValueComparer<List<Genre>>(
                    (a, b) => (a ?? new List<Genre>()).SequenceEqual(b ?? new List<Genre>()),
                    c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c == null ? new List<Genre>() : c.ToList()));

        // Seed data
        var book1 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
            Title = "The Great Adventure",
            PublishDate = new DateTime(2023, 1, 15),
            BasePrice = 19.99m,
            Genres = [Genre.NonFiction]
        };
        book1.SetSlug("the-great-adventure");

        var book2 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
            Title = "Mystery of the Lost City",
            PublishDate = new DateTime(2023, 3, 10),
            BasePrice = 24.99m,
            Genres = [Genre.Mystery]
        };
        book2.SetSlug("mystery-of-the-lost-city");

        var book3 = new Book
        {
            BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
            Title = "Future Technologies",
            PublishDate = new DateTime(2023, 5, 22),
            BasePrice = 29.99m,
            Genres = [Genre.ScienceFiction]
        };
        book3.SetSlug("future-technologies");

        builder.HasData(book1, book2, book3);
    }
}