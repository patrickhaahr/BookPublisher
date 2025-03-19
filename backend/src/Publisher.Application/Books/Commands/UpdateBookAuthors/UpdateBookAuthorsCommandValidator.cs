using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBookAuthors;

public class UpdateBookAuthorsCommandValidator : AbstractValidator<UpdateBookAuthorsCommand>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateBookAuthorsCommandValidator(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;

        RuleFor(c => c.AuthorIds)
            .NotEmpty()
            .WithMessage("At least one author is required")
            .Must(authorIds => authorIds.All(Validation.IsValidGuid))
            .WithMessage("Author IDs must be valid GUIDs")
            .MustAsync(async (authorIds, token) => 
                await Validation.AllEntitiesExistAsync(authorIds, _authorRepository.GetAuthorByIdAsync, token))
            .WithMessage("All author IDs must exist in the database");
    }
}