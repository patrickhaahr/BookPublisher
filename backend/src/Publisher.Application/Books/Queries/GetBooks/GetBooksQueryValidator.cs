using System.Security.Cryptography.X509Certificates;
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

        RuleFor(x => x.Year)
            .Matches(@"^\d{1,4}$")
            .Must(year => year == null || (int.TryParse(year, out int y) && y <= DateTime.UtcNow.Year))
            .WithMessage($"Year must be a number between 1 and {DateTime.UtcNow.Year}.");
    }
}

