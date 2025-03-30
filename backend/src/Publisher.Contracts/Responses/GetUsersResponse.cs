namespace Publisher.Contracts.Responses;

public record GetUsersResponse(
    Guid UserId,
    string Username,
    string Email,
    string Role);
