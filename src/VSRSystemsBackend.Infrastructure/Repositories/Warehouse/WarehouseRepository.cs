using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using DomainWarehouse = VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _context;

    public WarehouseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainWarehouse.Warehouse?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<DomainWarehouse.Warehouse?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Code == code && !w.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.Warehouse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .Where(w => !w.IsDeleted)
            .OrderBy(w => w.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.Warehouse>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .Where(w => w.IsActive && !w.IsDeleted)
            .OrderBy(w => w.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.Warehouse>> FindAsync(Expression<Func<DomainWarehouse.Warehouse, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .Where(w => !w.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<DomainWarehouse.Warehouse> AddAsync(DomainWarehouse.Warehouse entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Warehouses.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<DomainWarehouse.Warehouse> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Warehouses.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(DomainWarehouse.Warehouse entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Warehouses.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(DomainWarehouse.Warehouse entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Warehouses.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<DomainWarehouse.Warehouse> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Warehouses.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<DomainWarehouse.Warehouse, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Warehouses.Where(w => !w.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<DomainWarehouse.Warehouse, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .Where(w => !w.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Warehouses
            .AnyAsync(w => w.Code == code && !w.IsDeleted, cancellationToken);
    }

    public IQueryable<DomainWarehouse.Warehouse> Query()
    {
        return _context.Warehouses.Where(w => !w.IsDeleted);
    }
}
