# Navigate to your Presentation layer directory first
## Add Migrations
dotnet ef migrations add X --project ../Publisher.Infrastructure

## Update Database
dotnet ef database update --project ../Publisher.Infrastructure

# Run Server
dotnet run

# Health check
GET http://localhost:5094/health

TODO
add Compiled Query for GetBooks


cleanup backend structure
    -Contracts classlib
    -unused endpoints and logic

Result pattern?

UML use case ( se video og lav uml diagram)