using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectRecord?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Projects.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Where(p => p.Status == status && !p.IsDeleted)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectRecord>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Where(p => p.WarehouseId == warehouseId && !p.IsDeleted)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectRecord>> FindAsync(Expression<Func<ProjectRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Where(p => !p.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectRecord> AddAsync(ProjectRecord entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Projects.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<ProjectRecord> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Projects.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(ProjectRecord entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Projects.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(ProjectRecord entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Projects.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<ProjectRecord> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Projects.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<ProjectRecord, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Projects.Where(p => !p.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<ProjectRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Where(p => !p.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<ProjectRecord> Query()
    {
        return _context.Projects.Where(p => !p.IsDeleted);
    }
}
