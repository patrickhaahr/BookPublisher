namespace Publisher.Domain.Entities;

public class BookGenres
{
    public Guid BookId { get; set; }
    public int GenreId { get; set; }
        
    // Navigation properties
    public Book? Book { get; set; }
    public Genre? Genre { get; set; }
} 