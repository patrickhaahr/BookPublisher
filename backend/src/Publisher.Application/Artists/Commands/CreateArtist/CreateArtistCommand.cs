using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Commands.CreateArtist;

public record CreateArtistCommand(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? PortfolioUrl) : IRequest<Artist>; 