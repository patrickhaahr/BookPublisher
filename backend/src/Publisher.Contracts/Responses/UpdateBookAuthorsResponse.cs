namespace Publisher.Contracts.Responses;

public record UpdateBookAuthorsResponse(
    Guid BookId,
    List<Guid> AuthorIds
); 