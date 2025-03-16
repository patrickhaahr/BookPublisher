# Book Publisher API

## Backend Architecture

### Overview
The Book Publisher API is built using .NET 10 WebAPI, following clean architecture principles.

### Technical Stack
- **.NET 10**: Latest framework version
- **Entity Framework Core**: ORM for data access
- **SQL Server**: Primary database

### Key Features
- Clean Architecture implementation
- CQRS pattern with MediatR
- Repository pattern for data access
- Global error handling middleware
- API versioning

### Design Patterns
- Repository Pattern
- CQRS
- Mediator

## API Testing

### Helpers
To test the API endpoints, import the Postman collection from the `/postman` directory. The collection includes pre-configured requests for all endpoints with example payloads and authentication headers.

### Postman Setup
1. Import the Postman collection:
   ```
   /postman/BookPublisher.postman_collection.json
   ```
2. Set up environment variables:
   - `BASE_URL`: Your API base URL (e.g., `https://localhost:5001`)

