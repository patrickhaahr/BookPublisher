using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityConfigurations;

public class BookPersonsConfigurations : IEntityTypeConfiguration<BookPersons>
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
                // The Oddyssey by Homer
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000001"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000001")
            },
            new BookPersons
            {
                // Berserk by Kentaro Miura
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000002")
            },
            new BookPersons
            {
                // Harry Potter by J.K. Rowling
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000003"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000003")
            },
            new BookPersons
            {
                // One Piece by Eiichiro Oda
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000004"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000004")
            },
            new BookPersons
            {
                // The Pragmatic Programmer by Andy Hunt
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000005"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000005")
            },
            new BookPersons
            {
                // The Pragmatic Programmer by Dave Thomas
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000006"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000006")
            },
            new BookPersons
            {
                // Invincible by Robert Kirkman
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000007"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000007")
            },
            new BookPersons
            {
                // Naruto by Masashi Kishimoto
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000008"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000008")
            },
            new BookPersons
            {
                // Dune by Frank Herbert
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000009"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000009")
            },
            new BookPersons
            {
                // Cant hurt me by David Goggins
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000010")
            },
            new BookPersons
            {
                // Never Finished by David Goggins
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000010")
            },
            new BookPersons
            {
                // The Hobbit by J.R.R. Tolkien
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000011")
            },
            new BookPersons
            {
                // The Lord of the Rings by J.R.R. Tolkien
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000011")
            },
            new BookPersons
            {
                // Rich Dad Poor Dad by Robert Kiyosaki
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000012")
            },
            new BookPersons
            {
                // Rich Dad Poor Dad by Sharon Lechter
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000013")
            },
            new BookPersons
            {
                // 48 Laws of Power by Robert Greene
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000014")
            },
            new BookPersons
            {
                // Bogen om C# by Michell Cronberg
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000025"),
                PersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                AuthorPersonId = Guid.Parse("c0a80121-0001-4000-0000-000000000015")
            }
            
        );
    }
}