namespace Publisher.Contracts.Requests;

public record UpdateBookCoversRequest(
    List<CoverUpdateData> Covers
);

public record CoverUpdateData(
    Guid? CoverId,
    string ImgBase64,
    List<Guid> ArtistIds
);