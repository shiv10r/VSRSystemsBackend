using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class GrnRepository : IGrnRepository
{
    private readonly AppDbContext _context;

    public GrnRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GrnRecord?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.GrnRecords
            .Include(g => g.Lines)
                .ThenInclude(l => l.Putaway)
            .FirstOrDefaultAsync(g => g.Id == (string)id && !g.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<GrnRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.GrnRecords
            .Where(g => !g.IsDeleted)
            .Include(g => g.Lines)
                .ThenInclude(l => l.Putaway)
            .OrderByDescending(g => g.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<GrnRecord>> GetByPoIdAsync(string poId, CancellationToken cancellationToken = default)
    {
        return await _context.GrnRecords
            .Where(g => g.PoId == poId && !g.IsDeleted)
            .Include(g => g.Lines)
                .ThenInclude(l => l.Putaway)
            .OrderByDescending(g => g.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<GrnRecord>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.GrnRecords
            .Where(g => g.PurchaseOrder!.WarehouseId == warehouseId && !g.IsDeleted)
            .Include(g => g.Lines)
                .ThenInclude(l => l.Putaway)
            .OrderByDescending(g => g.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<GrnRecord?> GetByGrnNumberAsync(string grnNumber, CancellationToken cancellationToken = default)
    {
        return await _context.GrnRecords
            .Include(g => g.Lines)
                .ThenInclude(l => l.Putaway)
            .FirstOrDefaultAsync(g => g.GrnNumber == grnNumber && !g.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<GrnRecord>> FindAsync(Expression<Func<GrnRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.GrnRecords
            .Where(g => !g.IsDeleted)
            .Where(predicate)
            .Include(g => g.Lines)
                .ThenInclude(l => l.Putaway)
            .ToListAsync(cancellationToken);
    }

    public async Task<GrnRecord> AddAsync(GrnRecord entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.GrnRecords.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<GrnRecord> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.GrnRecords.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(GrnRecord entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.GrnRecords.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(GrnRecord entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.GrnRecords.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<GrnRecord> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.GrnRecords.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<GrnRecord, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.GrnRecords.Where(g => !g.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<GrnRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.GrnRecords
            .Where(g => !g.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<GrnRecord> Query()
    {
        return _context.GrnRecords.Where(g => !g.IsDeleted);
    }
}
