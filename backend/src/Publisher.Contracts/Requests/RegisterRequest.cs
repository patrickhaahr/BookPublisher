namespace Publisher.Contracts.Requests;

public record RegisterRequest(
    string Username,
    string Email,
    string Password,
    string Role
);

