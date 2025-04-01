using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Publisher.Application.Interfaces;
using Publisher.Application.Interfaces.Authentication;
using Publisher.Infrastructure.Authentication;
using Publisher.Infrastructure.Repositories;

namespace Publisher.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPersistence(configuration);
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            // Suppress the warning about dynamic data in seeding
            options.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        });

        // Register repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<ICoverRepository, CoverRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserBookInteractionRepository, UserBookInteractionRepository>();

        // Register authentication services
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddSingleton(configuration.GetSection("JwtSettings").Get<JwtSettings>() 
            ?? throw new InvalidOperationException("JWT settings are not configured in the application configuration."));

        // JWT Authentication and Entra ID configuration
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() 
            ?? throw new InvalidOperationException("JWT settings are not configured in the application configuration.");
            
        services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            })
            .AddMicrosoftIdentityWebApi(configuration.GetSection("EntraID"), "EntraID");

        // Azure Functions
        services.AddHttpClient();

        return services;
    }
}
