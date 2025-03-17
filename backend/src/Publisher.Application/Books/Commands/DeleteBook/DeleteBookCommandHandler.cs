using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler(IBookRepository bookRepository) 
    : IRequestHandler<DeleteBookCommand, DeleteBookResponse>
{
    public async Task<DeleteBookResponse> Handle(DeleteBookCommand command, CancellationToken token)
    {
        var deletedBook = await bookRepository.DeleteBookAsync(command.Id);

        if (deletedBook is null)
            throw new NotFoundException(nameof(Book), command.Id);
            
        return new DeleteBookResponse(
            deletedBook.BookId,
            deletedBook.Title,
            deletedBook.Slug
        );
    }
}