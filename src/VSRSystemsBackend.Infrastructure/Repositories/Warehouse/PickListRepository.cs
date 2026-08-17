using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class PickListRepository : IPickListRepository
{
    private readonly AppDbContext _context;

    public PickListRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PickList?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.PickLists
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == (string)id && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<PickList>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PickLists
            .Where(p => !p.IsDeleted)
            .Include(p => p.Items)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PickList>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        return await _context.PickLists
            .Where(p => p.OrderId == orderId && !p.IsDeleted)
            .Include(p => p.Items)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PickList>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.PickLists
            .Where(p => p.Status == status && !p.IsDeleted)
            .Include(p => p.Items)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PickList?> GetByPickNumberAsync(string pickNumber, CancellationToken cancellationToken = default)
    {
        return await _context.PickLists
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.PickNumber == pickNumber && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<PickList>> FindAsync(Expression<Func<PickList, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PickLists
            .Where(p => !p.IsDeleted)
            .Where(predicate)
            .Include(p => p.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<PickList> AddAsync(PickList entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.PickLists.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<PickList> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.PickLists.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(PickList entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PickLists.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(PickList entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PickLists.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<PickList> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.PickLists.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<PickList, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.PickLists.Where(p => !p.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<PickList, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PickLists
            .Where(p => !p.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<PickList> Query()
    {
        return _context.PickLists.Where(p => !p.IsDeleted);
    }
}
