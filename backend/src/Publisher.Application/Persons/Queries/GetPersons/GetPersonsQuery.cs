using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Persons.Queries.GetPersons;

public record GetPersonsQuery : IRequest<List<Person>>; 