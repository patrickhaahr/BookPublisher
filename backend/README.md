# Navigate to your Presentation layer directory first
## Add Migrations
dotnet ef migrations add X --project ../Publisher.Infrastructure

## Update Database
dotnet ef database update --project ../Publisher.Infrastructure

# Run Server
dotnet run

// TODO
UPDATE THE REST OF CQRS METHODS TO USE PROPER RESPONSES AND REQUEST FROM PUBLISHER.CONTRACTS, INSTEAD OF DOMAIN ENTITY