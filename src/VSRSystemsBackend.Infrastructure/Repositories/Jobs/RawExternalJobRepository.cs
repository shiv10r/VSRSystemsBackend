using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class RawExternalJobRepository : IRawExternalJobRepository
{
    private readonly AppDbContext _context;

    public RawExternalJobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RawExternalJob?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.RawExternalJobs.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<RawExternalJob>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RawExternalJobs
            .Where(j => !j.IsDeleted)
            .OrderByDescending(j => j.FetchedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<RawExternalJob?> GetBySourceAndExternalIdAsync(string jobSourceId, string externalJobId, CancellationToken cancellationToken = default)
    {
        return await _context.RawExternalJobs
            .FirstOrDefaultAsync(j => j.JobSourceId == jobSourceId && j.ExternalJobId == externalJobId && !j.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<RawExternalJob>> GetBySourceIdAsync(string jobSourceId, int limit = 100, CancellationToken cancellationToken = default)
    {
        return await _context.RawExternalJobs
            .Where(j => j.JobSourceId == jobSourceId && !j.IsDeleted)
            .OrderByDescending(j => j.FetchedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RawExternalJob>> GetByProcessingStatusAsync(string status, int limit = 100, CancellationToken cancellationToken = default)
    {
        return await _context.RawExternalJobs
            .Where(j => j.ProcessingStatus == status && !j.IsDeleted)
            .OrderBy(j => j.FetchedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RawExternalJob>> FindAsync(Expression<Func<RawExternalJob, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.RawExternalJobs
            .Where(j => !j.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<RawExternalJob> AddAsync(RawExternalJob entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.RawExternalJobs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<RawExternalJob> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.RawExternalJobs.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RawExternalJob entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.RawExternalJobs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(RawExternalJob entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.RawExternalJobs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<RawExternalJob> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.RawExternalJobs.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<RawExternalJob, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.RawExternalJobs.Where(j => !j.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<RawExternalJob, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.RawExternalJobs
            .Where(j => !j.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<RawExternalJob> Query()
    {
        return _context.RawExternalJobs.Where(j => !j.IsDeleted);
    }
}

public class ScrapeRunRepository : IScrapeRunRepository
{
    private readonly AppDbContext _context;

    public ScrapeRunRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ScrapeRun?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeRuns.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<ScrapeRun>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeRuns
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.StartedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ScrapeRun>> GetBySourceIdAsync(string jobSourceId, int limit = 50, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeRuns
            .Where(r => r.JobSourceId == jobSourceId && !r.IsDeleted)
            .OrderByDescending(r => r.StartedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<ScrapeRun?> GetLatestRunAsync(string jobSourceId, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeRuns
            .Where(r => r.JobSourceId == jobSourceId && !r.IsDeleted)
            .OrderByDescending(r => r.StartedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ScrapeRun>> FindAsync(Expression<Func<ScrapeRun, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeRuns
            .Where(r => !r.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ScrapeRun> AddAsync(ScrapeRun entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.ScrapeRuns.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<ScrapeRun> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.ScrapeRuns.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ScrapeRun entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ScrapeRuns.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ScrapeRun entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ScrapeRuns.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<ScrapeRun> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.ScrapeRuns.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<ScrapeRun, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.ScrapeRuns.Where(r => !r.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<ScrapeRun, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeRuns
            .Where(r => !r.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<ScrapeRun> Query()
    {
        return _context.ScrapeRuns.Where(r => !r.IsDeleted);
    }
}

public class ScrapeLogRepository : IScrapeLogRepository
{
    private readonly AppDbContext _context;

    public ScrapeLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ScrapeLog?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeLogs.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<ScrapeLog>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeLogs
            .Where(l => !l.IsDeleted)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ScrapeLog>> GetByRunIdAsync(string scrapeRunId, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeLogs
            .Where(l => l.ScrapeRunId == scrapeRunId && !l.IsDeleted)
            .OrderBy(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ScrapeLog>> FindAsync(Expression<Func<ScrapeLog, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeLogs
            .Where(l => !l.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ScrapeLog> AddAsync(ScrapeLog entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.ScrapeLogs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<ScrapeLog> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.ScrapeLogs.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ScrapeLog entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ScrapeLogs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ScrapeLog entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ScrapeLogs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<ScrapeLog> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.ScrapeLogs.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<ScrapeLog, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.ScrapeLogs.Where(l => !l.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<ScrapeLog, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ScrapeLogs
            .Where(l => !l.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<ScrapeLog> Query()
    {
        return _context.ScrapeLogs.Where(l => !l.IsDeleted);
    }
}