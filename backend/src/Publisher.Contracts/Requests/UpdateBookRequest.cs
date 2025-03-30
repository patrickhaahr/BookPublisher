namespace Publisher.Contracts.Requests;

public record UpdateBookRequest(
    string? Title,
    DateTime? PublishDate,
    decimal? BasePrice,
    List<string>? Mediums,
    List<string>? Genres,
    List<Guid>? AuthorIds,
    List<CoverUpdateData>? Covers
);

public record CoverUpdateData(
    Guid? CoverId,
    string ImgBase64,
    List<Guid> ArtistIds
);