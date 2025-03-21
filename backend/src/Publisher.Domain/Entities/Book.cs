namespace Publisher.Domain.Entities;
public class Book
{
    public Guid BookId { get; set; }
    public required string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public decimal BasePrice { get; set; }
    public string Slug { get; private set; } = string.Empty;
    public List<Genre> Genres { get; set; } = new();
        
    // Navigation properties - Initialize with empty collection
    // Many-to-Many or One-to-Many (Collection side)
    public List<UserBookInteraction> UserBookInteractions { get; set; } = new();
    public List<Cover> Covers { get; set; } = new();
    public List<BookPersons> BookPersons { get; set; } = new();
    public void SetSlug(string slug) => Slug = slug;
} 