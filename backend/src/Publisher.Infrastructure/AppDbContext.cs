using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Publisher.Domain.Entities;
using Publisher.Domain.Entities.ViewModels;

namespace Publisher.Infrastructure;

public class AppDbContext : DbContext
{
    // Expression-Bodied Property with Set<T>()
    // Expresses the intent of the property
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Cover> Covers => Set<Cover>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserBookInteraction> UserBookInteractions => Set<UserBookInteraction>();
    public DbSet<BookPersons> BookPersons => Set<BookPersons>();
    public DbSet<CoverPersons> CoverPersons => Set<CoverPersons>();
    public DbSet<BookSummary> BookSummaries => Set<BookSummary>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity mappings automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Example of explicitly applying a mapping
        // Violates the DRY principle
        // modelBuilder.ApplyConfiguration(new BookMapping())
    }
}