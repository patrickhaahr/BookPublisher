namespace Publisher.Contracts.Responses;
public record AuthorResponse(
    Guid AuthorPersonId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    decimal RoyaltyRate
);