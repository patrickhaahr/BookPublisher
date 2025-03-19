using System.Net;
using System.Text.Json;
using FluentValidation;
using Publisher.Contracts.Responses;
using Publisher.Domain.Exceptions;

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
            ValidationException validationException => new ErrorResponse
            {
                Message = "Validation failed",
                StatusCode = (int)HttpStatusCode.BadRequest,
                Errors = validationException.Errors.Select(e => new ValidationError { PropertyName = e.PropertyName, ErrorMessage = e.ErrorMessage }).ToList()
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