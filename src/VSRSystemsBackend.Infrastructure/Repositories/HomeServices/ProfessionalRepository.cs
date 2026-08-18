using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.HomeServices;

public class ProfessionalRepository : IProfessionalRepository
{
    private readonly AppDbContext _context;

    public ProfessionalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Professional?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Professional>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Professional>> FindAsync(Expression<Func<Professional, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Where(p => !p.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Professional> AddAsync(Professional entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Professionals.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Professional> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Professionals.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Professional entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Professionals.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Professional entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Professionals.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Professional> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Professionals.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Professional, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Professionals.Where(p => !p.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Professional, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Where(p => !p.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Professional> Query()
    {
        return _context.Professionals.Where(p => !p.IsDeleted);
    }

    public async Task<IReadOnlyList<Professional>> GetVerifiedProfessionalsByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Where(p => p.OnboardingStatus == "verified" && !p.IsDeleted && p.Skills.Any(s => s.ServiceId == serviceId && !s.IsDeleted))
            .OrderBy(p => p.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Professional>> GetEligibleProfessionalsAsync(string serviceId, string cityId, string zoneId, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Where(p => p.OnboardingStatus == "verified" && !p.IsDeleted
                && p.Skills.Any(s => s.ServiceId == serviceId && !s.IsDeleted)
                && p.ServiceAreas.Any(a => a.CityId == cityId && a.ZoneId == zoneId && a.IsActive && !a.IsDeleted))
            .OrderBy(p => p.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Professional?> GetWithDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Documents)
            .Include(p => p.Skills)
            .Include(p => p.ServiceAreas)
            .Include(p => p.Availabilities)
            .Include(p => p.TimeOffs)
            .Include(p => p.Performances)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalDocument>> GetDocumentsAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalDocuments
            .Where(d => d.ProfessionalId == professionalId && !d.IsDeleted)
            .OrderBy(d => d.DocType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalAvailability>> GetAvailabilitiesAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalAvailabilities
            .Where(a => a.ProfessionalId == professionalId && !a.IsDeleted)
            .OrderBy(a => a.DayOfWeek)
            .ThenBy(a => a.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalSkill>> GetSkillsAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalSkills
            .Where(s => s.ProfessionalId == professionalId && !s.IsDeleted)
            .OrderBy(s => s.ServiceId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalServiceArea>> GetServiceAreasAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalServiceAreas
            .Where(a => a.ProfessionalId == professionalId && !a.IsDeleted)
            .OrderBy(a => a.CityId)
            .ThenBy(a => a.ZoneId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProfessionalPerformance?> GetCurrentPerformanceAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalPerformances
            .Where(p => p.ProfessionalId == professionalId && !p.IsDeleted)
            .OrderByDescending(p => p.PeriodEnd)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Professional>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Where(p => p.OnboardingStatus == status && !p.IsDeleted)
            .OrderBy(p => p.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsAvailableAtAsync(string professionalId, DateTime slotStart, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        var slotEnd = slotStart + duration;
        var conflicts = await _context.HomeServiceBookings
            .AnyAsync(b => b.AssignedProfessionalId == professionalId && !b.IsDeleted
                && b.ScheduledStart < slotEnd && b.ExpectedEnd > slotStart
                && b.Status != "cancelled" && b.Status != "closed", cancellationToken);
        if (conflicts)
            return false;

        var dayOfWeek = (int)slotStart.DayOfWeek;
        return await _context.ProfessionalAvailabilities
            .AnyAsync(a => a.ProfessionalId == professionalId && a.DayOfWeek == dayOfWeek
                && a.StartTime <= slotStart.TimeOfDay && a.EndTime >= slotEnd.TimeOfDay && !a.IsDeleted, cancellationToken);
    }
}