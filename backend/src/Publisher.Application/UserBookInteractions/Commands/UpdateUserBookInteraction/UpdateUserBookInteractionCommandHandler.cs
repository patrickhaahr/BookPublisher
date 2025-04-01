using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.UserBookInteractions.Commands.UpdateUserBookInteraction;

public class UpdateUserBookInteractionCommandHandler(
    IUserBookInteractionRepository _userBookInteractionRepository,
    IBookRepository _bookRepository)
    : IRequestHandler<UpdateUserBookInteractionCommand, UserBookInteractionResponse>
{
    public async Task<UserBookInteractionResponse> Handle(UpdateUserBookInteractionCommand command, CancellationToken token)
    {
        var interaction = await _userBookInteractionRepository.GetUserBookInteractionByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(UserBookInteraction), command.Id);

        // Process the bookId
        Guid bookId;
        if (command.BookId is Guid guidId)
        {
            bookId = guidId;
        }
        else if (!Guid.TryParse(command.BookId?.ToString() ?? string.Empty, out bookId))
        {
            // If not a valid Guid, treat it as a slug and look up the book
            var bookSlug = command.BookId?.ToString();
            if (string.IsNullOrEmpty(bookSlug))
            {
                throw new ValidationException("BookId", "BookId must be a valid GUID or slug");
            }
            
            var book = await _bookRepository.GetBookBySlugAsync(bookSlug, token)
                ?? throw new NotFoundException("Book", bookSlug);
            bookId = book.BookId;
        }

        interaction.UserId = command.UserId;
        interaction.BookId = bookId;
        interaction.IsFavorite = command.IsFavorite ?? interaction.IsFavorite;
        interaction.IsSaved = command.IsSaved ?? interaction.IsSaved;
        interaction.Status = command.Status ?? interaction.Status;
        interaction.Rating = command.Rating ?? interaction.Rating;

        var updatedInteraction = await _userBookInteractionRepository.UpdateUserBookInteractionAsync(command.Id, interaction, token)
            ?? throw new NotFoundException(nameof(UserBookInteraction), command.Id);
            
        return MapToResponse(updatedInteraction);
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