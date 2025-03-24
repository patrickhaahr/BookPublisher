namespace Publisher.Contracts.Responses;

public record AuthenticationResponse(
    Guid UserId,
    string Username,
    string Email,
    string AccessToken,
    string RefreshToken);
