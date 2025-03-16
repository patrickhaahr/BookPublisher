using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Commands.CreateCover;

public record CreateCoverCommand(
    string? ImgBase64,
    Guid BookId) : IRequest<Cover>; 