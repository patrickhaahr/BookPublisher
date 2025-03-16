using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Commands.UpdateArtist;

public record UpdateArtistCommand(
    Guid Id,
    string FirstName,
    string LastName, 
    string Email, 
    string? Phone, 
    string? PortfolioUrl) : IRequest<Artist>; 