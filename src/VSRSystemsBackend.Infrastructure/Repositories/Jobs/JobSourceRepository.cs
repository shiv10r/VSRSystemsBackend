using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class JobSourceRepository : IJobSourceRepository
{
    private readonly AppDbContext _context;

    public JobSourceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JobSource?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.JobSources.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<JobSource>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.JobSources
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobSource?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.JobSources
            .FirstOrDefaultAsync(s => s.Slug == slug && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<JobSource>> GetEnabledSourcesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.JobSources
            .Where(s => s.IsEnabled && !s.IsDeleted && s.HealthStatus != JobSourceHealth.Disabled)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobSource>> GetDueSourcesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.JobSources
            .Where(s => s.IsEnabled && !s.IsDeleted
                && s.HealthStatus != JobSourceHealth.Disabled
                && s.HealthStatus != JobSourceHealth.Paused
                && (s.LastSuccessfulRunAt == null
                    || (s.LastFailedRunAt != null && s.LastFailedRunAt > s.LastSuccessfulRunAt)
                    || s.LastSuccessfulRunAt <= now.AddMinutes(-s.RequestIntervalMinutes)))
            .OrderBy(s => s.LastSuccessfulRunAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobSource>> FindAsync(Expression<Func<JobSource, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobSources
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobSource> AddAsync(JobSource entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.JobSources.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<JobSource> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.JobSources.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(JobSource entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobSources.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(JobSource entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobSources.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<JobSource> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.JobSources.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<JobSource, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.JobSources.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<JobSource, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobSources
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<JobSource> Query()
    {
        return _context.JobSources.Where(s => !s.IsDeleted);
    }
}

public class JobSourceConfigRepository : IJobSourceConfigRepository
{
    private readonly AppDbContext _context;

    public JobSourceConfigRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JobSourceConfig?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceConfigs.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<JobSourceConfig>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceConfigs
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.JobSourceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobSourceConfig?> GetActiveConfigAsync(string jobSourceId, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceConfigs
            .FirstOrDefaultAsync(c => c.JobSourceId == jobSourceId && c.IsActive && !c.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<JobSourceConfig>> FindAsync(Expression<Func<JobSourceConfig, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceConfigs
            .Where(c => !c.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobSourceConfig> AddAsync(JobSourceConfig entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.JobSourceConfigs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<JobSourceConfig> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.JobSourceConfigs.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(JobSourceConfig entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobSourceConfigs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(JobSourceConfig entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobSourceConfigs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<JobSourceConfig> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.JobSourceConfigs.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<JobSourceConfig, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.JobSourceConfigs.Where(c => !c.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<JobSourceConfig, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceConfigs
            .Where(c => !c.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<JobSourceConfig> Query()
    {
        return _context.JobSourceConfigs.Where(c => !c.IsDeleted);
    }
}