using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Commands.CreateCover;

public class CreateCoverCommandHandler(ICoverRepository _coverRepository)
    : IRequestHandler<CreateCoverCommand, Cover>
{
    public async Task<Cover> Handle(CreateCoverCommand command, CancellationToken token)
    {
        var cover = new Cover
        {
            CoverId = Guid.NewGuid(),
            ImgBase64 = command.ImgBase64,
            BookId = command.BookId,
            CreatedDate = DateTime.UtcNow
        };
        return await _coverRepository.CreateCoverAsync(cover);
    }
} 