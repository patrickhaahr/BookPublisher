namespace Publisher.Domain.Entities;

public class Cover
{
    public Guid CoverId { get; set; }
    public string? ImgBase64 { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid BookId { get; set; }
        
    // Navigation properties
    public Book? Book { get; set; }
    public List<CoverPersons> CoverPersons { get; set; } = new();
} 