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

        return new CreateBookResponse(
            book.BookId,
            book.Title,
            book.Slug
        );
    }
}
