namespace Publisher.Domain.Entities;

public class UserBookInteraction
{
    public Guid InteractionId { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public bool IsFavorite { get; set; } = false;
    public bool IsSaved { get; set; } = false;
    public string? Status { get; set; }
    public int? Rating { get; set; }
        
    // Navigation properties
    public User? User { get; set; }
    public Book? Book { get; set; }
} 