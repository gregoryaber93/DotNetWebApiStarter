# DotNetWebApiStarter

Starter template for a layered ASP.NET Core Web API with JWT authentication, PostgreSQL persistence, and email-based account flows.

## Project structure

- `src/API/Api` – Web API host, controllers, Swagger/OpenAPI, JWT setup
- `src/Core/Application` – Application services, DTOs, validators, mapping
- `src/Core/Domain` – Domain entities and value objects
- `src/Infrastructure/Persistence.EF` – EF Core DbContext, seeding, persistence registration

## Tech stack

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core + Npgsql (PostgreSQL)
- JWT Bearer Authentication
- FluentValidation
- AutoMapper
- Swagger / OpenAPI

## Prerequisites

- .NET 9 SDK
- PostgreSQL 16+ (or Docker)

## Local development

### 1) Start PostgreSQL (Docker)

```bash
docker compose -f docker-compose.dev.yml up -d
```

### 2) Configure application settings

Update `src/API/Api/appsettings.json` as needed:

- `ConnectionStrings:UserConnectionString`
- `Authentication` (JWT key/issuer)
- `EmailSettings` (SMTP server, credentials, base URL)

### 3) Build and run

```bash
dotnet restore DotNetWebApiStarter.sln
dotnet build DotNetWebApiStarter.sln
dotnet run --project src/API/Api/Api.csproj
```

### 4) Open API docs

- Swagger UI (development): `https://localhost:7167/swagger`
- OpenAPI document: `https://localhost:7167/openapi/v1.json`

## Database and seed data

- The app uses `UserDbContext` in `Persistence.EF`.
- On startup, `SeederData` inserts default roles if missing:
  - `admin`
  - `user`

If this is a fresh database, create/apply EF migrations before first use:

```bash
dotnet ef migrations add InitialCreate --project src/Infrastructure/Persistence.EF/Persistence.EF.csproj --startup-project src/API/Api/Api.csproj
dotnet ef database update --project src/Infrastructure/Persistence.EF/Persistence.EF.csproj --startup-project src/API/Api/Api.csproj
```

## API endpoints

Base route: `/api`

### Auth (`/api/Auth`)

- `POST /login`
- `POST /register`
- `POST /verify-email`
- `POST /change-password/{userId}` (authorized)
- `POST /request-password-reset`
- `POST /reset-password`
- `DELETE /delete-account/{userId}` (authorized)

### Restaurant (`/api/Restaurant`)

- `GET /test` (authorized)

## Notes

- JWT authentication is configured in `Program.cs` with Bearer scheme.
- Role claims are included in issued JWT tokens.
- OpenAPI/Swagger is enabled in development environment.
