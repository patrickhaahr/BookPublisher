namespace Publisher.Contracts.Responses;

public record DeleteBookResponse(
    Guid BookId,
    string Title,
    string Slug
); 