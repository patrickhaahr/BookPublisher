namespace Publisher.Domain.Entities.ViewModels;

// Keyless entity
public class BookSummary
{
    public Guid BookId { get; set; }
    public required string Title { get; set; }
    public double AverageRating { get; set; }
    public int FavoriteCount { get; set; }
    public int CompletedCount { get; set; }
    public int PlanToReadCount { get; set; }
    public int DroppedCount { get; set; }
    public int OnHoldCount { get; set; }
}