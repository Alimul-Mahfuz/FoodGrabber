# FoodGrabber

FoodGrabber currently contains an ASP.NET Core modular monolith backend and a separate Next.js frontend scaffold under `frontend/`.

## Backend Stack

- .NET 10
- ASP.NET Core Minimal API
- Entity Framework Core
- SQL Server LocalDB

## Frontend Stack

- Next.js
- React
- TypeScript
- App Router

## Backend Connection String

The project is configured to use SQL Server LocalDB with this connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FoodGrab;Integrated Security=True;Trust Server Certificate=True"
}
```

Configured files:

- `src/FoodGrabber.API/appsettings.json`
- `src/FoodGrabber.API/appsettings.Development.json`

## Prerequisites

- .NET SDK 10
- SQL Server LocalDB installed
- Node.js and npm for the frontend

Check LocalDB:

```powershell
sqllocaldb info
```

If `MSSQLLocalDB` is missing, create/start it:

```powershell
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

## Backend Restore

From the repository root:

```powershell
dotnet restore
```

## Run Backend Migrations

Create a new migration:

```powershell
dotnet ef migrations add YourMigrationName `
  --project src\Infrastructure\FoodGrabber.Infrastructure\FoodGrabber.Infrastructure.csproj `
  --startup-project src\FoodGrabber.API\FoodGrabber.API.csproj `
  --context FoodGrabber.Infrastructure.Data.AppDbContext `
  --output-dir Migrations
```

Apply migrations:

```powershell
dotnet ef database update `
  --project src\Infrastructure\FoodGrabber.Infrastructure\FoodGrabber.Infrastructure.csproj `
  --startup-project src\FoodGrabber.API\FoodGrabber.API.csproj `
  --context FoodGrabber.Infrastructure.Data.AppDbContext
```

The initial migration has already been created and applied for the current model.

## Run The Backend

```powershell
dotnet run --project src\FoodGrabber.API\FoodGrabber.API.csproj
```

Swagger UI will be available at:

```text
http://localhost:5000
```

or the HTTPS URL shown by ASP.NET Core when the app starts.

## Run The Frontend

The frontend lives in `frontend/`.

Install dependencies:

```powershell
cd frontend
npm install
```

Start the development server:

```powershell
npm run dev
```

By default the frontend will run on:

```text
http://localhost:3000
```

## Default Admin User

Seeded automatically on startup:

- Email: `admin@foodgrabber.local`
- Password: `Admin@123456`

## Notes

- The app now uses EF Core migrations via `Database.MigrateAsync()`.
- If build fails with file lock errors, stop any running `FoodGrabber.API` process and rerun the command.
- EF tooling may warn that tools version `10.0.2` is older than runtime `10.0.3`. The warning does not block migrations, but updating `dotnet-ef` is recommended.
