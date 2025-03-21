namespace Publisher.Contracts.Responses;

public record BookSummaryResponse(
    Guid BookId,
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    string Slug,
    List<string> Mediums,
    List<string> Genres
);

public record GetBooksResponse(
    List<BookSummaryResponse> Books
); 