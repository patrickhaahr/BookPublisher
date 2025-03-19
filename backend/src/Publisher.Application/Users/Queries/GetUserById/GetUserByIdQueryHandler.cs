using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository _userRepository)
    : IRequestHandler<GetUserByIdQuery, User>
{
    public async Task<User> Handle(GetUserByIdQuery query, CancellationToken token)
    {
        var user = await _userRepository.GetUserByIdAsync(query.Id, token);
        
        return user is null
            ? throw new NotFoundException(nameof(User), query.Id)
            : user;
    }
} 