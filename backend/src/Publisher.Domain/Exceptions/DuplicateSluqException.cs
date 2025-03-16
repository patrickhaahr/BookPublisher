namespace Publisher.Domain.Exceptions;

public class DuplicateSlugException(string slug) 
    : DomainException($"A book with slug '{slug}' already exists.");