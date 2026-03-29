# FoodGrabber Architecture

## Purpose

This document defines the architecture currently used in `src` and the rules for extending it.

The codebase is organized around:

- `FoodGrabber.API`: application host and composition root
- `Modules/*`: business modules
- `Infrastructure/*`: shared persistence and EF Core database setup
- `Shared/*`: cross-cutting primitives used by multiple modules

The goal is to keep modules independent, predictable, and easy to register from the API without pushing module-specific wiring into the API layer.

## High-Level Structure

```text
src/
  FoodGrabber.API/
  Infrastructure/
    FoodGrabber.Infrastructure/
  Modules/
    Identity/
    Menu/
    Order/
    Product/
    Delivery/
    Notification/
    Payment/
    Restaurant/
    Review/
  Shared/
    FoodGrabber.Shared/
```

## Layer Responsibilities

### `FoodGrabber.API`

This project is the host.

It should:

- bootstrap the web app
- read configuration
- call each module's registration extension method
- configure host-only concerns such as Swagger and frontend CORS
- map module endpoints
- trigger module seeding entry points

It should not:

- contain module business logic
- contain module repositories or services
- contain authentication implementation details that belong to the identity module
- manually register individual module internals when a module extension method should own that work

### `Modules/*`

Each module owns its own behavior and public integration surface.

Typical contents inside a module:

- `Abstractions/`: interfaces owned by the module
- `Entities/` or `Entites/`: domain/data objects owned by the module
- `Infrastructure/` or `Infrastructures/`: module-local implementations such as repositories, read contracts, or stores
- `Services/`: application/service layer behavior
- `Contracts/`: DTOs exposed by the module
- `Extensions/`: module registration, endpoint mapping, and seed entry points
- `Exceptions/`: module-specific exceptions
- `Events/`: module-specific event types

Every module should expose one clear registration method in `Extensions/*ModuleExtensions.cs`.

Examples already in the codebase:

- `AddMenuModule()`
- `AddOrderModule()`
- `AddProductModule()`
- `AddIdentityModule<TDbContext>(...)`

### `Infrastructure/FoodGrabber.Infrastructure`

This project owns shared EF Core persistence concerns.

It currently contains:

- `AppDbContext`
- design-time db context factory
- EF entity configurations
- migrations

It is the concrete persistence host for multiple modules.

### `Shared/FoodGrabber.Shared`

This project contains small cross-cutting primitives only.

Current examples:

- pagination types
- result wrapper
- role names
- general abstractions such as `IRepository`, `IUnitOfWork`, `IAdminUserProvider`

This project must stay small and stable.

## Composition Root Rules

The API is the composition root, but module-level registration belongs to the module.

Current pattern:

```csharp
builder.Services.AddApplicationModules(builder.Configuration);
```

Inside `AddApplicationModules(...)`, the API should call module entry points, not register module internals one by one.

Current example:

```csharp
services.AddIdentityModule<AppDbContext>(
    configuration,
    options => options.UseSqlServer(connectionString));
services.AddOrderModule();
services.AddMenuModule();
services.AddProductModule();
```

This means:

- API knows which modules are enabled
- each module knows how to register its own dependencies

## Module Registration Standard

Every module should follow this pattern.

### Required

Each module should expose:

- `Add{ModuleName}Module(...)` for service registration
- `Map{ModuleName}Endpoints(...)` for HTTP endpoints when the module owns endpoints
- optional `Seed{ModuleName}Async(...)` when the module owns seeding

### Registration Ownership Rule

The module extension method must register the module's own:

- services
- repositories
- stores
- read contracts
- internal abstractions
- auth helpers specific to that module

The API should call the module extension method, not duplicate those registrations.

## Dependency Direction

Allowed dependency flow:

```text
API -> Modules
API -> Infrastructure
API -> Shared

Infrastructure -> Modules
Infrastructure -> Shared

Modules -> Shared
```

Be careful with `Infrastructure -> Modules`: this is used today because `AppDbContext` includes entity/configuration types from modules. Do not add reverse references from a module back into `Infrastructure` unless you intentionally want a tight coupling and have verified no project cycle is created.

### Important Rule

Do not create circular project references.

Example of what to avoid:

- `Infrastructure` references `Identity`
- then `Identity` also references `Infrastructure`

That creates a cycle and breaks the solution structure.

## Persistence Pattern

`AppDbContext` is currently the shared EF Core context for multiple modules.

It contains sets for:

- identity entities
- menu entities
- product entities
- order entities

It also applies EF configurations from multiple assemblies.

Implication:

- persistence is centralized
- business behavior is modularized

This is acceptable for the current codebase, but contributors must respect module ownership even though persistence is shared.

### Current Practical Rule

If a module needs persistence logic:

1. define an abstraction in the module
2. implement it inside that module when possible
3. register it from that module's extension method
4. use `DbContext` or the generic context abstraction if that avoids coupling the module to `Infrastructure`

Identity currently follows this approach with `ICustomerProfileStore` and `IdentityCustomerProfileStore`.

## Identity Module Rules

Identity is a normal module. It is not special-cased in the API except for being called from the host.

Identity owns:

- auth endpoints
- JWT token factory and options
- identity seeding
- admin-user provider
- customer profile store abstraction and implementation
- identity entities such as `ApplicationUser`, `ApplicationRole`, and `Customer`

The API should only:

- call `AddIdentityModule<TDbContext>(...)`
- call `MapAuthEndpoints()`
- call `SeedIdentityAsync(...)`

The API should not:

- directly register `ICustomerProfileStore`
- directly register `JwtTokenFactory`
- directly register identity-specific services
- reintroduce auth classes under `FoodGrabber.API`

## Endpoint Mapping Rules

Endpoint mapping belongs to the module that owns the feature.

Examples:

- identity endpoints are mapped from the identity module
- menu endpoints are mapped from the menu module
- product endpoints are mapped from the product module
- order endpoints are mapped from the order module

Rules:

- use route groups per module
- set tags from the module
- set authorization at module or endpoint level where appropriate
- keep endpoint handlers thin
- delegate business behavior to services/stores/repositories

## Shared Code Rules

Put code into `Shared` only if it is:

- genuinely generic
- useful across multiple modules
- not tied to one business concept
- stable enough that multiple modules can depend on it safely

Do not put code into `Shared` just because two files need it today. Shared is not a dumping ground.

## Do

- Do keep business logic inside the owning module.
- Do expose one registration entry point per module.
- Do keep the API focused on hosting and orchestration.
- Do define abstractions in the same module that owns the behavior.
- Do keep implementations close to the owning module unless the concern is truly cross-cutting.
- Do prefer thin endpoints and explicit services.
- Do keep `Shared` minimal.
- Do use module-specific exceptions for module validation/business errors.
- Do add seed entry points in the module when the module owns seed behavior.
- Do think about project-reference direction before moving classes between projects.

## Don't

- Don't register module internals directly in the API if the module can register them itself.
- Don't move module behavior into `FoodGrabber.API`.
- Don't let one module depend on another module's internal implementation classes.
- Don't add circular references between `Infrastructure` and modules.
- Don't put database-specific code into API endpoints.
- Don't use `Shared` for module-specific DTOs, repositories, stores, or services.
- Don't create fat endpoint methods with embedded business rules.
- Don't bypass module abstractions just because `AppDbContext` is available.
- Don't duplicate registration logic across API and module extension methods.
- Don't treat Identity as a special exception to the module architecture.

## When Adding a New Module

Use this checklist:

1. Create a module project under `Modules/{ModuleName}`.
2. Add `Extensions/{ModuleName}ModuleExtensions.cs`.
3. Put module interfaces under `Abstractions/`.
4. Put module implementations under `Services/` and `Infrastructure/` or `Infrastructures/`.
5. Add endpoint mapping inside the module extension file if the module exposes HTTP endpoints.
6. Register all module dependencies inside `Add{ModuleName}Module(...)`.
7. Only call `services.Add{ModuleName}Module(...)` from the API.
8. If EF persistence is needed, integrate with `AppDbContext` and module EF configurations without creating a project-reference cycle.

## When Refactoring Existing Code

Before moving a class:

- identify who owns the behavior
- identify current project-reference direction
- verify the destination project can reference what it needs
- check whether the move will create a cycle
- move registration with the class ownership, not just the type itself

Bad refactor:

- move a repository/store into a module
- keep registering it manually from API

Good refactor:

- move the repository/store into the owning module
- register it inside that module's extension method
- keep API limited to calling the module extension

## Current Known Constraints

- `AppDbContext` is shared across several modules instead of each module owning a separate database context.
- Some folders use inconsistent naming such as `Entites` and `Infrastructures`.
- Several modules exist as placeholders and are not fully implemented yet.

These constraints are acceptable for now, but new work should still follow the architectural rules in this document.

## Source of Truth

If code and this document diverge, update the code or update this document immediately. The architecture document must describe the real enforced pattern, not an aspirational one that the repo does not follow.
