namespace Publisher.Contracts.Requests;

public record UpdateBookArtistsRequest(
    List<Guid> ArtistIds
); 