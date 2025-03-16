using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Queries.GetCovers;

public record GetCoversQuery : IRequest<List<Cover>>; 