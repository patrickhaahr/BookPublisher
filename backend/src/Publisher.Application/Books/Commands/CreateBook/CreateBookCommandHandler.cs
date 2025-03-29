using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;
namespace Publisher.Application.Books.Commands.CreateBook;

public class CreateBookCommandHandler(IBookRepository bookRepository) 
    : IRequestHandler<CreateBookCommand, CreateBookResponse>
{
    public async Task<CreateBookResponse> Handle(CreateBookCommand command, CancellationToken token)
    {
        // Generate slug from title
        var baseSlug = SlugGenerator.GenerateSlug(command.Title);
        
        // Find a unique slug by appending number if needed
        var slug = baseSlug;
        var slugAttempt = 1;
        
        while (await bookRepository.SlugExistsAsync(slug, token))
        {
            slugAttempt++;
            slug = SlugGenerator.MakeSlugUnique(baseSlug, slugAttempt);
        }

        var book = new Book
        {
            BookId = Guid.NewGuid(),
            Title = command.Title,
            PublishDate = command.PublishDate,
            BasePrice = command.BasePrice,
            BookMediums = [.. command.Mediums.Select(m => new BookMedium { MediumId = (int)Enum.Parse<MediumEnum>(m, ignoreCase: true) })], // Map strings to enum
            BookGenres = [.. command.Genres.Select(g => new BookGenre { GenreId = (int)Enum.Parse<GenreEnum>(g, ignoreCase: true) })], // Map strings to enum
        };
        book.SetSlug(slug);

        // Collection expression
        // Instead of using .ToList()

        // Add covers and artists
        book.Covers = [.. command.Covers.Select(coverRequest => {
            var coverId = Guid.NewGuid();
            return new Cover
            {
                CoverId = coverId,
                BookId = book.BookId,
                ImgBase64 = coverRequest.ImgBase64,
                CreatedDate = DateTime.UtcNow,
                CoverPersons = [.. coverRequest.ArtistIds.Select(artistId => new CoverPersons
                {
                    CoverId = coverId,
                    PersonId = artistId,
                    ArtistPersonId = artistId
                })]
            };
        })];

        // Add authors
        book.BookPersons = [.. command.AuthorIds.Select(authorId => new BookPersons
        {
            BookId = book.BookId, 
            PersonId = authorId,
            AuthorPersonId = authorId
        })];

        var createdBook = await bookRepository.CreateBookAsync(book, token);

        return new CreateBookResponse(
            createdBook.BookId,
            createdBook.Title,
            createdBook.Slug
        );
    }
}
