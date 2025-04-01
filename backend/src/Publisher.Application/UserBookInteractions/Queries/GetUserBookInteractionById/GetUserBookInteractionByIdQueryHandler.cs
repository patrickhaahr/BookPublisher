using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionById;

public class GetUserBookInteractionByIdQueryHandler(IUserBookInteractionRepository _userBookInteractionRepository)
    : IRequestHandler<GetUserBookInteractionByIdQuery, UserBookInteractionResponse>
{
    public async Task<UserBookInteractionResponse> Handle(GetUserBookInteractionByIdQuery query, CancellationToken cancellationToken)
    {
        var interaction = await _userBookInteractionRepository.GetUserBookInteractionByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(UserBookInteraction), query.Id);

        return MapToResponse(interaction);
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