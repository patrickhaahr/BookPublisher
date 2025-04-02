namespace Publisher.Contracts.Requests;

public record UpdateCoverData(
    Guid? CoverId,
    string ImgBase64,
    List<Guid> ArtistIds
);
