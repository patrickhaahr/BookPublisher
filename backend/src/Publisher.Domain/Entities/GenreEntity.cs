namespace Publisher.Domain.Entities;

public class GenreEntity
{
    public int Id { get; set; } // Matches Genre enum value
    public string Name { get; set; } = string.Empty;
}
