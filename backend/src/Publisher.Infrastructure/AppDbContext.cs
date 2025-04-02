using Microsoft.EntityFrameworkCore;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Expression-Bodied Property with Set<T>()
    // Expresses the intent of the property
    public DbSet<Book> Books => Set<Book>();
    public DbSet<GenreEntity> Genres => Set<GenreEntity>();
    public DbSet<MediumEntity> Mediums => Set<MediumEntity>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Cover> Covers => Set<Cover>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserBookInteraction> UserBookInteractions => Set<UserBookInteraction>();
    public DbSet<BookPersons> BookPersons => Set<BookPersons>();
    public DbSet<CoverPersons> CoverPersons => Set<CoverPersons>();
    public DbSet<BookMedium> BookMediums => Set<BookMedium>();
    public DbSet<BookGenre> BookGenres => Set<BookGenre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity mappings automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Example of explicitly applying a mapping
        // Violates the DRY principle
        // modelBuilder.ApplyConfiguration(new BookMapping())
    }
}