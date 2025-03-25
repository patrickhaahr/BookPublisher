# Navigate to your Presentation layer directory first
## Add Migrations
dotnet ef migrations add X --project ../Publisher.Infrastructure

## Update Database
dotnet ef database update --project ../Publisher.Infrastructure

# Run Server
dotnet run

// TODO
Update Register endpoint, so i dont need to request with Roles. Roles should be automaticly assigned with User