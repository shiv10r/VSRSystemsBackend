using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Platform.ModuleData;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Platform.ModuleData;

public sealed class ModuleDataService(AppDbContext context) : IModuleDataService
{
    public async Task<string?> GetAsync(string module, string collection, CancellationToken cancellationToken = default)
    {
        return await context.ModuleDataDocuments
            .Where(document => document.Module == module && document.Collection == collection)
            .Select(document => document.Json)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task SaveAsync(string module, string collection, string json, CancellationToken cancellationToken = default)
    {
        await context.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "ModuleDataDocuments" ("Id", "Module", "Collection", "Json", "CreatedAt", "UpdatedAt", "IsDeleted")
            VALUES ({Guid.NewGuid()}, {module}, {collection}, {json}, {DateTime.UtcNow}, NULL, FALSE)
            ON CONFLICT ("Module", "Collection")
            DO UPDATE SET "Json" = EXCLUDED."Json", "UpdatedAt" = {DateTime.UtcNow}, "IsDeleted" = FALSE
            """, cancellationToken);
    }
}
