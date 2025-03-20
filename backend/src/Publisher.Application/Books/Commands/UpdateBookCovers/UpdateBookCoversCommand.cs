using MediatR;
using Publisher.Contracts.Requests;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBookCovers;

public record UpdateBookCoversCommand(
    string IdOrSlug,
    List<CoverUpdateData> Covers
) : IRequest<UpdateBookCoversResponse>;