using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface ICoverRepository
{
    Task<List<Cover>> GetCoversAsync();
    Task<Cover?> GetCoverByIdAsync(Guid id);
    Task<Cover> CreateCoverAsync(Cover cover);
    Task<Cover?> UpdateCoverAsync(Guid id, Cover cover);
    Task<Cover?> DeleteCoverAsync(Guid id);
} 