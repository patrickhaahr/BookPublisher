namespace Publisher.Contracts.Requests;

public record CreateBookRequest(
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    List<int> GenreIds,
    List<Guid> AuthorIds,
    List<CreateCoverRequest>? Covers
);
