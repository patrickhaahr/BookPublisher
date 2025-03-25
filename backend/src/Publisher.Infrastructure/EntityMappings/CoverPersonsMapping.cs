using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class CoverPersonsMapping : IEntityTypeConfiguration<CoverPersons>
{
    public void Configure(EntityTypeBuilder<CoverPersons> builder)
    {
        builder.ToTable("CoverPersons");
        builder.HasKey(cp => new { cp.CoverId, cp.PersonId });

        builder.HasOne(cp => cp.Artist)
            .WithMany(a => a.CoverPersons)
            .HasForeignKey(cp => cp.ArtistPersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cp => cp.Cover)
            .WithMany(c => c.CoverPersons)
            .HasForeignKey(cp => cp.CoverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new CoverPersons
            {
                CoverId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                PersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000001"),
                ArtistPersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000001")
            },
            new CoverPersons
            {
                CoverId = Guid.Parse("c0a80121-0001-4000-0000-000000000020"),
                PersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000002"),
                ArtistPersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000002")
            },
            new CoverPersons
            {
                CoverId = Guid.Parse("c0a80121-0001-4000-0000-000000000021"),
                PersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000001"),
                ArtistPersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000001")
            },
            new CoverPersons
            {
                CoverId = Guid.Parse("c0a80121-0001-4000-0000-000000000022"),
                PersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000002"),
                ArtistPersonId = Guid.Parse("c0a80121-0001-4100-0000-000000000002")
            }
        );
    }
}