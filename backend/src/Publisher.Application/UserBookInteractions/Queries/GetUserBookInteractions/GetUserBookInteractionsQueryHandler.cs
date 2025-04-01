using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractions;

public class GetUserBookInteractionsQueryHandler(IUserBookInteractionRepository _userBookInteractionRepository)
    : IRequestHandler<GetUserBookInteractionsQuery, List<UserBookInteractionResponse>>
{
    public async Task<List<UserBookInteractionResponse>> Handle(GetUserBookInteractionsQuery request, CancellationToken cancellationToken)
    {
        var interactions = await _userBookInteractionRepository.GetUserBookInteractionsAsync(cancellationToken);
        var result = new List<UserBookInteractionResponse>();
        
        foreach (var interaction in interactions)
        {
            result.Add(MapToResponse(interaction));
        }
        
        return result;
    }

    private static UserBookInteractionResponse MapToResponse(UserBookInteraction interaction)
    {
        return new UserBookInteractionResponse(
            interaction.InteractionId,
            interaction.UserId,
            interaction.BookId,
            interaction.IsFavorite,
            interaction.IsSaved,
            interaction.Status,
            interaction.Rating,
            interaction.User is not null ? new UserBriefResponse(
                interaction.User.UserId,
                interaction.User.Username,
                interaction.User.Email
            ) : null,
            interaction.Book is not null ? new BookBriefResponse(
                interaction.Book.BookId,
                interaction.Book.Title,
                interaction.Book.Slug
            ) : null
        );
    }
} 