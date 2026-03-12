# Todo

ASP.NET Core Web API built with Clean Architecture (no DDD), CQRS (MediatR), EF Core, Identity + JWT Bearer, and FluentValidation.

## Architecture

```
src/
├── Todo.Domain/           — Plain entities, enums, repository interfaces
├── Todo.Application/      — Use cases (commands/queries), validators, DTOs, business logic
├── Todo.Infrastructure/   — EF Core, Identity, JWT, service implementations
└── Todo.API/              — Controllers, middleware, DI wiring
tests/
├── Todo.Application.Tests/
├── Todo.Infrastructure.Tests/
└── Todo.Integration.Tests/
```

Dependencies flow inward only: `API → Application → Domain` and `Infrastructure → Application → Domain`.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (included with Visual Studio) or SQL Server

## Getting Started

### 1. Configure the connection string

Edit `src/Todo.API/appsettings.json` if needed (default targets LocalDB):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoDb;Trusted_Connection=True;"
}
```

### 2. Set the JWT secret

Store the JWT key in user secrets or an environment variable — **never commit it**:

```bash
cd src/Todo.API
dotnet user-secrets set "Jwt:Key" "your-min-32-char-secret-here"
```

### 3. Apply migrations

```bash
dotnet ef migrations add InitialCreate -p src/Todo.Infrastructure -s src/Todo.API
dotnet ef database update             -p src/Todo.Infrastructure -s src/Todo.API
```

### 4. Run the API

```bash
dotnet run --project src/Todo.API
```

Swagger UI: http://localhost:5000/swagger

## API Endpoints

| Method | Route | Description | Auth |
|--------|-------|-------------|------|
| `POST` | `/api/auth/register` | Register a new user | Public |
| `POST` | `/api/auth/login` | Login and get tokens | Public |
| `POST` | `/api/auth/refresh` | Refresh access token | Public |
| `POST` | `/api/auth/revoke` | Revoke refresh token | Public |
| `GET`  | `/api/auth/me` | Get current user | Bearer |
| `GET`  | `/api/todolists` | Get all lists | CanRead |
| `GET`  | `/api/todolists/{id}` | Get list by ID | CanRead |
| `POST` | `/api/todolists` | Create a list | CanWrite |
| `DELETE` | `/api/todolists/{id}` | Delete a list | CanDelete |
| `POST` | `/api/todoitems` | Create an item | CanWrite |
| `PUT`  | `/api/todoitems/{id}` | Update an item | CanWrite |
| `POST` | `/api/todoitems/{id}/complete` | Mark item done | CanWrite |
| `DELETE` | `/api/todoitems/{id}` | Delete an item | CanDelete |

## Roles

| Role | CanRead | CanWrite | CanDelete |
|------|---------|----------|-----------|
| User | ✅ | ❌ | ❌ |
| Manager | ✅ | ✅ | ❌ |
| Admin | ✅ | ✅ | ✅ |

## Development

```bash
dotnet build
dotnet test
```
