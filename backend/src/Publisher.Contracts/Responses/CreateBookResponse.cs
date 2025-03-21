namespace Publisher.Contracts.Responses;

public record CreateBookResponse(
    Guid BookId,
    string Title,
    string Slug,
    List<string> Genres
);