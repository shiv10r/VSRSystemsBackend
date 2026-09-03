# Railway Platform Compatibility

| Capability | Existing implementation | Railway registration |
|---|---|---|
| Authentication | `CacheTokenAuthenticationHandler` | Railway scope derives user and tenant claims from `HttpContext.User`. |
| Organization scope | Railway claim scope boundary | `IRailwayScopeAccessor` rejects missing organization claims. |
| Permissions | Permission claims | `RailwayScope.RequirePermission` enforces application-level permissions. |
| Feature flags | Application configuration | `IRailwayFeatureGate` resolves organization capability flags and the 72-hour offline maximum. |
| PostgreSQL | Npgsql `DefaultConnection` | `RailwayDbContext` uses the same physical database with Railway-owned migration history. |
| OpenAPI | Swashbuckle | Railway operations receive stable operation IDs and bearer metadata. |

The shared append-only audit writer, transactional outbox, malware scanner, and private evidence-storage workflow are not yet registered. Railway capability handlers must not claim production readiness until those Task 4 prerequisites are implemented and tested.
