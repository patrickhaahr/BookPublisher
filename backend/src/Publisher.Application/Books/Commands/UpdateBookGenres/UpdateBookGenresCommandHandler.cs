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
        // Verify book exists
        var book = await bookRepository.GetBookByIdAsync(command.BookId);

        if (book is null)
            throw new NotFoundException(nameof(Book), command.BookId);

        // Remove existing genres
        await bookRepository.RemoveBookGenresAsync(command.BookId);

        // Add new genres
        var bookGenres = command.GenreIds.Select(gid => new BookGenres
        {
            BookId = command.BookId,
            GenreId = gid
        }).ToList();

        await bookRepository.AddBookGenresAsync(bookGenres);

        return new UpdateBookGenresResponse(command.BookId, command.GenreIds);
    }
}
