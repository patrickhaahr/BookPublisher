namespace Publisher.Domain.Entities;

public abstract class Person
{
    public Guid PersonId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
} 