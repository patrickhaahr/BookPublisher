using FluentValidation;
using Publisher.Domain.Entities;

namespace Publisher.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
         RuleFor(c => c.Title)
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters");

        RuleFor(c => c.PublishDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Publish date cannot be in the future");

        RuleFor(c => c.BasePrice)
            .GreaterThan(0)
            .WithMessage("Base price must be greater than 0");

        RuleFor(c => c.Mediums)
            .Must(mediums => mediums is null || mediums.All(m => Enum.TryParse<Medium>(m, ignoreCase: true, out _)))
            .WithMessage("All mediums must be valid enum values (e.g., 'Print', 'EBook')");
        
        RuleFor(c => c.Genres)
            .Must(genres => genres is null || genres.All(g => Enum.TryParse<Genre>(g, ignoreCase: true, out _)))
            .WithMessage("All genres must be valid enum values (e.g., 'ScienceFiction', 'Mystery')");
    }
}