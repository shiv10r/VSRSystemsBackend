using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class SavedJobRepository : ISavedJobRepository
{
    private readonly AppDbContext _context;

    public SavedJobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SavedJob?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.SavedJobs.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<SavedJob>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SavedJobs
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.SavedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SavedJob>> GetByCandidateIdAsync(string candidateId, CancellationToken cancellationToken = default)
    {
        return await _context.SavedJobs
            .Where(s => s.CandidateId == candidateId && !s.IsDeleted)
            .OrderByDescending(s => s.SavedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SavedJob?> GetByCandidateAndJobAsync(string candidateId, string jobId, CancellationToken cancellationToken = default)
    {
        return await _context.SavedJobs
            .FirstOrDefaultAsync(s => s.CandidateId == candidateId && s.JobId == jobId && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<SavedJob>> FindAsync(Expression<Func<SavedJob, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.SavedJobs
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<SavedJob> AddAsync(SavedJob entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.SavedJobs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<SavedJob> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.SavedJobs.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SavedJob entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SavedJobs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SavedJob entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SavedJobs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<SavedJob> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.SavedJobs.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<SavedJob, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.SavedJobs.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<SavedJob, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.SavedJobs
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<SavedJob> Query()
    {
        return _context.SavedJobs.Where(s => !s.IsDeleted);
    }
}
