namespace Publisher.Domain.Entities;
public class Artist : Person
{
    public string? PortfolioUrl { get; set; }
        
    // Navigation properties
    public List<CoverPersons> CoverPersons { get; set; } = new();
}