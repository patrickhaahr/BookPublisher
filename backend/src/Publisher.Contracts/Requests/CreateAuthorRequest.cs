namespace Publisher.Contracts.Requests;

public record CreateAuthorRequest(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    decimal RoyaltyRate
);
