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
        // Verify book exists
        var book = await bookRepository.GetBookByIdAsync(command.BookId) 
            ?? throw new NotFoundException(nameof(Book), command.BookId);

        // This is a little more complex as artists are associated with covers
        // First, get all covers for this book
        var bookCovers = await bookRepository.GetBookByIdAsync(command.BookId);
        if (bookCovers?.Covers == null || !bookCovers.Covers.Any())
        {
            // No covers to associate artists with
            return new UpdateBookArtistsResponse(command.BookId, new List<Guid>());
        }

        // For each cover, remove existing artist associations and add new ones
        foreach (var cover in bookCovers.Covers)
        {
            // Remove existing artist associations
            await bookRepository.RemoveCoverPersonsAsync(cover.CoverId);

            // Add new artist associations
            var coverPersons = command.ArtistIds.Select(artistId => new CoverPersons
            {
                CoverId = cover.CoverId,
                PersonId = artistId,
                ArtistPersonId = artistId
            }).ToList();

            if (coverPersons.Any())
            {
                await bookRepository.AddCoverPersonsAsync(coverPersons);
            }
        }

        return new UpdateBookArtistsResponse(command.BookId, command.ArtistIds);
    }
} 