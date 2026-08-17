using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class StockCountRepository : IStockCountRepository
{
    private readonly AppDbContext _context;

    public StockCountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StockCount?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.StockCounts
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == (string)id && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<StockCount>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StockCounts
            .Where(s => !s.IsDeleted)
            .Include(s => s.Items)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockCount>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.StockCounts
            .Where(s => s.WarehouseId == warehouseId && !s.IsDeleted)
            .Include(s => s.Items)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockCount>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.StockCounts
            .Where(s => s.Status == status && !s.IsDeleted)
            .Include(s => s.Items)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockCount?> GetByCountNumberAsync(string countNumber, CancellationToken cancellationToken = default)
    {
        return await _context.StockCounts
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.CountNumber == countNumber && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<StockCount>> FindAsync(Expression<Func<StockCount, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockCounts
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .Include(s => s.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockCount> AddAsync(StockCount entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.StockCounts.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<StockCount> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.StockCounts.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(StockCount entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockCounts.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(StockCount entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockCounts.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<StockCount> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.StockCounts.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<StockCount, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.StockCounts.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<StockCount, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockCounts
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<StockCount> Query()
    {
        return _context.StockCounts.Where(s => !s.IsDeleted);
    }
}
