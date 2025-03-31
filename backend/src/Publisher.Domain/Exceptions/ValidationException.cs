namespace Publisher.Domain.Exceptions;

public sealed class ValidationException : DomainException
{
    public IReadOnlyCollection<ValidationError> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = Array.Empty<ValidationError>();
    }

    public ValidationException(string message, IEnumerable<ValidationError> errors) : base(message)
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public ValidationException(string propertyName, string errorMessage) : base(errorMessage)
    {
        Errors = new[] { new ValidationError(propertyName, errorMessage) }.ToList().AsReadOnly();
    }
}

public record ValidationError(string PropertyName, string ErrorMessage); 