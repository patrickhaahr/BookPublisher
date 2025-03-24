using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}

