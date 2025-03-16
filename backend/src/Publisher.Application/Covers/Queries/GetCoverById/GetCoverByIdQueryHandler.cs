using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Covers.Queries.GetCoverById;

public class GetCoverByIdQueryHandler(ICoverRepository _coverRepository)
    : IRequestHandler<GetCoverByIdQuery, Cover>
{
    public async Task<Cover> Handle(GetCoverByIdQuery query, CancellationToken cancellationToken)
    {
        var cover = await _coverRepository.GetCoverByIdAsync(query.Id);
        
        return cover is null
            ? throw new NotFoundException(nameof(Cover), query.Id)
            : cover;
    }
} 