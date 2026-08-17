using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class ReturnRepository : IReturnRepository
{
    private readonly AppDbContext _context;

    public ReturnRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReturnRecord?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.ReturnRecords
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == (string)id && !r.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<ReturnRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ReturnRecords
            .Where(r => !r.IsDeleted)
            .Include(r => r.Items)
            .OrderByDescending(r => r.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ReturnRecord>> GetByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        return await _context.ReturnRecords
            .Where(r => r.Type == type && !r.IsDeleted)
            .Include(r => r.Items)
            .OrderByDescending(r => r.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ReturnRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.ReturnRecords
            .Where(r => r.Status == status && !r.IsDeleted)
            .Include(r => r.Items)
            .OrderByDescending(r => r.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<ReturnRecord?> GetByReturnNumberAsync(string returnNumber, CancellationToken cancellationToken = default)
    {
        return await _context.ReturnRecords
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.ReturnNumber == returnNumber && !r.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<ReturnRecord>> FindAsync(Expression<Func<ReturnRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ReturnRecords
            .Where(r => !r.IsDeleted)
            .Where(predicate)
            .Include(r => r.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<ReturnRecord> AddAsync(ReturnRecord entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.ReturnRecords.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<ReturnRecord> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.ReturnRecords.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(ReturnRecord entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ReturnRecords.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(ReturnRecord entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ReturnRecords.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<ReturnRecord> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.ReturnRecords.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<ReturnRecord, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.ReturnRecords.Where(r => !r.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<ReturnRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ReturnRecords
            .Where(r => !r.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<ReturnRecord> Query()
    {
        return _context.ReturnRecords.Where(r => !r.IsDeleted);
    }
}
