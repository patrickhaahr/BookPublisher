using FluentValidation;

namespace Publisher.Application.Genres.Queries.GetGenreById;

public class GetGenreByIdQueryValidator : AbstractValidator<GetGenreByIdQuery>
{
    public GetGenreByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .GreaterThan(0);
    }
}
