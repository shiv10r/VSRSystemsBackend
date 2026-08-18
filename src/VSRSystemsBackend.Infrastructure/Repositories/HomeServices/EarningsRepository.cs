using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.HomeServices;

public class EarningsRepository : IEarningsRepository, IProfessionalEarningRepository
{
    private readonly AppDbContext _context;

    public EarningsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProfessionalEarning?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalEarnings.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalEarning>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalEarnings
            .Where(e => !e.IsDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalEarning>> FindAsync(Expression<Func<ProfessionalEarning, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalEarnings
            .Where(e => !e.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProfessionalEarning> AddAsync(ProfessionalEarning entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.ProfessionalEarnings.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<ProfessionalEarning> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.ProfessionalEarnings.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProfessionalEarning entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ProfessionalEarnings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ProfessionalEarning entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ProfessionalEarnings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<ProfessionalEarning> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.ProfessionalEarnings.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<ProfessionalEarning, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.ProfessionalEarnings.Where(e => !e.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<ProfessionalEarning, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalEarnings
            .Where(e => !e.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<ProfessionalEarning> Query()
    {
        return _context.ProfessionalEarnings.Where(e => !e.IsDeleted);
    }

    public async Task<IReadOnlyList<ProfessionalEarning>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalEarnings
            .Where(e => e.ProfessionalId == professionalId && !e.IsDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProfessionalEarning?> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalEarnings
            .FirstOrDefaultAsync(e => e.BookingId == bookingId && !e.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalEarning>> GetSettledInRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalEarnings
            .Where(e => e.Status == "settled" && e.SettledAt >= from && e.SettledAt <= to && !e.IsDeleted)
            .OrderBy(e => e.SettledAt)
            .ToListAsync(cancellationToken);
    }
}

public class PayoutRepository : IPayoutRepository
{
    private readonly AppDbContext _context;

    public PayoutRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payout?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Payout>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Payout>> FindAsync(Expression<Func<Payout, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Where(p => !p.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payout> AddAsync(Payout entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Payouts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Payout> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Payouts.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Payout entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Payouts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Payout entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Payouts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Payout> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Payouts.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Payout, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Payouts.Where(p => !p.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Payout, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Where(p => !p.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Payout> Query()
    {
        return _context.Payouts.Where(p => !p.IsDeleted);
    }

    public async Task<IReadOnlyList<Payout>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Where(p => p.ProfessionalId == professionalId && !p.IsDeleted)
            .OrderByDescending(p => p.PeriodEnd)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Payout>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Where(p => p.Status == status && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payout?> GetPendingForPeriodAsync(string professionalId, DateTime periodStart, DateTime periodEnd, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .FirstOrDefaultAsync(p => p.ProfessionalId == professionalId && p.PeriodStart == periodStart
                && p.PeriodEnd == periodEnd && p.Status == "pending" && !p.IsDeleted, cancellationToken);
    }
}