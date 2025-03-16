namespace Publisher.Contracts.Responses;

public record CoverResponse(
    Guid CoverId,
    string ImgBase64,
    DateTime CreatedDate,
    List<ArtistResponse> Artists
);
