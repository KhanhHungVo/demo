# Todo

ASP.NET Core Web API built with Clean Architecture, DDD, CQRS (MediatR), EF Core, and FluentValidation.

## Architecture

```
src/
├── Todo.Domain/           — Entities (TodoList, TodoItem), value objects, domain events, repository interfaces
├── Todo.Application/      — Use cases (commands/queries), validators, DTOs
├── Todo.Infrastructure/   — EF Core, repository implementations, external services
└── Todo.API/              — Controllers, middleware, DI wiring
tests/
├── Todo.Domain.Tests/
├── Todo.Application.Tests/
└── Todo.Infrastructure.Tests/
```

Dependencies flow inward only: `API → Application → Domain` and `Infrastructure → Application → Domain`.

## Domain Model

- **TodoList** (AggregateRoot) — has a title, description, status (Pending/InProgress/Completed), and a collection of `TodoItem` children.
- **TodoItem** (child Entity) — has a title, notes, done flag, due date, and priority.
- **Value Objects**: `Priority` (0=Low, 1=Medium, 2=High, 3=Critical), `DueDate`
- **Domain Events**: `TodoListCreatedEvent`, `TodoItemAddedEvent`, `TodoItemCompletedEvent`

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (included with Visual Studio) or SQL Server

## Getting Started

### 1. Configure the connection string

Edit `src/Todo.API/appsettings.json` if needed (default targets LocalDB):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoDb;Trusted_Connection=True;"
}
```

### 2. Apply migrations

```bash
dotnet ef migrations add InitialCreate -p src/Todo.Infrastructure -s src/Todo.API
dotnet ef database update -p src/Todo.Infrastructure -s src/Todo.API
```

### 3. Run the API

```bash
dotnet run --project src/Todo.API
```

Swagger UI: http://localhost:5000/swagger

## API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | /api/todolists | Get all todo lists |
| GET | /api/todolists/{id} | Get todo list by ID |
| POST | /api/todolists | Create a new todo list |
| DELETE | /api/todolists/{id} | Delete a todo list |
| POST | /api/todolists/{id}/items | Add an item to a list |
| POST | /api/todolists/{id}/items/{itemId}/complete | Complete an item |
| DELETE | /api/todolists/{id}/items/{itemId} | Remove an item |

## Development

### Build

```bash
dotnet build
```

### Run tests

```bash
dotnet test
```

### Add a migration

```bash
dotnet ef migrations add <MigrationName> -p src/Todo.Infrastructure -s src/Todo.API
```

### Add a new use case

1. Create a folder under `src/Todo.Application/TodoLists/Commands/<UseCaseName>/` or `Queries/`
2. Add `Command.cs` (or `Query.cs`), `Handler.cs`, `Validator.cs`
3. Add the corresponding controller action in `src/Todo.API/Controllers/TodoListsController.cs`

## Error Handling

`ExceptionHandlingMiddleware` maps exceptions to HTTP status codes:

| Exception | Status |
|---|---|
| `ValidationException` | 422 Unprocessable Entity |
| `NotFoundException` | 404 Not Found |
| `DomainException` | 400 Bad Request |
