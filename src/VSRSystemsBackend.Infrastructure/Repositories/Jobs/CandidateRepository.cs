using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class CandidateRepository : ICandidateRepository
{
    private readonly AppDbContext _context;

    public CandidateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Candidate?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Candidates.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Candidate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Candidates
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Candidates
            .FirstOrDefaultAsync(c => c.Email == email && !c.IsDeleted, cancellationToken);
    }

    public async Task<Candidate?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return await _context.Candidates
            .FirstOrDefaultAsync(c => c.Phone == phone && !c.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Candidate>> FindAsync(Expression<Func<Candidate, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Candidates
            .Where(c => !c.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Candidate> AddAsync(Candidate entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Candidates.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Candidate> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Candidates.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(Candidate entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Candidates.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Candidate entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Candidates.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<Candidate> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Candidates.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<Candidate, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Candidates.Where(c => !c.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Candidate, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Candidates
            .Where(c => !c.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Candidate> Query()
    {
        return _context.Candidates.Where(c => !c.IsDeleted);
    }
}
