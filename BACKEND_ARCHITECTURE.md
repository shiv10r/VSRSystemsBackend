# VSR Systems Backend Architecture

## Technology Stack
- **Framework**: .NET (ASP.NET Core)
- **Database**: PostgreSQL
- **Architecture Pattern**: MVC (Model-View-Controller)

## Layered Architecture

```
Controller → Interface (IBusinessLayer) → BusinessLayer → Interface (IRepo) → Repo → PostgreSQL
```

### Components

| Layer | Responsibility | Key Interfaces |
|-------|---------------|----------------|
| **Controller** | HTTP endpoints, request/response handling | - |
| **Business Layer** | Business logic, validation, orchestration | `IBusinessLayer` |
| **Repository Layer** | Data access, CRUD operations | `IRepo` |

### Interface Contracts

- **`IBusinessLayer`** - Defines business operations exposed to controllers
- **`IRepo`** - Defines data access operations (CRUD, queries)

### Flow
1. Controller receives HTTP request
2. Controller calls `IBusinessLayer` implementation
3. Business layer applies logic, calls `IRepo` implementation
4. Repository executes PostgreSQL queries
5. Response flows back up the chain

## Project Structure (Expected)
```
VSRSystemsBackend/
├── Controllers/
├── BusinessLayer/
│   ├── Interfaces/ (IBusinessLayer)
│   └── Implementations/
├── Repository/
│   ├── Interfaces/ (IRepo)
│   └── Implementations/
├── Models/
└── Data/ (DbContext, Migrations)
```