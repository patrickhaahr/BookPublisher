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
        // Fetch the book by ID or slug
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

        // Update title and slug
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
        // Title
        book.Title = command.Title ?? book.Title;

        // Publish Date
        book.PublishDate = command.PublishDate ?? book.PublishDate;

        // Base Price
        book.BasePrice = command.BasePrice ?? book.BasePrice;

        // Handle Mediums
        if (command.Mediums is not null)
        {
            book.BookMediums.Clear();
            var newMediums = command.Mediums
                .Select(m => new BookMedium
                {   BookId = book.BookId,
                    MediumId = (int)Enum.Parse<MediumEnum>(m, ignoreCase: true) 
                })
                .ToList();
            book.BookMediums.AddRange(newMediums);
        }

        // Handle Genres
        if (command.Genres is not null)
        {
            book.BookGenres.Clear();
            var newGenres = command.Genres
                .Select(g => new BookGenre
                {   BookId = book.BookId,
                    GenreId = (int)Enum.Parse<GenreEnum>(g, ignoreCase: true) 
                })
                .ToList();
            book.BookGenres.AddRange(newGenres);
        }

        // Handle Authors
        if (command.AuthorIds is not null)
        {
            await bookRepository.RemoveBookPersonsAsync(book.BookId, token);
            var newAuthors = command.AuthorIds
                .Select(authorId => new BookPersons
                {   BookId = book.BookId,
                    PersonId = authorId,
                    AuthorPersonId = authorId
                })
                .ToList();
            await bookRepository.AddBookPersonsAsync(newAuthors, token);
        }

        // Handle Covers
        if (command.Covers is not null)
        {
            await bookRepository.RemoveBookCoversAsync(book.BookId, token);
            var updatedCovers = new List<Cover>();
            foreach (var coverRequest in command.Covers)
            {
                var coverId = coverRequest.CoverId ?? Guid.NewGuid();
                var cover = new Cover
                {
                    CoverId = coverId,
                    BookId = book.BookId,
                    ImgBase64 = coverRequest.ImgBase64,
                    CreatedDate = DateTime.UtcNow,
                };
                await bookRepository.AddBookCoverAsync(cover, token);

                // Artists
                var coverPersons = coverRequest.ArtistIds
                    .Select(artistId => new CoverPersons
                    {
                        CoverId = coverId,
                        PersonId = artistId,
                        ArtistPersonId = artistId
                    })
                    .ToList();
                await bookRepository.AddCoverPersonsAsync(coverPersons, token);

                updatedCovers.Add(cover);
            }
            book.Covers = updatedCovers;
        }

        // Save changes
        await bookRepository.UpdateBookAsync(book, token);

        // Fetch the updated book
        var updatedBook = await bookRepository.GetBookByIdAsync(book.BookId, token)
            ?? throw new NotFoundException(nameof(Book), command.IdOrSlug);
        
        // Build response
        return new UpdateBookResponse(
            updatedBook.BookId,
            updatedBook.Title,
            updatedBook.PublishDate,
            updatedBook.BasePrice,
            updatedBook.Slug,
            [.. updatedBook.BookMediums.Select(m => m.Medium?.Name ?? "Unknown")],
            [.. updatedBook.BookGenres.Select(g => g.Genre?.Name ?? "Unknown")],
            [.. updatedBook.BookPersons.Select(p => p.AuthorPersonId)],
            [.. updatedBook.Covers.Select(c => new CoverResponseData(
                c.CoverId,
                c.ImgBase64 ?? string.Empty,
                [.. c.CoverPersons.Select(p => p.ArtistPersonId)]
            ))]
        );
    }
}