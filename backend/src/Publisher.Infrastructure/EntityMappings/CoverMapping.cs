using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class CoverMapping : IEntityTypeConfiguration<Cover>
{
    public void Configure(EntityTypeBuilder<Cover> builder)
    {
        // Fluent API
        builder.Property(c => c.ImgBase64).HasColumnType("NVARCHAR(MAX)");

        // Seed data
        builder.HasData(
            new Cover
            {
                CoverId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                ImgBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==",
                CreatedDate = new DateTime(2023, 1, 10),
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010")
            },
            new Cover
            {
                CoverId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                ImgBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==",
                CreatedDate = new DateTime(2023, 3, 5),
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011")
            },
            new Cover
            {
                CoverId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                ImgBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==",
                CreatedDate = new DateTime(2023, 5, 15),
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012")
            }
        );
    }
}