using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class StockAdjustmentRepository : IStockAdjustmentRepository
{
    private readonly AppDbContext _context;

    public StockAdjustmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StockAdjustment?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.StockAdjustments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<StockAdjustment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StockAdjustments
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockAdjustment>> GetByItemIdAsync(string itemId, CancellationToken cancellationToken = default)
    {
        return await _context.StockAdjustments
            .Where(s => s.ItemId == itemId && !s.IsDeleted)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockAdjustment>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _context.StockAdjustments
            .Where(s => s.Date >= from && s.Date <= to && !s.IsDeleted)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockAdjustment>> FindAsync(Expression<Func<StockAdjustment, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockAdjustments
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockAdjustment> AddAsync(StockAdjustment entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.StockAdjustments.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<StockAdjustment> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.StockAdjustments.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(StockAdjustment entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockAdjustments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(StockAdjustment entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockAdjustments.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<StockAdjustment> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.StockAdjustments.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<StockAdjustment, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.StockAdjustments.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<StockAdjustment, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockAdjustments
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<StockAdjustment> Query()
    {
        return _context.StockAdjustments.Where(s => !s.IsDeleted);
    }
}
