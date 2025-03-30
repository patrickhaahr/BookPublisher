namespace Publisher.Contracts.Responses;

public record UpdateBookResponse(
    Guid BookId,
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    string Slug,
    List<string> Mediums,
    List<string> Genres,
    List<Guid> AuthorIds,
    List<CoverResponseData> Covers
);

public record CoverResponseData(
    Guid CoverId,
    string ImgBase64,
    List<Guid> ArtistIds
);