using Microsoft.AspNetCore.Server.Kestrel.Core;
using Publisher.Application;
using Publisher.Infrastructure;
using Publisher.Presentation.Middleware;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// Upload image size limit
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024; // 1 MB
});

// JSON Enum Converter
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection(); // Always redirect to HTTPS (safest port)
app.UseAuthorization();
app.MapControllers();

app.Run();