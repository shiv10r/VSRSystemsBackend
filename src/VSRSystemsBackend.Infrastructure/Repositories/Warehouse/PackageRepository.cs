using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class PackageRepository : IPackageRepository
{
    private readonly AppDbContext _context;

    public PackageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Package?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Packages
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == (string)id && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Package>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Packages
            .Where(p => !p.IsDeleted)
            .Include(p => p.Items)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Package>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Packages
            .Where(p => p.OrderId == orderId && !p.IsDeleted)
            .Include(p => p.Items)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Package>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Packages
            .Where(p => p.Status == status && !p.IsDeleted)
            .Include(p => p.Items)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Package?> GetByPackageIdAsync(string packageId, CancellationToken cancellationToken = default)
    {
        return await _context.Packages
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.PackageId == packageId && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Package>> FindAsync(Expression<Func<Package, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Packages
            .Where(p => !p.IsDeleted)
            .Where(predicate)
            .Include(p => p.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<Package> AddAsync(Package entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Packages.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Package> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Packages.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(Package entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Packages.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Package entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Packages.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<Package> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Packages.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<Package, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Packages.Where(p => !p.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Package, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Packages
            .Where(p => !p.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Package> Query()
    {
        return _context.Packages.Where(p => !p.IsDeleted);
    }
}
