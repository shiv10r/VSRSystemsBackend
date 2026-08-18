using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _context;

    public JobApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JobApplication?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<JobApplication>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobApplication>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(a => a.JobId == jobId && !a.IsDeleted)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobApplication>> GetByCandidateIdAsync(string candidateId, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(a => a.CandidateId == candidateId && !a.IsDeleted)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<JobApplication>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(a => a.Status == status && !a.IsDeleted)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobApplication?> GetByJobAndCandidateAsync(string jobId, string candidateId, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(a => a.JobId == jobId && a.CandidateId == candidateId && !a.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<JobApplication>> FindAsync(Expression<Func<JobApplication, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(a => !a.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobApplication> AddAsync(JobApplication entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.JobApplications.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<JobApplication> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.JobApplications.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(JobApplication entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobApplications.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(JobApplication entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.JobApplications.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<JobApplication> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.JobApplications.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<JobApplication, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.JobApplications.Where(a => !a.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<JobApplication, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .Where(a => !a.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<JobApplication> Query()
    {
        return _context.JobApplications.Where(a => !a.IsDeleted);
    }
}
