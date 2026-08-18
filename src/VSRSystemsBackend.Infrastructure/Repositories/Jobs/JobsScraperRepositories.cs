using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class JobSourceMappingRepository : IJobSourceMappingRepository
{
    private readonly AppDbContext _context;

    public JobSourceMappingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JobSourceMapping?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceMappings.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<JobSourceMapping>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceMappings
            .Where(m => !m.IsDeleted)
            .OrderByDescending(m => m.FirstSeenAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobSourceMapping?> GetBySourceAndExternalIdAsync(string jobSourceId, string externalJobId, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceMappings
            .FirstOrDefaultAsync(m => m.JobSourceId == jobSourceId && m.ExternalJobId == externalJobId && !m.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<JobSourceMapping>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceMappings
            .Where(m => m.JobId == jobId && !m.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobSourceMapping>> FindAsync(Expression<Func<JobSourceMapping, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceMappings
            .Where(m => !m.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobSourceMapping> AddAsync(JobSourceMapping entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.JobSourceMappings.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<JobSourceMapping> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.JobSourceMappings.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(JobSourceMapping entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobSourceMappings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(JobSourceMapping entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobSourceMappings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<JobSourceMapping> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.JobSourceMappings.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<JobSourceMapping, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.JobSourceMappings.Where(m => !m.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<JobSourceMapping, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobSourceMappings
            .Where(m => !m.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<JobSourceMapping> Query()
    {
        return _context.JobSourceMappings.Where(m => !m.IsDeleted);
    }
}

public class DuplicateCandidateRepository : IDuplicateCandidateRepository
{
    private readonly AppDbContext _context;

    public DuplicateCandidateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DuplicateCandidate?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.DuplicateCandidates.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<DuplicateCandidate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DuplicateCandidates
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DuplicateCandidate>> GetPendingAsync(int limit = 50, CancellationToken cancellationToken = default)
    {
        return await _context.DuplicateCandidates
            .Where(d => d.Status == "Pending" && !d.IsDeleted)
            .OrderByDescending(d => d.Score)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DuplicateCandidate>> FindAsync(Expression<Func<DuplicateCandidate, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.DuplicateCandidates
            .Where(d => !d.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<DuplicateCandidate> AddAsync(DuplicateCandidate entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.DuplicateCandidates.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<DuplicateCandidate> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.DuplicateCandidates.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(DuplicateCandidate entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.DuplicateCandidates.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(DuplicateCandidate entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.DuplicateCandidates.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<DuplicateCandidate> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.DuplicateCandidates.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<DuplicateCandidate, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.DuplicateCandidates.Where(d => !d.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<DuplicateCandidate, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.DuplicateCandidates
            .Where(d => !d.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<DuplicateCandidate> Query()
    {
        return _context.DuplicateCandidates.Where(d => !d.IsDeleted);
    }
}

public class IngestionErrorRepository : IIngestionErrorRepository
{
    private readonly AppDbContext _context;

    public IngestionErrorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IngestionError?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.IngestionErrors.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<IngestionError>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.IngestionErrors
            .Where(e => !e.IsDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<IngestionError>> GetBySourceIdAsync(string jobSourceId, int limit = 50, CancellationToken cancellationToken = default)
    {
        return await _context.IngestionErrors
            .Where(e => e.JobSourceId == jobSourceId && !e.IsDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<IngestionError>> FindAsync(Expression<Func<IngestionError, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.IngestionErrors
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IngestionError> AddAsync(IngestionError entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.IngestionErrors.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<IngestionError> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.IngestionErrors.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(IngestionError entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.IngestionErrors.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(IngestionError entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.IngestionErrors.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<IngestionError> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.IngestionErrors.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<IngestionError, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.IngestionErrors.Where(e => !e.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<IngestionError, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.IngestionErrors
            .Where(e => !e.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<IngestionError> Query()
    {
        return _context.IngestionErrors.Where(e => !e.IsDeleted);
    }
}