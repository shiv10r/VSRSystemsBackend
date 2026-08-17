using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly AppDbContext _context;

    public PurchaseOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrder?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.Id == (string)id && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(p => !p.IsDeleted)
            .Include(p => p.Lines)
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(p => p.WarehouseId == warehouseId && !p.IsDeleted)
            .Include(p => p.Lines)
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetBySupplierIdAsync(string supplierId, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(p => p.SupplierId == supplierId && !p.IsDeleted)
            .Include(p => p.Lines)
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(p => p.Status == status && !p.IsDeleted)
            .Include(p => p.Lines)
            .OrderByDescending(p => p.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByPoNumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.PoNumber == poNumber && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetPendingReceivingAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(p => p.WarehouseId == warehouseId 
                     && (p.Status == "approved" || p.Status == "partial") 
                     && !p.IsDeleted)
            .Include(p => p.Lines)
            .OrderBy(p => p.ExpectedDelivery)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrder>> FindAsync(Expression<Func<PurchaseOrder, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(p => !p.IsDeleted)
            .Where(predicate)
            .Include(p => p.Lines)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder> AddAsync(PurchaseOrder entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.PurchaseOrders.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<PurchaseOrder> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.PurchaseOrders.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(PurchaseOrder entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PurchaseOrders.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(PurchaseOrder entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PurchaseOrders.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<PurchaseOrder> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.PurchaseOrders.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<PurchaseOrder, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.PurchaseOrders.Where(p => !p.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<PurchaseOrder, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Where(p => !p.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<PurchaseOrder> Query()
    {
        return _context.PurchaseOrders.Where(p => !p.IsDeleted);
    }
}
