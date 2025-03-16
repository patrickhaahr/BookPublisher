using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionById;

public class GetUserBookInteractionByIdQueryHandler(IUserBookInteractionRepository _userBookInteractionRepository)
    : IRequestHandler<GetUserBookInteractionByIdQuery, UserBookInteraction>
{
    public async Task<UserBookInteraction> Handle(GetUserBookInteractionByIdQuery query, CancellationToken cancellationToken)
    {
        var interaction = await _userBookInteractionRepository.GetUserBookInteractionByIdAsync(query.Id);
        
        return interaction is null
            ? throw new NotFoundException(nameof(UserBookInteraction), query.Id)
            : interaction;
    }
} 