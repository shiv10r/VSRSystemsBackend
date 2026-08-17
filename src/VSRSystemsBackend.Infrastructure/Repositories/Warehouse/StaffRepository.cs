using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.Warehouse;

public class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StaffMember?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.StaffMembers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<StaffMember>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StaffMembers
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StaffMember>> GetActiveStaffAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StaffMembers
            .Where(s => s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<StaffMember>> FindAsync(Expression<Func<StaffMember, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StaffMembers
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<StaffMember> AddAsync(StaffMember entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.StaffMembers.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<StaffMember> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.StaffMembers.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(StaffMember entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StaffMembers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(StaffMember entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StaffMembers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<StaffMember> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.StaffMembers.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task<int> CountAsync(Expression<Func<StaffMember, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.StaffMembers.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<StaffMember, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.StaffMembers
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<StaffMember> Query()
    {
        return _context.StaffMembers.Where(s => !s.IsDeleted);
    }
}
