using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Commands.UpdateBookCovers;

public class UpdateBookCoversCommandHandler(IBookRepository bookRepository)
    : IRequestHandler<UpdateBookCoversCommand, UpdateBookCoversResponse>
{
    public async Task<UpdateBookCoversResponse> Handle(UpdateBookCoversCommand command, CancellationToken token)
    {
        // Verify book exists
        var book = await bookRepository.GetBookByIdAsync(command.BookId) 
            ?? throw new NotFoundException(nameof(Book), command.BookId);

        // Remove all existing covers and their artists
        await bookRepository.RemoveBookCoversAsync(command.BookId);

        // Add or update covers
        var updatedCovers = new List<Cover>();
        foreach (var coverRequest in command.Covers)
        {
            var coverId = coverRequest.CoverId ?? Guid.NewGuid();
            var cover = new Cover
            {
                CoverId = coverId,
                BookId = command.BookId,
                ImgBase64 = coverRequest.ImgBase64,
                CreatedDate = DateTime.UtcNow
            };
            
            // Save the cover first to ensure CoverId exists
            await bookRepository.AddBookCoverAsync(cover);

            var coverPersons = coverRequest.ArtistIds.Select(artistId => new CoverPersons
            {
                CoverId = cover.CoverId, // Use the saved CoverId
                PersonId = artistId,
                ArtistPersonId = artistId
            }).ToList();
            await bookRepository.AddCoverPersonsAsync(coverPersons);

            updatedCovers.Add(cover);
        }

        book.Covers = updatedCovers; // Update the book's cover collection
        await bookRepository.UpdateBookAsync(book);

        return new UpdateBookCoversResponse(command.BookId, command.Covers);
    }
} 