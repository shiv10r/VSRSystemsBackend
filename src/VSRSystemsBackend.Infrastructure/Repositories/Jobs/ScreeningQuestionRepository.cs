using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Jobs;

public class ScreeningQuestionRepository : IScreeningQuestionRepository
{
    private readonly AppDbContext _context;

    public ScreeningQuestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ScreeningQuestion?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.ScreeningQuestions.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<ScreeningQuestion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ScreeningQuestions
            .Where(q => !q.IsDeleted)
            .OrderBy(q => q.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ScreeningQuestion>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken = default)
    {
        return await _context.ScreeningQuestions
            .Where(q => q.JobId == jobId && !q.IsDeleted)
            .OrderBy(q => q.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ScreeningQuestion>> FindAsync(Expression<Func<ScreeningQuestion, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ScreeningQuestions
            .Where(q => !q.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ScreeningQuestion> AddAsync(ScreeningQuestion entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.ScreeningQuestions.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<ScreeningQuestion> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.ScreeningQuestions.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(ScreeningQuestion entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ScreeningQuestions.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(ScreeningQuestion entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ScreeningQuestions.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<ScreeningQuestion> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.ScreeningQuestions.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<ScreeningQuestion, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.ScreeningQuestions.Where(q => !q.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<ScreeningQuestion, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ScreeningQuestions
            .Where(q => !q.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<ScreeningQuestion> Query()
    {
        return _context.ScreeningQuestions.Where(q => !q.IsDeleted);
    }
}
