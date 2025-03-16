using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityMappings;

public class PersonMapping : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        // TPT inheritance
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Phone).HasMaxLength(20);

        // Discriminator
        builder.HasDiscriminator<string>("PersonType")
            .HasValue<Author>("Author")
            .HasValue<Artist>("Artist");
    }
}