namespace Publisher.Contracts.Requests;

public record CreateArtistRequest(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? PortfolioUrl
);

