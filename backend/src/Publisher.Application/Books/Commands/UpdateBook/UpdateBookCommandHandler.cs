using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler(IBookRepository bookRepository) 
    : IRequestHandler<UpdateBookCommand, UpdateBookResponse>
{
    public async Task<UpdateBookResponse> Handle(UpdateBookCommand command, CancellationToken token)
    {
        // Resolve the book using ID or slug
        Book? book;
        if (Guid.TryParse(command.IdOrSlug, out var bookId))
        {
            book = await bookRepository.GetBookByIdAsync(bookId, token);
        }
        else
        {
            book = await bookRepository.GetBookBySlugAsync(command.IdOrSlug, token);
        }

        if (book == null)
            throw new NotFoundException(nameof(Book), command.IdOrSlug);

        var baseSlug = SlugGenerator.GenerateSlug(command.Title ?? book.Title);
        
        var slug = baseSlug;
        var slugAttempt = 1;
        
        // If the title has changed or the slug is different from the base slug
        if (baseSlug != book.Slug)
        {
            // Find a unique slug
            while (await bookRepository.SlugExistsAsync(slug, token) && slug != book.Slug)
            {
                slugAttempt++;
                slug = SlugGenerator.MakeSlugUnique(baseSlug, slugAttempt);
            }
        }
        else
        {
            // Keep the existing slug if title hasn't changed
            slug = book.Slug;
        }

        book.Title = command.Title ?? book.Title;
        book.PublishDate = command.PublishDate ?? book.PublishDate;
        book.BasePrice = command.BasePrice ?? book.BasePrice;
        book.BookMediums = command.Mediums?.Select(m => new BookMedium { MediumId = (int)Enum.Parse<MediumEnum>(m, ignoreCase: true) }).ToList() ?? book.BookMediums;
        book.BookGenres = command.Genres?.Select(g => new BookGenre { GenreId = (int)Enum.Parse<GenreEnum>(g, ignoreCase: true) }).ToList() ?? book.BookGenres;
        book.SetSlug(slug);

        var updatedBook = await bookRepository.UpdateBookAsync(book.BookId, book, token);

        if (updatedBook is null)
            throw new NotFoundException(nameof(Book), command.IdOrSlug);
            
        return new UpdateBookResponse(
            updatedBook.BookId,
            updatedBook.Title,
            updatedBook.PublishDate,
            updatedBook.BasePrice,
            updatedBook.Slug,
            [.. updatedBook.BookMediums.Select(m => m.Medium.Name)],
            [.. updatedBook.BookGenres.Select(g => g.Genre.Name)]
        );
    }
}