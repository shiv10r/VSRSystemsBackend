using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Supplier>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Supplier?> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Gstin == gstin && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .Where(s => s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByGstinAsync(string gstin, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .AnyAsync(s => s.Gstin == gstin && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Supplier> AddAsync(Supplier entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Suppliers.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Supplier> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Suppliers.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(Supplier entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Suppliers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Supplier entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Suppliers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<Supplier> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Suppliers.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<Supplier, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Suppliers.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Supplier, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Suppliers
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Supplier> Query()
    {
        return _context.Suppliers.Where(s => !s.IsDeleted);
    }
}
