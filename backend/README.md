# Navigate to your Presentation layer directory first
## Add Migrations
dotnet ef migrations add X --project ../Publisher.Infrastructure

## Update Database
dotnet ef database update --project ../Publisher.Infrastructure

# Run Server
dotnet run

// TODO
// i need to be able to use the slug in the other endpoints aswell. update, delete, update-book: genres, covers, authors, artists. Fix fluentvalidation to allow slugs