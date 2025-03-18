namespace Publisher.Contracts.Responses;

public record UpdateBookArtistsResponse(
    Guid BookId,
    List<Guid> ArtistIds
); 