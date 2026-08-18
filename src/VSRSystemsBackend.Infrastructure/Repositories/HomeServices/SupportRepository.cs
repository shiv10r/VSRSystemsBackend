using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.HomeServices;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> FindAsync(Expression<Func<Review, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews
            .Where(r => !r.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Review> AddAsync(Review entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.HomeServiceReviews.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Review> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.HomeServiceReviews.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Review entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceReviews.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Review entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceReviews.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Review> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.HomeServiceReviews.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Review, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.HomeServiceReviews.Where(r => !r.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews
            .Where(r => !r.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Review> Query()
    {
        return _context.HomeServiceReviews.Where(r => !r.IsDeleted);
    }

    public async Task<IReadOnlyList<Review>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews
            .Where(r => r.ProfessionalId == professionalId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        var bookingIds = _context.HomeServiceBookings
            .Where(b => b.ServiceId == serviceId && !b.IsDeleted)
            .Select(b => b.Id);
        return await _context.HomeServiceReviews
            .Where(r => !r.IsDeleted && bookingIds.Contains(r.BookingId))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Review?> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews
            .FirstOrDefaultAsync(r => r.BookingId == bookingId && !r.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsForBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews
            .AnyAsync(r => r.BookingId == bookingId && !r.IsDeleted, cancellationToken);
    }

    public async Task<double> GetAverageRatingAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var average = await _context.HomeServiceReviews
            .Where(r => r.ProfessionalId == professionalId && !r.IsDeleted)
            .AverageAsync(r => (double?)r.Rating, cancellationToken);
        return average ?? 0;
    }

    public async Task<IReadOnlyList<Review>> GetRecentAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceReviews
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}

public class SupportRepository : ISupportRepository
{
    private readonly AppDbContext _context;

    public SupportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SupportTicket?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> FindAsync(Expression<Func<SupportTicket, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .Where(t => !t.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<SupportTicket> AddAsync(SupportTicket entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.SupportTickets.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<SupportTicket> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.SupportTickets.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SupportTicket entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SupportTickets.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SupportTicket entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SupportTickets.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<SupportTicket> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.SupportTickets.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<SupportTicket, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.SupportTickets.Where(t => !t.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<SupportTicket, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .Where(t => !t.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<SupportTicket> Query()
    {
        return _context.SupportTickets.Where(t => !t.IsDeleted);
    }

    public async Task<SupportTicket?> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber && !t.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .Where(t => t.RaisedBy == customerId && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .Where(t => t.Status == status && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.SupportTickets
            .Where(t => t.BookingId == bookingId && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class DisputeRepository : IDisputeRepository
{
    private readonly AppDbContext _context;

    public DisputeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Dispute?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Disputes.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Dispute>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Disputes
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Dispute>> FindAsync(Expression<Func<Dispute, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Disputes
            .Where(d => !d.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dispute> AddAsync(Dispute entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Disputes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Dispute> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Disputes.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Dispute entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Disputes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Dispute entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Disputes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Dispute> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Disputes.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Dispute, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Disputes.Where(d => !d.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Dispute, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Disputes
            .Where(d => !d.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Dispute> Query()
    {
        return _context.Disputes.Where(d => !d.IsDeleted);
    }

    public async Task<IReadOnlyList<Dispute>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.Disputes
            .Where(d => d.BookingId == bookingId && !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Dispute>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Disputes
            .Where(d => d.Status == status && !d.IsDeleted)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceNotifications.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceNotifications
            .Where(n => !n.IsDeleted)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> FindAsync(Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceNotifications
            .Where(n => !n.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Notification> AddAsync(Notification entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.HomeServiceNotifications.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Notification> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.HomeServiceNotifications.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceNotifications.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Notification entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceNotifications.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Notification> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.HomeServiceNotifications.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Notification, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.HomeServiceNotifications.Where(n => !n.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceNotifications
            .Where(n => !n.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Notification> Query()
    {
        return _context.HomeServiceNotifications.Where(n => !n.IsDeleted);
    }

    public async Task<IReadOnlyList<Notification>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceNotifications
            .Where(n => n.UserId == userId && !n.IsDeleted)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceNotifications
            .CountAsync(n => n.UserId == userId && n.ReadAt == null && !n.IsDeleted, cancellationToken);
    }
}