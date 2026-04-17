# Migration Instruction for a New Module

This guide covers how to run EF Core migrations when you create a new module (for example: `Cart`) and want new tables in the database.

## 1. Prepare the module for EF mapping

1. Create entity configuration classes in the module:
   - `Infrastructure/Persistence/Configurations/<Entity>EntityConfiguration.cs`
2. Map each entity:
   - `builder.ToTable("...")`
   - keys (`HasKey`)
   - columns (`Property(...).HasColumnName(...)`)
   - relationships (`HasMany/WithOne/HasForeignKey`)
   - indexes (`HasIndex`)

Without these configs, EF may not generate the expected schema.

## 2. Wire module into shared Infrastructure DbContext

1. In `Infrastructure/FoodGrabber.Infrastructure/FoodGrabber.Infrastructure.csproj`:
   - Add `ProjectReference` to the new module project.
2. In `Infrastructure/FoodGrabber.Infrastructure/Data/AppDbContext.cs`:
   - Add `DbSet<ModuleEntity>` properties.
   - Register configuration assembly with:
     - `modelBuilder.ApplyConfigurationsFromAssembly(typeof(YourModuleEntityConfiguration).Assembly);`

If this wiring is missing, migration can run but no tables for the new module will appear.

## 3. Build first (recommended)

Run:

```powershell
dotnet build Infrastructure\FoodGrabber.Infrastructure\FoodGrabber.Infrastructure.csproj
```

Fix build errors before creating migration.

## 4. Create migration

Run:

```powershell
dotnet ef migrations add <MigrationName> --project Infrastructure\FoodGrabber.Infrastructure
```

Example:

```powershell
dotnet ef migrations add AddCartTables --project Infrastructure\FoodGrabber.Infrastructure
```

## 5. Apply migration

Run:

```powershell
dotnet ef database update --project Infrastructure\FoodGrabber.Infrastructure
```

## 6. Verify migration output

1. Check migration files exist in:
   - `Infrastructure/FoodGrabber.Infrastructure/Migrations`
2. Open migration and verify expected operations:
   - `CreateTable("YourTable")`
   - foreign keys
   - indexes
3. Confirm update output includes:
   - `Applying migration '...'`
   - `Done.`

## Common challenges and fixes (from this module integration)

### Challenge 1: `database update` says database is up to date, but new module tables do not exist

Cause:
- New module not included in `AppDbContext` model (missing `DbSet`, missing configuration registration, or missing `ProjectReference`).

Fix:
- Add module project reference in Infrastructure csproj.
- Add `DbSet`s for new entities in `AppDbContext`.
- Register module configuration assembly in `OnModelCreating`.
- Create a new migration and run `database update` again.

### Challenge 2: `dotnet ef` migration add fails with `Build failed`

Cause:
- Compilation error in module code blocks migration generation.

Fix:
- Run `dotnet build` to see exact error.
- Fix code issue first, then rerun `dotnet ef migrations add`.

Example encountered:
- `RouteGroupBuilder` had no `WithTags` extension in one module context.
- Removing/fixing that API usage unblocked the build and migration.

### Challenge 3: EF CLI first-time/sandbox permission issues

Cause:
- Environment restrictions prevent `dotnet ef` from creating required local tool paths.

Fix:
- Run command with appropriate elevated permissions in your environment, then rerun migration commands.

### Challenge 4: EF tools/runtime version warning

Message:
- EF tools version older than runtime.

Fix:
- Not blocking for migration execution, but recommended to update `dotnet-ef` to match runtime version to avoid edge-case tooling issues.

## Minimal command sequence (after wiring is done)

```powershell
dotnet build Infrastructure\FoodGrabber.Infrastructure\FoodGrabber.Infrastructure.csproj
dotnet ef migrations add <MigrationName> --project Infrastructure\FoodGrabber.Infrastructure
dotnet ef database update --project Infrastructure\FoodGrabber.Infrastructure
```

