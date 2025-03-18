using System.ComponentModel.DataAnnotations;
using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.CreateBook;

public class CreateBookCommandHandler(IBookRepository bookRepository) 
    : IRequestHandler<CreateBookCommand, CreateBookResponse>
{
    public async Task<CreateBookResponse> Handle(CreateBookCommand command, CancellationToken token)
    {
        // Validate command
        var validator = new CreateBookCommandValidator();

        var validationResult = await validator.ValidateAsync(command, token);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        // Generate slug from title
        var slug = SlugGenerator.GenerateSlug(command.Title);
        
        // Check if slug exists
        if (await bookRepository.SlugExistsAsync(slug)) 
            throw new DuplicateSlugException(slug);

        var book = new Book
        {
            BookId = Guid.NewGuid(),
            Title = command.Title,
            PublishDate = command.PublishDate,
            BasePrice = command.BasePrice
        };
        book.SetSlug(slug);

        // Collection expression
        // Instead of using .ToList()

        // Add covers and artists
        book.Covers = [.. command.Covers.Select(coverRequest => {
            var coverId = Guid.NewGuid();
            return new Cover
            {
                CoverId = coverId,
                BookId = book.BookId,
                ImgBase64 = coverRequest.ImgBase64,
                CreatedDate = DateTime.UtcNow,
                CoverPersons = [.. coverRequest.ArtistIds.Select(artistId => new CoverPersons
                {
                    CoverId = coverId,
                    PersonId = artistId,
                    ArtistPersonId = artistId
                })]
            };
        })];

        // Add genres
        book.BookGenres = [.. command.GenreIds.Select(gid => new BookGenres
        {
            BookId = book.BookId,
            GenreId = gid
        })];

        // Add authors
        book.BookPersons = [.. command.AuthorIds.Select(aid => new BookPersons
        {
            BookId = book.BookId,
            PersonId = aid,
            AuthorPersonId = aid
        })];

        // Save the book to the database
        await bookRepository.CreateBookAsync(book);

        return new CreateBookResponse(
            book.BookId,
            book.Title,
            book.Slug
        );
    }
}
