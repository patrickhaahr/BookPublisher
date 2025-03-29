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
        // Resolve the book using ID or slug, including existing mediums and genres
        Book? book;
        if (Guid.TryParse(command.IdOrSlug, out var bookId))
        {
            // Include BookMediums and BookGenres when fetching
            book = await bookRepository.GetBookByIdAsync(bookId, token); 
        }
        else
        {
             // Include BookMediums and BookGenres when fetching
            book = await bookRepository.GetBookBySlugAsync(command.IdOrSlug, token);
        }

        if (book is null)
            throw new NotFoundException(nameof(Book), command.IdOrSlug);

        var baseSlug = SlugGenerator.GenerateSlug(command.Title ?? book.Title);
        
        var slug = baseSlug;
        var slugAttempt = 1;
        
        // If the title has changed or the slug is different from the base slug
        if (baseSlug != book.Slug)
        {
            // Find a unique slug
            while (await bookRepository.SlugExistsAsync(slug, token))
            {
                slugAttempt++;
                slug = SlugGenerator.MakeSlugUnique(baseSlug, slugAttempt);
            }
            book.SetSlug(slug);
        }

        book.Title = command.Title ?? book.Title;
        book.PublishDate = command.PublishDate ?? book.PublishDate;
        book.BasePrice = command.BasePrice ?? book.BasePrice;

        if (command.Mediums is not null)
        {
            book.BookMediums.Clear();
            var newMediums = command.Mediums
                .Select(m => new BookMedium { BookId = book.BookId, MediumId = (int)Enum.Parse<MediumEnum>(m, ignoreCase: true) })
                .ToList();
            foreach(var bm in newMediums) book.BookMediums.Add(bm);
        }

        if (command.Genres is not null)
        {
            book.BookGenres.Clear();
            var newGenres = command.Genres
                .Select(g => new BookGenre { BookId = book.BookId, GenreId = (int)Enum.Parse<GenreEnum>(g, ignoreCase: true) })
                .ToList();
            foreach(var bg in newGenres) book.BookGenres.Add(bg);
        }

        await bookRepository.UpdateBookAsync(book, token);

        var updatedBook = await bookRepository.GetBookByIdAsync(book.BookId, token);

        if (updatedBook is null)
             throw new NotFoundException(nameof(Book), command.IdOrSlug);
            
        return new UpdateBookResponse(
            updatedBook.BookId,
            updatedBook.Title,
            updatedBook.PublishDate,
            updatedBook.BasePrice,
            updatedBook.Slug,
            [.. updatedBook.BookMediums.Select(m => m.Medium?.Name ?? "Unknown")],
            [.. updatedBook.BookGenres.Select(g => g.Genre?.Name ?? "Unknown")]
        );
    }
}