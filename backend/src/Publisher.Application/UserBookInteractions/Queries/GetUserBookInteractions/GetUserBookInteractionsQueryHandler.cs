using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractions;

public class GetUserBookInteractionsQueryHandler(IUserBookInteractionRepository _userBookInteractionRepository)
    : IRequestHandler<GetUserBookInteractionsQuery, List<UserBookInteraction>>
{
    public async Task<List<UserBookInteraction>> Handle(GetUserBookInteractionsQuery request, CancellationToken cancellationToken)
    {
        return await _userBookInteractionRepository.GetUserBookInteractionsAsync();
    }
} 