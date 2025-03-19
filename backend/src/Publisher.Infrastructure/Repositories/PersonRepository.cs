using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class PersonRepository(AppDbContext _context) : IPersonRepository
{
    public async Task<List<Person>> GetPersonsAsync(CancellationToken token = default)
    {
        return await _context.Persons.ToListAsync(token);
    }
} 