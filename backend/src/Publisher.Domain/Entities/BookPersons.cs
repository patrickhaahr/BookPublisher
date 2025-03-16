namespace Publisher.Domain.Entities;

public class BookPersons
{
    public Guid BookId { get; set; }
    public Guid PersonId { get; set; }
    public Guid AuthorPersonId { get; set; }
    
    // Navigation properties
    // Many-to-One or One-to-One (Reference side) - should be nullable
    public Book Book { get; set; } = null!;
    public Author Author { get; set; } = null!;
} 