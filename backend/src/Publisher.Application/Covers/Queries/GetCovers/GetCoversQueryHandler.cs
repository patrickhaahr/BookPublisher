using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Queries.GetCovers;

public class GetCoversQueryHandler(ICoverRepository _coverRepository)
    : IRequestHandler<GetCoversQuery, List<Cover>>
{
    public async Task<List<Cover>> Handle(GetCoversQuery request, CancellationToken cancellationToken)
    {
        return await _coverRepository.GetCoversAsync();
    }
} 