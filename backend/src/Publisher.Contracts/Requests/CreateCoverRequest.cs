namespace Publisher.Contracts.Requests;

public record CreateCoverRequest(
    string ImgBase64, 
    List<Guid> ArtistIds
);
