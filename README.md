# Members Management API

An ASP.NET Core 8 backend for a members management platform. The API demonstrates authentication, policy-based authorization, member CRUD workflows, paging/search, EF Core persistence, Swagger documentation, health checks, rate limiting and a clean separation between API, application, domain and infrastructure layers.

This repository is part of a full-stack portfolio project. The companion Angular frontend lives at [members-app](https://github.com/sandeepyeg/members-app).

## What It Shows

- JWT login/logout flow with token revocation support
- Role and permission based access policies for member operations
- Member create, read, update, delete, search, filter and paging endpoints
- EF Core with SQLite for simple local development
- Application/domain/infrastructure project boundaries
- MediatR command/query handlers and FluentValidation validators
- Swagger/OpenAPI setup with bearer-token authorization
- Health endpoints for liveness and database readiness
- Rate limiting and centralized exception handling middleware

## Project Structure

```text
src/
  EnterpriseMembers.Api              HTTP API, middleware, Swagger, auth setup
  EnterpriseMembers.Application      DTOs, services, CQRS handlers, validators
  EnterpriseMembers.Domain           Entities, enums and shared domain models
  EnterpriseMembers.Infrastructure   EF Core, repositories, seeding and services
tests/
  EnterpriseMembers.Tests            Test project scaffold
```

## Local Setup

Prerequisites:

- .NET 8 SDK

Run the API:

```bash
dotnet restore
dotnet build EnterpriseMembers.sln
dotnet run --project src/EnterpriseMembers.Api
```

The API starts on the ports defined in `src/EnterpriseMembers.Api/Properties/launchSettings.json`. In development, Swagger is available at `/swagger`.

The app creates a local SQLite database automatically through EF Core migrations and seeds demo records when the database is empty.

Default demo login:

```text
Email: admin@company.com
Password: 123456
```

## Configuration

Public configuration uses safe placeholders. For real deployments, override these values with environment variables, user secrets or a secret manager:

```text
ConnectionStrings__DefaultConnection
Jwt__Secret
Jwt__Issuer
Jwt__Audience
Jwt__ExpiryMinutes
Cors__AllowedOrigins__0
```

Do not commit generated SQLite database files or local secrets. The repository ignores `*.db`, `*.db-shm`, `*.db-wal`, `.env`, `appsettings.Development.json` and other local-only files.

## Useful Endpoints

```text
POST   /api/v1/auth/login
POST   /api/v1/auth/logout
GET    /api/v1/members
GET    /api/v1/members/{id}
POST   /api/v1/members
PUT    /api/v1/members/{id}
DELETE /api/v1/members/{id}
GET    /health
GET    /health/db
```

## Portfolio Notes

This is intentionally a compact business application rather than a giant platform. It is useful for showing everyday production skills: secure API boundaries, validation, database-backed workflows, role-aware access, structured project organization and a frontend/backend contract that can grow cleanly.

## Next Improvements

- Add richer automated tests around authorization, validation and member workflows
- Add Docker Compose for running API and frontend together
- Add deployment notes for Azure Container Apps or App Service
- Add screenshots and a short architecture diagram to the companion frontend README
