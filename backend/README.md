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

Getbookbyid response body should include covers.
UpdateBook

{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "id": [
            "The value '36e6b9f-12b1-4730-b07c-ff09705e3482' is not valid."
        ]
    },
    "traceId": "00-bfbd2a917bcc5d993941fbc3d0534f73-d14c5e5397dd2805-00"
}