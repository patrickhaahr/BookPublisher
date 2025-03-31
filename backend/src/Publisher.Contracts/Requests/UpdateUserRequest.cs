namespace Publisher.Contracts.Requests;

public record UpdateUserRequest(
    string? Username,
    string? Email,
    string? PasswordHash
);
