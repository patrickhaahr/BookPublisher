using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;

namespace Publisher.Application.UserBookInteractions.Commands.UpdateUserBookInteraction;

public class UpdateUserBookInteractionCommandValidator : AbstractValidator<UpdateUserBookInteractionCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;

    public UpdateUserBookInteractionCommandValidator(IUserRepository userRepository, IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _bookRepository = bookRepository;

        // CHECK IF THE GUID IS IN DATABASE, ELSE RETURN ERROR
        RuleFor(c => c.UserId)
            .NotEmpty()
            .Must(Validation.IsValidGuid)
            .MustAsync(async (userId, token) => await _userRepository.GetUserByIdAsync(userId, token) is not null)
            .WithMessage("User must exist in database");

        RuleFor(c => c.BookId)
            .NotEmpty()
            .MustAsync(async (bookId, token) => {
                // If it's a Guid, check directly
                if (bookId is Guid guidId)
                {
                    return await _bookRepository.GetBookByIdAsync(guidId, token) is not null;
                }
                
                // If it's a string, try parsing as Guid first
                var bookIdStr = bookId?.ToString();
                if (string.IsNullOrEmpty(bookIdStr))
                {
                    return false;
                }
                
                if (Guid.TryParse(bookIdStr, out var parsedGuid))
                {
                    return await _bookRepository.GetBookByIdAsync(parsedGuid, token) is not null;
                }
                
                // Otherwise, treat as slug
                return await _bookRepository.GetBookBySlugAsync(bookIdStr, token) is not null;
            })
            .WithMessage("Book must exist in database");

        RuleFor(c => c.Status)
            .MaximumLength(50);

        RuleFor(c => c.Rating)
            .InclusiveBetween(1, 10)
            .When(c => c.Rating is not null);
    }
}

