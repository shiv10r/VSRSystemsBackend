using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.HomeServices;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> FindAsync(Expression<Func<Booking, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Booking> AddAsync(Booking entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.HomeServiceBookings.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Booking> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.HomeServiceBookings.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Booking entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceBookings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Booking entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceBookings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Booking> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.HomeServiceBookings.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Booking, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.HomeServiceBookings.Where(b => !b.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Booking> Query()
    {
        return _context.HomeServiceBookings.Where(b => !b.IsDeleted);
    }

    public async Task<Booking?> GetWithDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Include(b => b.Customer)
            .Include(b => b.Service)
            .Include(b => b.Package)
            .Include(b => b.Items)
            .Include(b => b.AddOns)
            .Include(b => b.Materials)
            .Include(b => b.Assignments)
            .Include(b => b.StatusHistory)
            .Include(b => b.Notes)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted, cancellationToken);
    }

    public async Task<Booking?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber && !b.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.CustomerId == customerId && !b.IsDeleted)
            .OrderByDescending(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.AssignedProfessionalId == professionalId && !b.IsDeleted)
            .OrderByDescending(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.Status == status && !b.IsDeleted)
            .OrderByDescending(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetUpcomingForProfessionalAsync(string professionalId, DateTime from, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.AssignedProfessionalId == professionalId && b.ScheduledStart >= from
                && b.Status != "cancelled" && b.Status != "closed" && !b.IsDeleted)
            .OrderBy(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.ServiceId == serviceId && !b.IsDeleted)
            .OrderByDescending(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetInDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.ScheduledStart >= from && b.ScheduledStart <= to && !b.IsDeleted)
            .OrderBy(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> FindConflictsAsync(string professionalId, DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.AssignedProfessionalId == professionalId && b.ScheduledStart < end && b.ExpectedEnd > start
                && b.Status != "cancelled" && b.Status != "closed" && !b.IsDeleted)
            .OrderBy(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }

    public async Task AddStatusHistoryAsync(BookingStatusHistory history, CancellationToken cancellationToken = default)
    {
        history.CreatedAt = DateTime.UtcNow;
        await _context.BookingStatusHistories.AddAsync(history, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BookingStatusHistory>> GetStatusHistoryAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.BookingStatusHistories
            .Where(h => h.BookingId == bookingId && !h.IsDeleted)
            .OrderBy(h => h.ChangedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAssignmentAsync(BookingAssignment assignment, CancellationToken cancellationToken = default)
    {
        assignment.CreatedAt = DateTime.UtcNow;
        await _context.BookingAssignments.AddAsync(assignment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BookingAssignment>> GetAssignmentsAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.BookingAssignments
            .Where(a => a.BookingId == bookingId && !a.IsDeleted)
            .OrderBy(a => a.OfferedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> GetCustomerCompletedAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .Where(b => b.CustomerId == customerId && b.Status == "completed" && !b.IsDeleted)
            .OrderByDescending(b => b.ScheduledStart)
            .ToListAsync(cancellationToken);
    }
}

public class RecurringBookingRepository : IRecurringBookingRepository
{
    private readonly AppDbContext _context;

    public RecurringBookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RecurringBooking?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringBookings.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<RecurringBooking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RecurringBookings
            .Where(r => !r.IsDeleted)
            .OrderBy(r => r.NextRunAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RecurringBooking>> FindAsync(Expression<Func<RecurringBooking, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringBookings
            .Where(r => !r.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<RecurringBooking> AddAsync(RecurringBooking entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.RecurringBookings.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<RecurringBooking> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.RecurringBookings.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RecurringBooking entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.RecurringBookings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(RecurringBooking entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.RecurringBookings.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<RecurringBooking> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.RecurringBookings.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<RecurringBooking, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.RecurringBookings.Where(r => !r.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<RecurringBooking, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringBookings
            .Where(r => !r.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<RecurringBooking> Query()
    {
        return _context.RecurringBookings.Where(r => !r.IsDeleted);
    }

    public async Task<IReadOnlyList<RecurringBooking>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringBookings
            .Where(r => r.CustomerId == customerId && !r.IsDeleted)
            .OrderBy(r => r.NextRunAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RecurringBooking>> GetDueAsync(DateTime upTo, CancellationToken cancellationToken = default)
    {
        return await _context.RecurringBookings
            .Where(r => r.NextRunAt <= upTo && r.IsActive && !r.IsDeleted)
            .OrderBy(r => r.NextRunAt)
            .ToListAsync(cancellationToken);
    }
}

public class AmcContractRepository : IAmcContractRepository
{
    private readonly AppDbContext _context;

    public AmcContractRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AmcContract?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.AmcContracts.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<AmcContract>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AmcContracts
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AmcContract>> FindAsync(Expression<Func<AmcContract, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.AmcContracts
            .Where(c => !c.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<AmcContract> AddAsync(AmcContract entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.AmcContracts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<AmcContract> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.AmcContracts.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AmcContract entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.AmcContracts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(AmcContract entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.AmcContracts.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<AmcContract> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.AmcContracts.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<AmcContract, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AmcContracts.Where(c => !c.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<AmcContract, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.AmcContracts
            .Where(c => !c.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<AmcContract> Query()
    {
        return _context.AmcContracts.Where(c => !c.IsDeleted);
    }

    public async Task<IReadOnlyList<AmcContract>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.AmcContracts
            .Where(c => c.CustomerId == customerId && !c.IsDeleted)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AmcContract>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AmcContracts
            .Where(c => c.Status == "active" && !c.IsDeleted)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync(cancellationToken);
    }
}

public class PriceQuoteRepository : IPriceQuoteRepository
{
    private readonly AppDbContext _context;

    public PriceQuoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PriceQuote?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.PriceQuotes.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<PriceQuote>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PriceQuotes
            .Where(q => !q.IsDeleted)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PriceQuote>> FindAsync(Expression<Func<PriceQuote, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PriceQuotes
            .Where(q => !q.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<PriceQuote> AddAsync(PriceQuote entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.PriceQuotes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<PriceQuote> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.PriceQuotes.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PriceQuote entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PriceQuotes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(PriceQuote entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PriceQuotes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<PriceQuote> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.PriceQuotes.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<PriceQuote, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.PriceQuotes.Where(q => !q.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<PriceQuote, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PriceQuotes
            .Where(q => !q.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<PriceQuote> Query()
    {
        return _context.PriceQuotes.Where(q => !q.IsDeleted);
    }

    public async Task<PriceQuote?> GetByQuoteNumberAsync(string quoteNumber, CancellationToken cancellationToken = default)
    {
        return await _context.PriceQuotes
            .FirstOrDefaultAsync(q => q.QuoteNumber == quoteNumber && !q.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<PriceQuote>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.PriceQuotes
            .Where(q => q.CustomerId == customerId && !q.IsDeleted)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<QuoteRevision>> GetRevisionsAsync(string priceQuoteId, CancellationToken cancellationToken = default)
    {
        return await _context.QuoteRevisions
            .Where(r => r.PriceQuoteId == priceQuoteId && !r.IsDeleted)
            .OrderByDescending(r => r.RevisionNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<PriceQuote?> GetActiveForBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var quoteId = await _context.HomeServiceBookings
            .Where(b => b.Id == bookingId && !b.IsDeleted)
            .Select(b => b.CurrentQuoteId ?? b.PriceQuoteId)
            .FirstOrDefaultAsync(cancellationToken);

        if (quoteId == null)
            return null;

        return await _context.PriceQuotes
            .FirstOrDefaultAsync(q => q.Id == quoteId && q.Status == "active" && !q.IsDeleted, cancellationToken);
    }
}