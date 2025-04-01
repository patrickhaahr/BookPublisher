using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Publisher.Infrastructure.Health;

public class DatabaseHealthCheck(AppDbContext context, ILogger<DatabaseHealthCheck> logger) : IHealthCheck
{
    public const string Name = "database";
    private readonly AppDbContext _context = context;
    private readonly ILogger<DatabaseHealthCheck> _logger = logger;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken token = new())
    {
        try
        {
            // Log that we're checking database connection
            _logger.LogInformation("Checking database connection health");
            
            // Actually check if we can connect
            bool canConnect = await _context.Database.CanConnectAsync(token);
            
            if (canConnect)
            {
                _logger.LogInformation("Database connection is healthy");
                return HealthCheckResult.Healthy("Database connection is successful");
            }
            else
            {
                const string errorMessage = "Database connection failed";
                _logger.LogError(errorMessage);
                return HealthCheckResult.Unhealthy(errorMessage);
            }
        }
        catch (Exception e)
        {
            const string errorMessage = "Database is unhealthy";
            _logger.LogError(e, errorMessage);
            return HealthCheckResult.Unhealthy(errorMessage, e);
        }
    }
}
