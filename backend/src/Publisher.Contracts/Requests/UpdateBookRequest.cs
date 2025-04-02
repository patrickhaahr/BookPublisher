namespace Publisher.Contracts.Requests;

public record UpdateBookRequest(
    string? Title,
    DateTime? PublishDate,
    decimal? BasePrice,
    List<string>? Mediums,
    List<string>? Genres,
    List<Guid>? AuthorIds,
    List<UpdateCoverData>? Covers
);