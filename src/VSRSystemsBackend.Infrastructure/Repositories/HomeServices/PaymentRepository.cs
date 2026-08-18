using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.HomeServices;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> FindAsync(Expression<Func<Payment, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Where(p => !p.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payment> AddAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Payments.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Payment> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Payments.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Payments.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Payments.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Payment> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Payments.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Payment, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Payments.Where(p => !p.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Payment, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Where(p => !p.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Payment> Query()
    {
        return _context.Payments.Where(p => !p.IsDeleted);
    }

    public async Task<Payment?> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Where(p => p.BookingId == bookingId && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Payment?> GetByGatewayOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.GatewayOrderId == orderId && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var bookingIds = _context.HomeServiceBookings
            .Where(b => b.CustomerId == customerId && !b.IsDeleted)
            .Select(b => b.Id);
        return await _context.Payments
            .Where(p => !p.IsDeleted && bookingIds.Contains(p.BookingId))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Where(p => p.Status == status && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class RefundRepository : IRefundRepository
{
    private readonly AppDbContext _context;

    public RefundRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Refund?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Refunds.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Refund>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Refund>> FindAsync(Expression<Func<Refund, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .Where(r => !r.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Refund> AddAsync(Refund entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Refunds.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Refund> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Refunds.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Refund entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Refunds.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Refund entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Refunds.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Refund> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Refunds.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Refund, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Refunds.Where(r => !r.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Refund, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .Where(r => !r.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Refund> Query()
    {
        return _context.Refunds.Where(r => !r.IsDeleted);
    }

    public async Task<IReadOnlyList<Refund>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .Where(r => r.BookingId == bookingId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Refund>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Refunds
            .Where(r => r.Status == status && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class CreditTransactionRepository : ICreditTransactionRepository
{
    private readonly AppDbContext _context;

    public CreditTransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreditTransaction?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.CreditTransactions.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<CreditTransaction>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CreditTransactions
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CreditTransaction>> FindAsync(Expression<Func<CreditTransaction, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.CreditTransactions
            .Where(t => !t.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<CreditTransaction> AddAsync(CreditTransaction entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.CreditTransactions.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<CreditTransaction> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.CreditTransactions.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CreditTransaction entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.CreditTransactions.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(CreditTransaction entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.CreditTransactions.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<CreditTransaction> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.CreditTransactions.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<CreditTransaction, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.CreditTransactions.Where(t => !t.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<CreditTransaction, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.CreditTransactions
            .Where(t => !t.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<CreditTransaction> Query()
    {
        return _context.CreditTransactions.Where(t => !t.IsDeleted);
    }

    public async Task<IReadOnlyList<CreditTransaction>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.CreditTransactions
            .Where(t => t.CustomerId == customerId && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetWalletBalanceAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.CreditTransactions
            .Where(t => t.CustomerId == customerId && !t.IsDeleted)
            .SumAsync(t => (decimal?)(t.Type == "credit" ? t.Amount : -t.Amount), cancellationToken) ?? 0;
    }
}