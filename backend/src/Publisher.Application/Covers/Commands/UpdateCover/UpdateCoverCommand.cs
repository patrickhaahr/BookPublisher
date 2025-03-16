using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Commands.UpdateCover;

public record UpdateCoverCommand(
    Guid Id,
    string? ImgBase64,
    Guid BookId) : IRequest<Cover>; 