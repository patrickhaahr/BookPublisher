using MediatR;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Queries.GetCovers;

public record GetCoversQuery : IRequest<List<GetCoversResponse>>; 