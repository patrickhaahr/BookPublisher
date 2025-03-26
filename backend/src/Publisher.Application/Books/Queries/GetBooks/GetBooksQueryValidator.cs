using FluentValidation;

namespace Publisher.Application.Books.Queries.GetBooks;

public class GetBooksQueryValidator : AbstractValidator<GetBooksQuery>
{
    public GetBooksQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}

