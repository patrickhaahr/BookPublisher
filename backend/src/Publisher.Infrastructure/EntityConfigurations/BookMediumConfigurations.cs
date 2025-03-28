using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;

namespace Publisher.Infrastructure.EntityConfigurations;
public class BookMediumConfigurations : IEntityTypeConfiguration<BookMedium>
{
    public void Configure(EntityTypeBuilder<BookMedium> builder)
    {
        // Configure BookMedium (Book - Medium many-to-many)
        builder.HasKey(bm => new { bm.BookId, bm.MediumId });

        builder.HasOne(bm => bm.Book)
            .WithMany(b => b.BookMediums)
            .HasForeignKey(bm => bm.BookId);

        builder.HasOne(bm => bm.Medium)
            .WithMany() // No navigation collection on MediumEntity
            .HasForeignKey(bm => bm.MediumId);

        // Seed BookMedium Relationships for "The Odyssey"
        var book1Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                MediumId = (int)MediumEnum.Print
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                MediumId = (int)MediumEnum.AudioBook
            }
        };

        // Seed BookMedium Relationships for "Berserk"
        var book2Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                MediumId = (int)MediumEnum.Manga
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "Harry Potter"
        var book3Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                MediumId = (int)MediumEnum.Novel
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "One Piece"
        var book4Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                MediumId = (int)MediumEnum.Manga
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000013"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "The Pragmatic Programmer"
        var book5Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000014"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "Invincible"
        var book6Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                MediumId = (int)MediumEnum.Comic
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                MediumId = (int)MediumEnum.GraphicNovel
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000015"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "Naruto"
        var book7Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
                MediumId = (int)MediumEnum.Manga
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000016"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "Dune"
        var book8Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
                MediumId = (int)MediumEnum.Novel
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000017"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "Cant hurt me"
        var book9Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
                MediumId = (int)MediumEnum.Print
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000018"),
                MediumId = (int)MediumEnum.AudioBook
            }
        };

        // Seed BookMedium Relationships for "Never Finished"
        var book10Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
                MediumId = (int)MediumEnum.Print
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000019"),
                MediumId = (int)MediumEnum.AudioBook
            }
        };

        // Seed BookMedium Relationships for "The Hobbit"
        var book11Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                MediumId = (int)MediumEnum.Novel
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "The Lord of the Rings"
        var book12Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                MediumId = (int)MediumEnum.Novel
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "Rich Dad Poor Dad"
        var book13Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "48 Laws of Power"
        var book14Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
                MediumId = (int)MediumEnum.Print
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000023"),
                MediumId = (int)MediumEnum.AudioBook
            }
        };

        // Seed BookMedium Relationships for "How to Scam People"
        var book15Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000024"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000024"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Seed BookMedium Relationships for "Bogen om C#"
        var book16Mediums = new[]
        {
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000025"),
                MediumId = (int)MediumEnum.EBook
            },
            new BookMedium
            {
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000025"),
                MediumId = (int)MediumEnum.Print
            }
        };

        // Combine all arrays into a single collection
        var allBookMediums = new List<BookMedium>();
        allBookMediums.AddRange(book1Mediums);
        allBookMediums.AddRange(book2Mediums);
        allBookMediums.AddRange(book3Mediums);
        allBookMediums.AddRange(book4Mediums);
        allBookMediums.AddRange(book5Mediums);
        allBookMediums.AddRange(book6Mediums);
        allBookMediums.AddRange(book7Mediums);
        allBookMediums.AddRange(book8Mediums);
        allBookMediums.AddRange(book9Mediums);
        allBookMediums.AddRange(book10Mediums);
        allBookMediums.AddRange(book11Mediums);
        allBookMediums.AddRange(book12Mediums);
        allBookMediums.AddRange(book13Mediums);
        allBookMediums.AddRange(book14Mediums);
        allBookMediums.AddRange(book15Mediums);
        allBookMediums.AddRange(book16Mediums);
        
        // Seed all data at once
        builder.HasData(allBookMediums);
    }   
}
