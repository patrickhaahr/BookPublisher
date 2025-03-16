using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Queries.GetBookById;

public record GetBookByIdQuery(Guid Id) : IRequest<GetBookByIdResponse>;
