namespace Publisher.Application.Interfaces;

public interface ICurrentUserService
{
    Guid GetCurrentUserId();
    string? Role { get; }
} 