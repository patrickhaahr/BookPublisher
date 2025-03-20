using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler(IBookRepository bookRepository) 
    : IRequestHandler<DeleteBookCommand, DeleteResponse>
{
    public async Task<DeleteResponse> Handle(DeleteBookCommand command, CancellationToken token)
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

        var deletedBook = await bookRepository.DeleteBookAsync(bookId, token);

        if (deletedBook is null)
            throw new NotFoundException(nameof(Book), command.IdOrSlug);
            
        return new DeleteResponse();
    }
}