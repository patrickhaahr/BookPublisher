namespace Publisher.Contracts.Responses;

public record UpdateUserResponse(
    Guid UserId,
    string Username,
    string Email,
    string Role
);
