using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.UpdateBookAuthors;

public class UpdateBookAuthorsCommandHandler(IBookRepository bookRepository)
    : IRequestHandler<UpdateBookAuthorsCommand, UpdateBookAuthorsResponse>
{
    public async Task<UpdateBookAuthorsResponse> Handle(UpdateBookAuthorsCommand command, CancellationToken token)
    {
        // Resolve the book using ID or slug
        Book? book;
        Guid bookId;
        
        if (Guid.TryParse(command.IdOrSlug, out bookId))
        {
            book = await bookRepository.GetBookByIdAsync(bookId, token);
        }
        else
        {
            book = await bookRepository.GetBookBySlugAsync(command.IdOrSlug, token);
            if (book != null)
            {
                bookId = book.BookId;
            }
            else
            {
                throw new NotFoundException(nameof(Book), command.IdOrSlug);
            }
        }

        if (book == null)
            throw new NotFoundException(nameof(Book), command.IdOrSlug);

        // Remove all existing authors
        await bookRepository.RemoveBookPersonsAsync(bookId, token);

        // Add new authors
        var bookPersons = command.AuthorIds
            .Select(authorId => new BookPersons
            {
                BookId = bookId,
                PersonId = authorId,
                AuthorPersonId = authorId
            })
            .ToList();

        await bookRepository.AddBookPersonsAsync(bookPersons, token);

        return new UpdateBookAuthorsResponse(
            bookId,
            command.AuthorIds
        );
    }
} 