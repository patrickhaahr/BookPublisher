using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.UserBookInteractions.Commands.CreateUserBookInteraction;

public class CreateUserBookInteractionCommandHandler(
    IUserBookInteractionRepository _userBookInteractionRepository,
    IBookRepository _bookRepository)
    : IRequestHandler<CreateUserBookInteractionCommand, UserBookInteractionResponse>
{
    public async Task<UserBookInteractionResponse> Handle(CreateUserBookInteractionCommand command, CancellationToken token)
    {
        // Check if bookId is a valid Guid first
        Guid bookId;
        Book? book = null;

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
            
            book = await _bookRepository.GetBookBySlugAsync(bookSlug, token)
                ?? throw new NotFoundException("Book", bookSlug);
            bookId = book.BookId;
        }

        // Check if interaction already exists
        var existingInteraction = await _userBookInteractionRepository.GetUserBookInteractionByUserAndBookAsync(
            command.UserId, bookId, token);

        if (existingInteraction is not null)
        {
            // Update existing interaction
            existingInteraction.IsFavorite = command.IsFavorite ?? existingInteraction.IsFavorite;
            existingInteraction.IsSaved = command.IsSaved ?? existingInteraction.IsSaved;
            existingInteraction.Status = command.Status ?? existingInteraction.Status;
            existingInteraction.Rating = command.Rating ?? existingInteraction.Rating;

            var result = await _userBookInteractionRepository.UpdateUserBookInteractionAsync(
                existingInteraction.InteractionId, existingInteraction, token);
                
            return MapToResponse(result!);
        }
        
        // Create new interaction
        var interaction = new UserBookInteraction
        {
            InteractionId = Guid.NewGuid(),
            UserId = command.UserId,
            BookId = bookId,
            IsFavorite = command.IsFavorite ?? false,
            IsSaved = command.IsSaved ?? false,
            Status = command.Status,
            Rating = command.Rating
        };
        
        var createdResult = await _userBookInteractionRepository.CreateUserBookInteractionAsync(interaction, token);
        return MapToResponse(createdResult);
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