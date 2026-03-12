# Copilot Instructions

## Build & Test Commands

```bash
# Build
dotnet build

# Run API (Scalar docs at http://localhost:5000/scalar)
dotnet run --project src/Todo.API

# Run all tests
dotnet test

# Run tests for a single project
dotnet test --project tests/Todo.Application.Tests

# Run a single test class
dotnet test --filter "ClassName=CreateTodoListHandlerTests"

# Run a single test method
dotnet test --filter "FullyQualifiedName~Handle_ValidCommand_AddsTodoListAndReturnsId"

# EF Core migrations (always target Infrastructure project, startup is API)
dotnet ef migrations add <MigrationName> -p src/Todo.Infrastructure -s src/Todo.API
dotnet ef database update -p src/Todo.Infrastructure -s src/Todo.API

# JWT secret (required for local dev — never hardcode)
cd src/Todo.API && dotnet user-secrets set "Jwt:Key" "your-min-32-char-secret-here"
```

## Architecture

Clean Architecture with CQRS via MediatR. Dependency flow: `API → Application → Domain`; `Infrastructure → Application → Domain`.

```
Todo.Domain        — Entities, enums, domain exceptions, repository interfaces (no dependencies)
Todo.Application   — CQRS handlers, validators, DTOs, pipeline behaviors, application interfaces
Todo.Infrastructure — EF Core DbContext, Identity, JWT TokenService, migration, implementations
Todo.API           — Controllers, middleware, DI wiring (Program.cs)
```

Each layer exposes a `DependencyInjection.cs` with an `AddXxxServices()` extension method. `Program.cs` calls these in order.

## CQRS Conventions

- **Commands** and **Queries** are `record` types implementing `IRequest<TResponse>`.
  - Naming: `CreateTodoListCommand`, `GetAllTodoListsQuery`
- **Handlers** are classes implementing `IRequestHandler<TRequest, TResponse>`.
  - Naming: `CreateTodoListHandler` (same file/folder as its command/query)
- **Validators** inherit `AbstractValidator<T>` and are co-located with their command/query.
  - Naming: `CreateTodoListValidator`
- Controllers call `Mediator.Send()` only — no business logic in controllers.
- `BaseApiController` provides lazy-loaded `IMediator` via `Mediator` property.

### Folder layout (Application layer)
```
Application/
├── Auth/Commands/, Auth/Queries/, Auth/Interfaces/
├── TodoLists/Commands/, TodoLists/Queries/
├── TodoItems/Commands/
└── Common/Behaviors/, Common/Exceptions/, Common/Interfaces/, Common/Settings/
```

## Pipeline Behaviors

MediatR pipeline order (registered in `AddApplicationServices()`):
1. `LoggingBehavior<,>` — logs request/response
2. `ValidationBehavior<,>` — runs FluentValidation; throws `ValidationException` on failure

Always add both behaviors when creating new handler types.

## Authentication & Authorization

- **JWT Bearer** tokens (15-minute access tokens) + **refresh tokens** stored hashed (SHA256) in the DB.
- Refresh tokens have `IsActive`, `IsExpired`, `IsRevoked` computed properties.
- Three authorization policies defined in `Infrastructure/DependencyInjection.cs`:
  - `CanRead` — User, Manager, Admin
  - `CanWrite` — Manager, Admin
  - `CanDelete` — Admin
- Apply policies with `[Authorize(Policy = "CanWrite")]` on controller actions.
- `ApplicationUser` extends `IdentityUser` with `FirstName`, `LastName`, `AvatarUrl`, `CreatedAt`.

## Exception Handling

Custom exceptions map to HTTP status codes via `ExceptionHandlingMiddleware`:

| Exception | Status | Throw from |
|---|---|---|
| `NotFoundException` | 404 | Domain / handlers |
| `ValidationException` | 422 | ValidationBehavior (automatic) |
| `UnauthorizedException` | 401 | Auth handlers |
| `InvalidOperationException` | 400 | Any layer |

Throw these from handlers; never return error status codes directly from controllers.

## Database

- **SQLite** via EF Core (`Data Source=todo.db`).
- `ApplicationDbContext` inherits `IdentityDbContext<ApplicationUser>`.
- Entity configuration via Fluent API in `Infrastructure/Persistence/Configurations/` — one file per entity, applied with `ApplyConfigurationsFromAssembly()`.
- Migrations live in `Infrastructure/Migrations/`.

## Testing

- **Unit tests** (`Todo.Application.Tests`): xUnit + NSubstitute (mocking) + FluentAssertions. Mock `IApplicationDbContext` using NSubstitute; use `Substitute.For<DbSet<T>>()` pattern.
- **Integration tests** (`Todo.Integration.Tests`): `WebApplicationFactory<Program>` with in-memory SQLite. Tests make real HTTP calls through the full pipeline. Replace the registered `DbContextOptions<ApplicationDbContext>` in `ConfigureServices` with an in-memory SQLite connection, then call `EnsureCreated()`.
- **Infrastructure tests** (`Todo.Infrastructure.Tests`): xUnit + FluentAssertions.
- `Program.cs` exposes `public partial class Program { }` at the end to make `Program` visible to `WebApplicationFactory<Program>` in test projects.

## Non-Obvious Conventions

- **Shared DTOs**: `TodoListDto` is defined in `Application/TodoLists/Queries/GetTodoListById/` and reused by `GetAllTodoLists`. Place DTOs in the query folder where they are first needed.
- **Static mapping**: `GetTodoListByIdHandler.MapToDto(TodoList)` is a static method reused across query handlers instead of a separate mapper class.
- **Controller route params**: Use C# record `with` expressions to inject route params — e.g., `await Mediator.Send(command with { Id = id })`.
- **Read queries**: Always use `.AsNoTracking()` on EF queries in handlers that return data (never track entities in query handlers).
- **`InvalidOperationException`**: Use for business-rule violations that don't fit `NotFoundException` or `ValidationException` (maps to 400).
- **Role seeding**: `IdentitySeeder.SeedRolesAsync` runs at startup (in `Program.cs`) and seeds the `Admin`, `Manager`, and `User` roles. New users are automatically assigned the `User` role by `IdentityService.CreateUserAsync`.
- **Password policy**: Min 8 chars, requires digit + uppercase, no non-alphanumeric required. Lockout: 5 failed attempts → 15-minute lockout.
