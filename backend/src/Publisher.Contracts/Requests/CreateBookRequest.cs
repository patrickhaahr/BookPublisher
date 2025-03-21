namespace Publisher.Contracts.Requests;

public record CreateBookRequest(
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    List<string> Mediums,
    List<string> Genres,
    List<Guid> AuthorIds,
    List<CreateCoverRequest>? Covers
);
