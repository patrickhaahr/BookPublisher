namespace Publisher.Domain.Entities;

// Junction table for Book and Genre
public class BookGenre
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int GenreId { get; set; }
    public GenreEntity Genre { get; set; } = null!;
}