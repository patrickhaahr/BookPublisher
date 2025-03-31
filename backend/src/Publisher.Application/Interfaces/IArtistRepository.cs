using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IArtistRepository
{
    Task<(List<Artist> Items, int TotalCount, int TotalPages)> GetArtistsAsync(int page, int pageSize, CancellationToken token = default);
    Task<Artist?> GetArtistByIdAsync(Guid id, CancellationToken token = default);
    Task<Artist> CreateArtistAsync(Artist artist, CancellationToken token = default);
    Task<Artist?> UpdateArtistAsync(Guid id, Artist artist, CancellationToken token = default);
    Task<Artist?> DeleteArtistAsync(Guid id, CancellationToken token = default);
} 