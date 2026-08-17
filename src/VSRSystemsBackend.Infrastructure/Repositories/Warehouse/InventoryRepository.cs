using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using DomainWarehouse = VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;

    public InventoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainWarehouse.InventoryItem?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => !i.IsDeleted)
            .OrderBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => i.WarehouseId == warehouseId && !i.IsDeleted)
            .OrderBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetByWarehouseAndLocationAsync(string warehouseId, string location, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => i.WarehouseId == warehouseId && i.Location == location && !i.IsDeleted)
            .OrderBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<DomainWarehouse.InventoryItem?> GetBySkuAsync(string sku, string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .FirstOrDefaultAsync(i => i.Sku == sku && i.WarehouseId == warehouseId && !i.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetLowStockAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => i.WarehouseId == warehouseId && i.Qty > 0 && i.Qty <= i.ReorderLevel && !i.IsDeleted)
            .OrderBy(i => i.Qty)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetOutOfStockAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => i.WarehouseId == warehouseId && i.Qty <= 0 && !i.IsDeleted)
            .OrderBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalStockValueAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => i.WarehouseId == warehouseId && !i.IsDeleted)
            .SumAsync(i => (int)(i.Qty * i.UnitPrice), cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetStockByCategoryAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => i.WarehouseId == warehouseId && !i.IsDeleted)
            .GroupBy(i => i.Category)
            .Select(g => new { Category = g.Key, Count = g.Sum(i => i.Qty) })
            .ToDictionaryAsync(x => x.Category, x => x.Count, cancellationToken);
    }

    public async Task<IReadOnlyList<DomainWarehouse.InventoryItem>> FindAsync(Expression<Func<DomainWarehouse.InventoryItem, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => !i.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<DomainWarehouse.InventoryItem> AddAsync(DomainWarehouse.InventoryItem entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.InventoryItems.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<DomainWarehouse.InventoryItem> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.InventoryItems.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(DomainWarehouse.InventoryItem entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.InventoryItems.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(DomainWarehouse.InventoryItem entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.InventoryItems.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<DomainWarehouse.InventoryItem> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.InventoryItems.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<DomainWarehouse.InventoryItem, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.InventoryItems.Where(i => !i.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<DomainWarehouse.InventoryItem, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .Where(i => !i.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<DomainWarehouse.InventoryItem> Query()
    {
        return _context.InventoryItems.Where(i => !i.IsDeleted);
    }

    public async Task ReserveStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default)
    {
        var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.Id == itemId && !i.IsDeleted, cancellationToken);
        if (item != null)
        {
            item.Reserved += quantity;
            item.UpdatedAt = DateTime.UtcNow;
            _context.InventoryItems.Update(item);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task ReleaseReservedStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default)
    {
        var item = await _context.InventoryItems.FirstOrDefaultAsync(i => i.Id == itemId && !i.IsDeleted, cancellationToken);
        if (item != null)
        {
            item.Reserved = Math.Max(0, item.Reserved - quantity);
            item.UpdatedAt = DateTime.UtcNow;
            _context.InventoryItems.Update(item);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
