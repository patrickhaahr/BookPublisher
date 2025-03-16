using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler(IBookRepository bookRepository) 
    : IRequestHandler<UpdateBookCommand, Book>
{
    public async Task<Book> Handle(UpdateBookCommand command, CancellationToken token)
    {
        var book = await bookRepository.GetBookByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(Book), command.Id);

        var newSlug = SlugGenerator.GenerateSlug(command.Title);
        
        // Check if new slug exists and it's not the same book
        if (newSlug != book.Slug && await bookRepository.SlugExistsAsync(newSlug))
            throw new DuplicateSlugException(newSlug);

        book.Title = command.Title;
        book.PublishDate = command.PublishDate;
        book.BasePrice = command.BasePrice;
        book.SetSlug(newSlug);

        var updatedBook = await bookRepository.UpdateBookAsync(command.Id, book);

        return updatedBook is null
            ? throw new NotFoundException(nameof(Book), command.Id)
            : updatedBook;
    }
}