using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Commands.DeleteArtist;

public record DeleteArtistCommand(Guid Id) : IRequest<Artist>; 