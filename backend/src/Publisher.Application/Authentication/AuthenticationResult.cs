using Publisher.Domain.Entities;

namespace Publisher.Application.Authentication;

public record AuthenticationResult(
    User User,
    string AccessToken,
    string RefreshToken);