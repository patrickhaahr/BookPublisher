using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionByUserAndBook;

public class GetUserBookInteractionByUserAndBookQueryHandler(
    IUserBookInteractionRepository _userBookInteractionRepository,
    IBookRepository _bookRepository)
    : IRequestHandler<GetUserBookInteractionByUserAndBookQuery, UserBookInteractionResponse?>
{
    public async Task<UserBookInteractionResponse?> Handle(GetUserBookInteractionByUserAndBookQuery query, CancellationToken cancellationToken)
    {
        // First, determine if we have a Guid or a slug
        if (Guid.TryParse(query.BookId, out var bookGuid))
        {
            // It's a valid Guid, use it directly
            var interaction = await _userBookInteractionRepository.GetUserBookInteractionByUserAndBookAsync(
                query.UserId, bookGuid, cancellationToken);
            
            if (interaction is null)
                return null;

            return MapToResponse(interaction);
        }
        else
        {
            // It's a slug, get the book first
            var book = await _bookRepository.GetBookBySlugAsync(query.BookId, cancellationToken);
            if (book is null)
                return null;
                
            var interaction = await _userBookInteractionRepository.GetUserBookInteractionByUserAndBookAsync(
                query.UserId, book.BookId, cancellationToken);
                
            if (interaction is null)
                return null;
                
            return MapToResponse(interaction);
        }
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