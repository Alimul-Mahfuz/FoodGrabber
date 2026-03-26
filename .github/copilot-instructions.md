# Copilot Instructions for FoodGrabber

## Build, test, and run commands

Run from repository root (`C:\Users\alimul_mahfuz\source\repos\FoodGrabber`):

```powershell
dotnet restore .\FoodGrabber.slnx
dotnet build .\FoodGrabber.slnx -v minimal
dotnet test .\FoodGrabber.slnx -v minimal
dotnet run --project .\src\FoodGrabber.API\FoodGrabber.API.csproj
```

Run a single xUnit test (example):

```powershell
dotnet test .\tests\FoodGrabber.Order.Tests\FoodGrabber.Order.Tests.csproj --filter "FullyQualifiedName=FoodGrabber.Order.Tests.UnitTest1.Test1"
```

Linting:

- No repository-specific lint command is currently configured (no `dotnet format`/analyzers config wired in repo files).

## High-level architecture

FoodGrabber is a **modular monolith** on ASP.NET Core (`net10.0`) with a solution-level split:

- `src/FoodGrabber.API`: host/composition root (`Program.cs`, `Extensions/ServiceExtensions.cs`)
- `src/Modules/*`: bounded-context modules (Identity, Order, Restaurant, etc.)
- `src/Infrastructure`: persistence/infrastructure wiring (`AppDbContext`)
- `src/Shared`: cross-cutting primitives (`Result<T>`, paging, repository/UoW abstractions, domain event interfaces)
- `tests/*`: module-level xUnit test projects

Current state is scaffold-heavy: module extension/event files are placeholders. API currently wires Swagger and development UI; module registrations are commented stubs in `ServiceExtensions.AddApplicationModules`.

`AppDbContext` in Infrastructure derives from `IdentityDbContext<ApplicationUser, ApplicationRole, string>` and renames Identity tables (`Users`, `Roles`, `UserRoles`, etc.), making Identity the only concretely connected module today.

## Key conventions in this codebase

- Namespace and project naming follow `FoodGrabber.<Area>` and `FoodGrabber.<Module>.*`.
- Each module uses a consistent internal layout pattern:
  - `Extensions/<Module>ModuleExtensions.cs` for DI registration entrypoint
  - `Exceptions/<Module>Exception.cs` for module-specific exception type
  - `Events/*` for module domain events
- Shared contracts are intentionally generic and async-first:
  - `IRepository<T, TKey>` and `IUnitOfWork` in `FoodGrabber.Shared.Abstractions`
  - result/pagination wrappers in `FoodGrabber.Shared.Result` and `.Pagination`
- Nullable reference types and implicit usings are enabled across projects.
- Keep existing identity entity namespace/path as-is unless doing a coordinated rename: it is currently `FoodGrabber.Identity.Entites` (note the spelling) and is referenced by Infrastructure.
