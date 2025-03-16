using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IPersonRepository
{
    Task<List<Person>> GetPersonsAsync();
} 