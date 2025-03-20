using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.UpdateBookArtists;

public class UpdateBookArtistsCommandHandler(IBookRepository bookRepository)
    : IRequestHandler<UpdateBookArtistsCommand, UpdateBookArtistsResponse>
{
    public async Task<UpdateBookArtistsResponse> Handle(UpdateBookArtistsCommand command, CancellationToken token)
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

        if (book == null)
            throw new NotFoundException(nameof(Book), command.IdOrSlug);

        // Update the artists for all covers of this book
        foreach (var cover in book.Covers)
        {
            // Remove existing cover artists
            await bookRepository.RemoveCoverPersonsAsync(cover.CoverId);
            
            // Add new cover artists
            var coverPersons = command.ArtistIds
                .Select(artistId => new CoverPersons
                {
                    CoverId = cover.CoverId,
                    PersonId = artistId,
                    ArtistPersonId = artistId
                })
                .ToList();
                
            await bookRepository.AddCoverPersonsAsync(coverPersons);
        }

        return new UpdateBookArtistsResponse(
            bookId,
            command.ArtistIds
        );
    }
} 