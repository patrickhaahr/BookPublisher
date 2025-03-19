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
        // Verify book exists
        var book = await bookRepository.GetBookByIdAsync(command.BookId, token) 
            ?? throw new NotFoundException(nameof(Book), command.BookId);

        // Remove existing book authors relationships
        await bookRepository.RemoveBookPersonsAsync(command.BookId);

        // Add new book authors relationships
        var bookPersons = command.AuthorIds.Select(authorId => new BookPersons
        {
            BookId = command.BookId,
            PersonId = authorId,
            AuthorPersonId = authorId
        }).ToList();

        await bookRepository.AddBookPersonsAsync(bookPersons);

        return new UpdateBookAuthorsResponse(command.BookId, command.AuthorIds);
    }
} 