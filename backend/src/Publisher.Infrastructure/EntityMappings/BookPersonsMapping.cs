using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class BookPersonsMapping : IEntityTypeConfiguration<BookPersons>
{
    public void Configure(EntityTypeBuilder<BookPersons> builder)
    {
        builder.ToTable("BookPersons");
        builder.HasKey(bp => new { bp.BookId, bp.PersonId });

        builder.HasOne(bp => bp.Author)
            .WithMany(a => a.BookPersons)
            .HasForeignKey(bp => bp.AuthorPersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bp => bp.Book)
            .WithMany(b => b.BookPersons)
            .HasForeignKey(bp => bp.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new BookPersons
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000001"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000001")
            },
            new BookPersons
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002")
            },
            new BookPersons
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000001"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000001")
            },
            new BookPersons
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002")
            }
        );
    }
}