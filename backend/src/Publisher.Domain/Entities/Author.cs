namespace Publisher.Domain.Entities;
public class Author : Person
{
    public decimal RoyaltyRate { get; set; }
        
    // Navigation properties
    public List<BookPersons> BookPersons { get; set; } = new();
} 