# Navigate to your Presentation layer directory first
## Add Migrations
dotnet ef migrations add X --project ../Publisher.Infrastructure

## Update Database
dotnet ef database update --project ../Publisher.Infrastructure

# Run Server
dotnet run

TODO
change to HTTPS
health check
add Compiled Query for GetBooks

Implement Azure functions test
Azure Entra ID login auth