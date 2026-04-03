# FoodGrabber

FoodGrabber contains:

- ASP.NET Core modular monolith backend (`src/`)
- Next.js frontend (`frontend/`)

## Stack

- .NET 10
- ASP.NET Core Minimal API
- Entity Framework Core
- SQL Server
- Next.js + React + TypeScript

## Local Docker Run (API + Frontend + SQL Server)

Files:

- `src/FoodGrabber.API/Dockerfile`
- `frontend/Dockerfile`
- `docker-compose.yml`
- `.env.example`

1. Create env file:

```powershell
Copy-Item .env.example .env
```

2. Set required values in `.env`:

- `MSSQL_SA_PASSWORD`
- `JWT_KEY`
- `DEFAULT_ADMIN_PASSWORD`

3. Run:

```powershell
docker compose --env-file .env up -d --build
```

4. Access:

- Frontend: `http://localhost:3000`
- API: `http://localhost:5000`

5. Stop:

```powershell
docker compose down
```

Remove DB volume too:

```powershell
docker compose down -v
```

## DigitalOcean Deployment (Droplet + Docker Compose + Caddy)

This repo now includes production-oriented DigitalOcean files:

- `docker-compose.digitalocean.yml`
- `deploy/Caddyfile`

This setup uses:

- HTTPS termination with Caddy (Let's Encrypt)
- domain routing:
  - `FRONTEND_DOMAIN` -> frontend container
  - `API_DOMAIN` -> backend API container
- SQL Server in Docker with persistent volume (`mssql-data`)

### 1) DNS

Create A records that point to your Droplet public IP:

- `FRONTEND_DOMAIN` (example: `foodgrabber.example.com`)
- `API_DOMAIN` (example: `api.foodgrabber.example.com`)

### 2) Provision the server

On Ubuntu Droplet, install Docker + Compose plugin, then clone the repo.

### 3) Configure environment

Create `.env`:

```bash
cp .env.example .env
```

Set production values:

- `MSSQL_SA_PASSWORD`
- `JWT_KEY`
- `DEFAULT_ADMIN_PASSWORD`
- `FRONTEND_DOMAIN`
- `API_DOMAIN`
- `LETSENCRYPT_EMAIL`
- `FRONTEND_PUBLIC_URL` (must be `https://<FRONTEND_DOMAIN>`)
- `NEXT_PUBLIC_API_URL` (must be `https://<API_DOMAIN>/api`)

Optional DigitalOcean Spaces:

- `DO_SPACES_ENDPOINT`
- `DO_SPACES_ACCESS_KEY`
- `DO_SPACES_SECRET_KEY`
- `DO_SPACES_BUCKET`
- `DO_SPACES_PUBLIC_BASE_URL`

### 4) Run production compose

```bash
docker compose --env-file .env -f docker-compose.digitalocean.yml up -d --build
```

### 5) Verify

- Frontend: `https://<FRONTEND_DOMAIN>`
- API health/test endpoint group: `https://<API_DOMAIN>/api/...`

Check logs:

```bash
docker compose --env-file .env -f docker-compose.digitalocean.yml logs -f
```

### 6) Update deployment

After pulling new code:

```bash
docker compose --env-file .env -f docker-compose.digitalocean.yml up -d --build
```

## Backend Connection String (non-Docker local dev)

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FoodGrab;Integrated Security=True;Trust Server Certificate=True"
}
```

Configured in:

- `src/FoodGrabber.API/appsettings.json`
- `src/FoodGrabber.API/appsettings.Development.json`

## Local Non-Docker Prerequisites

- .NET SDK 10
- SQL Server LocalDB
- Node.js + npm

LocalDB check:

```powershell
sqllocaldb info
```

If missing:

```powershell
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

## Backend Commands

Restore:

```powershell
dotnet restore
```

Run migrations:

```powershell
dotnet ef database update `
  --project src\Infrastructure\FoodGrabber.Infrastructure\FoodGrabber.Infrastructure.csproj `
  --startup-project src\FoodGrabber.API\FoodGrabber.API.csproj `
  --context FoodGrabber.Infrastructure.Data.AppDbContext
```

Run API:

```powershell
dotnet run --project src\FoodGrabber.API\FoodGrabber.API.csproj
```

## Frontend Commands

```powershell
cd frontend
npm install
npm run dev
```

## Default Admin User

Seeded on startup:

- Email: `admin@foodgrabber.local`
- Password: `Admin@123456`

## Notes

- API applies EF Core migrations automatically on startup.
- For production uploads, DigitalOcean Spaces is recommended. Local file storage in container is not ideal for long-term persistence.
