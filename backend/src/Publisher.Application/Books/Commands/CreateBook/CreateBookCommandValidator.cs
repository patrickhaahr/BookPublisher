using FluentValidation;

namespace Publisher.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}
