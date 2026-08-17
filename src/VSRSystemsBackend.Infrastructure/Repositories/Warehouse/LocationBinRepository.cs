using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class LocationBinRepository : ILocationBinRepository
{
    private readonly AppDbContext _context;

    public LocationBinRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LocationBin?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.LocationBins.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<LocationBin>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LocationBins
            .Where(l => !l.IsDeleted)
            .OrderBy(l => l.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LocationBin>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.LocationBins
            .Where(l => l.WarehouseId == warehouseId && !l.IsDeleted)
            .OrderBy(l => l.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<LocationBin?> GetByCodeAsync(string warehouseId, string code, CancellationToken cancellationToken = default)
    {
        return await _context.LocationBins
            .FirstOrDefaultAsync(l => l.WarehouseId == warehouseId && l.Code == code && !l.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<LocationBin>> GetActiveByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.LocationBins
            .Where(l => l.WarehouseId == warehouseId && l.IsActive && !l.IsDeleted)
            .OrderBy(l => l.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LocationBin>> FindAsync(Expression<Func<LocationBin, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.LocationBins
            .Where(l => !l.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<LocationBin> AddAsync(LocationBin entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.LocationBins.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<LocationBin> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.LocationBins.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(LocationBin entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.LocationBins.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(LocationBin entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.LocationBins.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<LocationBin> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.LocationBins.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<LocationBin, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.LocationBins.Where(l => !l.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<LocationBin, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.LocationBins
            .Where(l => !l.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<LocationBin> Query()
    {
        return _context.LocationBins.Where(l => !l.IsDeleted);
    }
}
