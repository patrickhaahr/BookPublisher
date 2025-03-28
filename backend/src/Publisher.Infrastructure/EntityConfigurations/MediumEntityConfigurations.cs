using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;

namespace Publisher.Infrastructure.EntityConfigurations;

public class MediumEntityConfigurations : IEntityTypeConfiguration<MediumEntity>
{
    public void Configure(EntityTypeBuilder<MediumEntity> builder)
    {
        builder.HasKey(m => m.Id);

        // Seed MediumEntity data
        var mediums = Enum.GetValues<MediumEnum>()
            .Select(m => new MediumEntity { Id = (int)m, Name = m.ToString() })
            .ToList();

        builder.HasData(mediums);
    }
}