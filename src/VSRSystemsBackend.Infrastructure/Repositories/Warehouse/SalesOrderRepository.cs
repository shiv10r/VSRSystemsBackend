using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class SalesOrderRepository : ISalesOrderRepository
{
    private readonly AppDbContext _context;

    public SalesOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SalesOrder?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Include(s => s.Lines)
            .FirstOrDefaultAsync(s => s.Id == (string)id && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => !s.IsDeleted)
            .Include(s => s.Lines)
            .OrderByDescending(s => s.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => s.WarehouseId == warehouseId && !s.IsDeleted)
            .Include(s => s.Lines)
            .OrderByDescending(s => s.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => s.CustomerId == customerId && !s.IsDeleted)
            .Include(s => s.Lines)
            .OrderByDescending(s => s.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => s.Status == status && !s.IsDeleted)
            .Include(s => s.Lines)
            .OrderByDescending(s => s.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Include(s => s.Lines)
            .FirstOrDefaultAsync(s => s.OrderNumber == orderNumber && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetOrdersForPickingAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => s.WarehouseId == warehouseId 
                     && (s.Status == "reserved" || s.Status == "confirmed") 
                     && !s.IsDeleted)
            .Include(s => s.Lines)
            .OrderBy(s => s.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> GetOrdersForDispatchAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => s.WarehouseId == warehouseId 
                     && (s.Status == "packed" || s.Status == "ready") 
                     && !s.IsDeleted)
            .Include(s => s.Lines)
            .OrderBy(s => s.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalesOrder>> FindAsync(Expression<Func<SalesOrder, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .Include(s => s.Lines)
            .ToListAsync(cancellationToken);
    }

    public async Task<SalesOrder> AddAsync(SalesOrder entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.SalesOrders.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<SalesOrder> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.SalesOrders.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(SalesOrder entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SalesOrders.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(SalesOrder entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SalesOrders.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<SalesOrder> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.SalesOrders.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<SalesOrder, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.SalesOrders.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<SalesOrder, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<SalesOrder> Query()
    {
        return _context.SalesOrders.Where(s => !s.IsDeleted);
    }
}
