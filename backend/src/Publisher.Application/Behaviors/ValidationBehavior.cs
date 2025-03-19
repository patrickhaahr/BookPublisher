using FluentValidation;
using MediatR;

namespace Publisher.Application.Behaviors;

// Open generic pipeline behavior
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator = validator;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        if (_validator is null) {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, token);

        if (validationResult.IsValid) {
            return await next();
        }
        
        throw new ValidationException(validationResult.Errors);
    }
}
