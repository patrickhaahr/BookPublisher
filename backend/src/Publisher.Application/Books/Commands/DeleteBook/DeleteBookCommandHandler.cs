using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler(IBookRepository bookRepository) 
    : IRequestHandler<DeleteBookCommand, Book>
{
    public async Task<Book> Handle(DeleteBookCommand command, CancellationToken token)
    {
        var deletedBook = await bookRepository.DeleteBookAsync(command.Id);

        return deletedBook is null 
            ? throw new NotFoundException(nameof(Book), command.Id)
            : deletedBook;
    }
}