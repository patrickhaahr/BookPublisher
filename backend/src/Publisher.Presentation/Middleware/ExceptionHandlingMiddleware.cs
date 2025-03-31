using System.Net;
using System.Text.Json;
using FluentValidation;
using Publisher.Contracts.Responses;
using Publisher.Domain.Exceptions;
using ValidationException = Publisher.Domain.Exceptions.ValidationException;
using DomainValidationError = Publisher.Domain.Exceptions.ValidationError;
using ResponseValidationError = Publisher.Contracts.Responses.ValidationError;

namespace Publisher.Presentation.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var result = exception switch
        {
            // Handle FluentValidation exceptions
            FluentValidation.ValidationException fluentValidationEx => new ErrorResponse
            {
                Message = "Validation failed",
                StatusCode = (int)HttpStatusCode.BadRequest,
                Errors = fluentValidationEx.Errors.Select(e => new ResponseValidationError { PropertyName = e.PropertyName, ErrorMessage = e.ErrorMessage }).ToList()
            },
            // Handle our custom ValidationException
            ValidationException domainValidationEx => new ErrorResponse
            {
                Message = domainValidationEx.Message,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Errors = domainValidationEx.Errors.Select(e => new ResponseValidationError { PropertyName = e.PropertyName, ErrorMessage = e.ErrorMessage }).ToList()
            },
            NotFoundException notFoundException => new ErrorResponse
            {
                Message = notFoundException.Message,
                StatusCode = (int)HttpStatusCode.NotFound,
                Errors = null
            },
            _ => new ErrorResponse
            {
                Message = "An unexpected error occurred",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Errors = null
            }
        };

        response.StatusCode = result.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}