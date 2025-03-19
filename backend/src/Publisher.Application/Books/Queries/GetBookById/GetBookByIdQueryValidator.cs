using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Queries.GetBookById;

public class GetBookByIdQueryValidator : AbstractValidator<GetBookByIdQuery>
{
    public GetBookByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty()
            .WithMessage("Book ID is required")
            .Must(Validation.IsValidGuid)
            .WithMessage("Book ID must be a valid GUID");
    }
}