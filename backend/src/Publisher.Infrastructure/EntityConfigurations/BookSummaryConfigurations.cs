using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Publisher.Domain.Entities.ViewModels;

namespace Publisher.Infrastructure.EntityConfigurations;

public class BookSummaryConfigurations : IEntityTypeConfiguration<BookSummary>
{
    public void Configure(EntityTypeBuilder<BookSummary> builder)
    {
        // Keyless entity
        builder.HasNoKey();
        builder.ToView("vw_BookSummary");
        builder.Property(bs => bs.AverageRating).HasColumnType("float");        
    }
}