# VSR Systems Backend Architecture

## Runtime

- .NET 8 ASP.NET Core API
- EF Core with PostgreSQL
- Redis with in-memory fallback
- Swagger/OpenAPI
- One module-isolated modular monolith

## Dependency Direction

```text
Api -> Application -> Domain
Infrastructure -> Application + Domain
```

`Core` and `Shared` contain cross-cutting primitives only. Business modules must not access another module's repository directly.

## Structure

```text
src/
  VSRSystemsBackend.Api/
    Modules/
    Platform/
  VSRSystemsBackend.Application/
  VSRSystemsBackend.Domain/
  VSRSystemsBackend.Infrastructure/
  VSRSystemsBackend.Core/
  VSRSystemsBackend.Shared/
```

## Data Environments

- Development: local PostgreSQL `vsr_systems_dev` on port `5433`; sample seed mode.
- Production: Supabase PostgreSQL; automatic seed mode disabled.
- Shared document persistence: `/api/{module}/data/{collection}`.
- Production secrets move to Azure Key Vault in Phase 3.

The local cluster files live under ignored `.local/postgres-data` and must never be committed.
