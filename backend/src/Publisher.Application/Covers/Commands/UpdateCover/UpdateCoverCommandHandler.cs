using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Covers.Commands.UpdateCover;

public class UpdateCoverCommandHandler(ICoverRepository _coverRepository)
    : IRequestHandler<UpdateCoverCommand, Cover>
{
    public async Task<Cover> Handle(UpdateCoverCommand command, CancellationToken token)
    {
        var cover = await _coverRepository.GetCoverByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(Cover), command.Id);

        cover.ImgBase64 = command.ImgBase64;
        cover.BookId = command.BookId;

        var updatedCover = await _coverRepository.UpdateCoverAsync(command.Id, cover);

        return updatedCover is null
            ? throw new NotFoundException(nameof(Cover), command.Id)
            : updatedCover;
    }
} 