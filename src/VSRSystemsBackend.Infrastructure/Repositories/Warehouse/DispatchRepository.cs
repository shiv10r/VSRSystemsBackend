using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class DispatchRepository : IDispatchRepository
{
    private readonly AppDbContext _context;

    public DispatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Dispatch?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Dispatches
            .FirstOrDefaultAsync(d => d.Id == (string)id && !d.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Dispatch>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Dispatches
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.DispatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Dispatch>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Dispatches
            .Where(d => d.OrderId == orderId && !d.IsDeleted)
            .OrderByDescending(d => d.DispatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Dispatch>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Dispatches
            .Where(d => d.Status == status && !d.IsDeleted)
            .OrderByDescending(d => d.DispatchDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dispatch?> GetByDispatchNumberAsync(string dispatchNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Dispatches
            .FirstOrDefaultAsync(d => d.DispatchNumber == dispatchNumber && !d.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Dispatch>> FindAsync(Expression<Func<Dispatch, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Dispatches
            .Where(d => !d.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dispatch> AddAsync(Dispatch entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Dispatches.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Dispatch> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Dispatches.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(Dispatch entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Dispatches.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Dispatch entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Dispatches.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<Dispatch> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Dispatches.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<Dispatch, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Dispatches.Where(d => !d.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Dispatch, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Dispatches
            .Where(d => !d.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Dispatch> Query()
    {
        return _context.Dispatches.Where(d => !d.IsDeleted);
    }
}
