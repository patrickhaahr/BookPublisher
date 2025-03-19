using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface ICoverRepository
{
    Task<List<Cover>> GetCoversAsync(CancellationToken token = default);
    Task<Cover?> GetCoverByIdAsync(Guid id, CancellationToken token = default);
    Task<Cover> CreateCoverAsync(Cover cover, CancellationToken token = default);
    Task<Cover?> UpdateCoverAsync(Guid id, Cover cover, CancellationToken token = default);
    Task<Cover?> DeleteCoverAsync(Guid id, CancellationToken token = default);
} 