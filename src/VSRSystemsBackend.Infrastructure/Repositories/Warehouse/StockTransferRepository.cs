using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class StockTransferRepository : IStockTransferRepository
{
    private readonly AppDbContext _context;

    public StockTransferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StockTransfer?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == (string)id && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<StockTransfer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Where(s => !s.IsDeleted)
            .Include(s => s.Items)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockTransfer>> GetByFromWarehouseAsync(string fromWarehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Where(s => s.FromWarehouseId == fromWarehouseId && !s.IsDeleted)
            .Include(s => s.Items)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockTransfer>> GetByToWarehouseAsync(string toWarehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Where(s => s.ToWarehouseId == toWarehouseId && !s.IsDeleted)
            .Include(s => s.Items)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StockTransfer>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Where(s => s.Status == status && !s.IsDeleted)
            .Include(s => s.Items)
            .OrderByDescending(s => s.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockTransfer?> GetByTransferNumberAsync(string transferNumber, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.TransferNumber == transferNumber && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<StockTransfer>> FindAsync(Expression<Func<StockTransfer, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .Include(s => s.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<StockTransfer> AddAsync(StockTransfer entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.StockTransfers.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<StockTransfer> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.StockTransfers.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(StockTransfer entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockTransfers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(StockTransfer entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockTransfers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<StockTransfer> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.StockTransfers.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<StockTransfer, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.StockTransfers.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<StockTransfer, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StockTransfers
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<StockTransfer> Query()
    {
        return _context.StockTransfers.Where(s => !s.IsDeleted);
    }
}
