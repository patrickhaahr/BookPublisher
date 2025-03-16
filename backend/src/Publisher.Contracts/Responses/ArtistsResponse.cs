namespace Publisher.Contracts.Responses;

public record ArtistResponse(
    Guid ArtistPersonId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string PortfolioUrl
);