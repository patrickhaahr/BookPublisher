using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;

namespace Publisher.Infrastructure.EntityConfigurations;

public class GenreEntityConfigurations : IEntityTypeConfiguration<GenreEntity>
{
    public void Configure(EntityTypeBuilder<GenreEntity> builder)
    {
        builder.HasKey(g => g.Id);

        // Seed GenreEntity data
        var genres = Enum.GetValues<GenreEnum>()
            .Select(g => new GenreEntity { Id = (int)g, Name = g.ToString() })
            .ToList();

        builder.HasData(genres);
    }
}