using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly AppDbContext _context;

    public StockMovementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StockMovement?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<StockMovement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockMovement>> GetByItemIdAsync(string itemId, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .Where(s => s.ItemId == itemId && !s.IsDeleted)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockMovement>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .Where(s => s.Date >= from && s.Date <= to && !s.IsDeleted)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockMovement>> GetByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .Where(s => s.Type == type && !s.IsDeleted)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockMovement>> FindAsync(Expression<Func<StockMovement, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockMovement> AddAsync(StockMovement entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.StockMovements.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<StockMovement> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.StockMovements.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(StockMovement entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockMovements.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(StockMovement entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockMovements.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<StockMovement> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.StockMovements.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<StockMovement, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.StockMovements.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<StockMovement, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockMovements
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<StockMovement> Query()
    {
        return _context.StockMovements.Where(s => !s.IsDeleted);
    }
}
