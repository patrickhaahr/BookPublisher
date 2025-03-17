namespace Publisher.Contracts.Responses;

public record UpdateBookResponse(
    Guid BookId,
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    string Slug
); 