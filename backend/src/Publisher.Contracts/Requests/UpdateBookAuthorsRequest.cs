namespace Publisher.Contracts.Requests;

public record UpdateBookAuthorsRequest(
    List<Guid> AuthorIds
); 