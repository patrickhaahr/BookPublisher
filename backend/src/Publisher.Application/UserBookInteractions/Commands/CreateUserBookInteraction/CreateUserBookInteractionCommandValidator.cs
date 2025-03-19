using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;

namespace Publisher.Application.UserBookInteractions.Commands.CreateUserBookInteraction;

public class CreateUserBookInteractionCommandValidator : AbstractValidator<CreateUserBookInteractionCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;

    public CreateUserBookInteractionCommandValidator(IUserRepository userRepository, IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _bookRepository = bookRepository;

        RuleFor(c => c.UserId)
            .NotEmpty()
            .Must(Validation.IsValidGuid)
            .MustAsync(async (userId, token) => await _userRepository.GetUserByIdAsync(userId, token) is not null)
            .WithMessage("User must exist in database");

        RuleFor(c => c.BookId)
            .NotEmpty()
            .Must(Validation.IsValidGuid)
            .MustAsync(async (bookId, token) => await _bookRepository.GetBookByIdAsync(bookId, token) is not null)
            .WithMessage("Book must exist in database");
            
        RuleFor(c => c.Status)
            .MaximumLength(50);

        RuleFor(c => c.Rating)
            .InclusiveBetween(1, 10)
            .When(c => c.Rating is not null);
    }
}