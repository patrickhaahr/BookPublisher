using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IArtistRepository
{
    Task<List<Artist>> GetArtistsAsync();
    Task<Artist?> GetArtistByIdAsync(Guid id);
    Task<Artist> CreateArtistAsync(Artist artist);
    Task<Artist?> UpdateArtistAsync(Guid id, Artist artist);
    Task<Artist?> DeleteArtistAsync(Guid id);
} 