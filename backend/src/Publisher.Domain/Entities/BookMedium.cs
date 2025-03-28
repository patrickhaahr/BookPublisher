namespace Publisher.Domain.Entities;

// Junction table for Book and Medium
public class BookMedium
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int MediumId { get; set; }
    public MediumEntity Medium { get; set; } = null!;
}