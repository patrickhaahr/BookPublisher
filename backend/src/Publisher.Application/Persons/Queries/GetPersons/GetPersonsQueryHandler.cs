using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Persons.Queries.GetPersons;

public class GetPersonsQueryHandler(IPersonRepository _personRepository)
    : IRequestHandler<GetPersonsQuery, List<Person>>
{
    public async Task<List<Person>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        return await _personRepository.GetPersonsAsync();
    }
} 