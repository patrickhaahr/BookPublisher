using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Artists.Commands.CreateArtist;

public record CreateArtistCommand(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? PortfolioUrl) : IRequest<ArtistResponse>; 