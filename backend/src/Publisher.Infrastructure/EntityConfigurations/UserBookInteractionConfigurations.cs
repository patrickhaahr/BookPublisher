using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.EntityConfigurations;

public class UserBookInteractionConfigurations : IEntityTypeConfiguration<UserBookInteraction>
{
    public void Configure(EntityTypeBuilder<UserBookInteraction> builder)
    {
        // Primary key
        builder.HasKey(ubi => ubi.InteractionId);
        
        // Indexes for foreign keys
        builder.HasIndex(ubi => ubi.UserId);
        builder.HasIndex(ubi => ubi.BookId);
        
        // Fluent API
        builder.ToTable("UserBookInteractions");
        builder.Property(ubi => ubi.Status).HasMaxLength(50);
        builder.Property(ubi => ubi.Rating).HasColumnType("int");

        // Seed data
        builder.HasData(
            new UserBookInteraction
            {
                InteractionId = Guid.Parse("c0a80121-0001-4000-0000-000000000040"),
                UserId = Guid.Parse("c0a80121-0001-4000-0000-000000000030"),
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000010"),
                IsFavorite = true,
                IsSaved = true,
                Status = "Read",
                Rating = 5
            },
            new UserBookInteraction
            {
                InteractionId = Guid.Parse("c0a80121-0001-4000-0000-000000000041"),
                UserId = Guid.Parse("c0a80121-0001-4000-0000-000000000031"),
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000011"),
                IsFavorite = false,
                IsSaved = true,
                Status = "Reading",
                Rating = null
            },
            new UserBookInteraction
            {
                InteractionId = Guid.Parse("c0a80121-0001-4000-0000-000000000042"),
                UserId = Guid.Parse("c0a80121-0001-4000-0000-000000000031"),
                BookId = Guid.Parse("c0a80121-0001-4000-0000-000000000012"),
                IsFavorite = true,
                IsSaved = false,
                Status = "Want to Read",
                Rating = null
            }
        );
    }
}