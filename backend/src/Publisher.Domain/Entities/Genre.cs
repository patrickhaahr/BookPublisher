namespace Publisher.Domain.Entities;

public class Genre
{
    public int GenreId { get; set; }
    public required string Name { get; set; }
        
    // Navigation properties
    public List<BookGenres> BookGenres { get; set; } = new();
} 