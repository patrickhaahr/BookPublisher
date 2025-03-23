using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Covers.Queries.GetCovers;

public class GetCoversQueryHandler(ICoverRepository _coverRepository)
    : IRequestHandler<GetCoversQuery, List<GetCoversResponse>>
{
    public async Task<List<GetCoversResponse>> Handle(GetCoversQuery request, CancellationToken token)
    {
        var covers = await _coverRepository.GetCoversAsync(token);
        return covers.Select(c => new GetCoversResponse(c.BookId.ToString(), c.ImgBase64 ?? string.Empty)).ToList();
    }
} 