# Navigate to your Presentation layer directory first
## Add Migrations
dotnet ef migrations add X --project ../Publisher.Infrastructure

## Update Database
dotnet ef database update --project ../Publisher.Infrastructure

# Run Server
dotnet run

// TODO
UPDATE THE REST OF CQRS METHODS TO USE PROPER RESPONSES AND REQUEST FROM PUBLISHER.CONTRACTS, INSTEAD OF DOMAIN ENTITY

Update response and request

Plan:
Keep the basic book update endpoint simple
Create separate CRUD endpoints for relationships
Design appropriate commands/handlers for each relationship type

public const string BookGenres = $"{Base}/{{id}}/genres";
public const string BookPersons = $"{Base}/{{id}}/persons";
public const string BookCovers = $"{Base}/{{id}}/covers";
public const string BookAuthors = $"{Base}/{{id}}/authors";
public const string BookArtists = $"{Base}/{{id}}/artists";