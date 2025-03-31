using Microsoft.AspNetCore.Server.Kestrel.Core;
using Publisher.Application;
using Publisher.Infrastructure;
using Publisher.Presentation.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddHttpContextAccessor();

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

builder.Services.AddHttpClient();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:5173") // Allow frontend
            .AllowAnyMethod()                        // Allow all HTTP methods
            .AllowAnyHeader()                        // Allow all headers
            .AllowCredentials();                     // Allow credentials (cookies, auth headers)
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection(); // Always redirect to HTTPS (safest port)

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();