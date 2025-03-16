using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Covers.Commands.DeleteCover;

public class DeleteCoverCommandHandler(ICoverRepository _coverRepository)
    : IRequestHandler<DeleteCoverCommand, Cover>
{
    public async Task<Cover> Handle(DeleteCoverCommand command, CancellationToken token)
    {
        var deletedCover = await _coverRepository.DeleteCoverAsync(command.Id);

        return deletedCover is null
            ? throw new NotFoundException(nameof(Cover), command.Id)
            : deletedCover;
    }
} 