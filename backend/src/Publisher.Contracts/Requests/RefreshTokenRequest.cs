namespace Publisher.Contracts.Requests;

public record RefreshTokenRequest(
    Guid UserId,
    string RefreshToken
);


