using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _context;

    public JobRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Job?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Job>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => !j.IsDeleted)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Job>> GetActiveJobsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => j.Status == "published" && !j.IsDeleted
                && (j.ExpiresAt == null || j.ExpiresAt >= DateTime.UtcNow))
            .OrderByDescending(j => j.PublishedAt ?? j.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Job>> GetByCompanyIdAsync(string companyId, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => j.CompanyId == companyId && !j.IsDeleted)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Job>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => j.Category == category && !j.IsDeleted)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Job>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => !j.IsDeleted
                && (j.Title.Contains(searchTerm)
                    || j.Description.Contains(searchTerm)
                    || j.Requirements.Contains(searchTerm)
                    || j.Category.Contains(searchTerm)
                    || j.Location.Contains(searchTerm)))
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Job?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .FirstOrDefaultAsync(j => j.Slug == slug && !j.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Job>> FindAsync(Expression<Func<Job, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => !j.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Job> AddAsync(Job entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Jobs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Job> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Jobs.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Job entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Jobs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Job entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Jobs.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Job> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Jobs.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Job, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Jobs.Where(j => !j.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Job, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Where(j => !j.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Job> Query()
    {
        return _context.Jobs.Where(j => !j.IsDeleted);
    }
}
