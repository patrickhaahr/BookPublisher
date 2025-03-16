using System.Net;
using System.Text.Json;
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
        
        var result = new
        {
            Message = exception.Message,
            StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            }
        };

        response.StatusCode = result.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(result));
    }
}
