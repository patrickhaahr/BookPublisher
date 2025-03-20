using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.UpdateBookGenres;

public class UpdateBookGenresCommandHandler(IBookRepository bookRepository)
    : IRequestHandler<UpdateBookGenresCommand, UpdateBookGenresResponse>
{
    public async Task<UpdateBookGenresResponse> Handle(UpdateBookGenresCommand command, CancellationToken token)
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

        // Remove all existing genres
        await bookRepository.RemoveBookGenresAsync(bookId, token);

        // Add new genres
        var bookGenres = command.GenreIds
            .Select(genreId => new BookGenres
            {
                BookId = bookId,
                GenreId = genreId
            })
            .ToList();

        await bookRepository.AddBookGenresAsync(bookGenres, token);

        // Reload the book to get the updated genres
        book = await bookRepository.GetBookByIdAsync(bookId, token)
            ?? throw new NotFoundException(nameof(Book), command.IdOrSlug);

        return new UpdateBookGenresResponse(
            book.BookId,
            book.BookGenres.Select(bg => bg.GenreId).ToList()
        );
    }
}
