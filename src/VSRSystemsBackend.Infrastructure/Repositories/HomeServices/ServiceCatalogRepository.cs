using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Repositories.HomeServices;

public class ServiceCatalogRepository : IServiceCatalogRepository
{
    private readonly AppDbContext _context;

    public ServiceCatalogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Service?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Services.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> FindAsync(Expression<Func<Service, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(s => !s.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Service> AddAsync(Service entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Services.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Service> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Services.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Service entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Services.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Service entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Services.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Service> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Services.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Service, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Services.Where(s => !s.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Service, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(s => !s.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Service> Query()
    {
        return _context.Services.Where(s => !s.IsDeleted);
    }

    public async Task<IReadOnlyList<ServiceCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCategories
            .Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceCategory?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCategories
            .FirstOrDefaultAsync(c => c.Slug == slug && !c.IsDeleted, cancellationToken);
    }

    public async Task<ServiceCategory> AddCategoryAsync(ServiceCategory category, CancellationToken cancellationToken = default)
    {
        category.CreatedAt = DateTime.UtcNow;
        await _context.ServiceCategories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<IReadOnlyList<Service>> GetActiveServicesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(s => s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetServicesByCategoryAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(s => s.CategoryId == categoryId && !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Service?> GetServiceBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .FirstOrDefaultAsync(s => s.Slug == slug && !s.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<ServicePackage>> GetPackagesByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.ServicePackages
            .Where(p => p.ServiceId == serviceId && !p.IsDeleted)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceAddOn>> GetAddOnsByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceAddOns
            .Where(a => a.ServiceId == serviceId && !a.IsDeleted)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ServiceProblem>> GetProblemsByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceProblems
            .Where(p => p.ServiceId == serviceId && !p.IsDeleted)
            .OrderBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ServicePackage>> GetActivePackagesByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.ServicePackages
            .Where(p => p.ServiceId == serviceId && p.IsActive && !p.IsDeleted)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetEmergencyServicesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(s => s.IsEmergency && s.IsActive && !s.IsDeleted)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }
}

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<City?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Cities.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cities
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<City>> FindAsync(Expression<Func<City, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Cities
            .Where(c => !c.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<City> AddAsync(City entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Cities.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<City> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Cities.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(City entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Cities.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(City entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Cities.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<City> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Cities.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<City, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Cities.Where(c => !c.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<City, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Cities
            .Where(c => !c.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<City> Query()
    {
        return _context.Cities.Where(c => !c.IsDeleted);
    }

    public async Task<IReadOnlyList<City>> GetActiveCitiesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cities
            .Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Zone>> GetZonesByCityAsync(string cityId, CancellationToken cancellationToken = default)
    {
        return await _context.Zones
            .Where(z => z.CityId == cityId && !z.IsDeleted)
            .OrderBy(z => z.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Locality>> GetLocalitiesByZoneAsync(string zoneId, CancellationToken cancellationToken = default)
    {
        return await _context.Localities
            .Where(l => l.ZoneId == zoneId && !l.IsDeleted)
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Pincode?> GetPincodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Pincodes
            .FirstOrDefaultAsync(p => p.Code == code && !p.IsDeleted, cancellationToken);
    }

    public async Task<ServiceArea?> GetServiceAreaAsync(string cityId, string zoneId, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceAreas
            .FirstOrDefaultAsync(a => a.CityId == cityId && a.ZoneId == zoneId && !a.IsDeleted, cancellationToken);
    }

    public async Task<bool> IsServiceAreaActiveAsync(string serviceAreaId, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceAreas
            .AnyAsync(a => a.Id == serviceAreaId && a.IsActive && !a.IsDeleted, cancellationToken);
    }

    public async Task<bool> IsServiceInAreaAsync(string serviceAreaId, string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceAreaServices
            .AnyAsync(sa => sa.ServiceAreaId == serviceAreaId && sa.ServiceId == serviceId && sa.IsActive && !sa.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetServiceIdsInAreaAsync(string serviceAreaId, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceAreaServices
            .Where(sa => sa.ServiceAreaId == serviceAreaId && sa.IsActive && !sa.IsDeleted)
            .OrderBy(sa => sa.ServiceId)
            .Select(sa => sa.ServiceId)
            .ToListAsync(cancellationToken);
    }
}

public class PriceRuleRepository : IPriceRuleRepository
{
    private readonly AppDbContext _context;

    public PriceRuleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PriceRule?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.PriceRules.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<PriceRule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PriceRules
            .Where(r => !r.IsDeleted)
            .OrderBy(r => r.ServiceId)
            .ThenBy(r => r.RuleType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PriceRule>> FindAsync(Expression<Func<PriceRule, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PriceRules
            .Where(r => !r.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<PriceRule> AddAsync(PriceRule entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.PriceRules.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<PriceRule> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.PriceRules.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PriceRule entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PriceRules.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(PriceRule entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PriceRules.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<PriceRule> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.PriceRules.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<PriceRule, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.PriceRules.Where(r => !r.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<PriceRule, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.PriceRules
            .Where(r => !r.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<PriceRule> Query()
    {
        return _context.PriceRules.Where(r => !r.IsDeleted);
    }

    public async Task<IReadOnlyList<PriceRule>> GetActiveForServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        return await _context.PriceRules
            .Where(r => r.ServiceId == serviceId && r.IsActive && !r.IsDeleted)
            .OrderBy(r => r.RuleType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PriceRule>> GetActiveForServiceAndCityAsync(string serviceId, string? cityId, CancellationToken cancellationToken = default)
    {
        var query = _context.PriceRules
            .Where(r => r.ServiceId == serviceId && r.IsActive && !r.IsDeleted);
        if (cityId != null)
            query = query.Where(r => r.CityId == null || r.CityId == cityId);
        else
            query = query.Where(r => r.CityId == null);
        return await query
            .OrderBy(r => r.RuleType)
            .ToListAsync(cancellationToken);
    }
}

public class CouponRepository : ICouponRepository
{
    private readonly AppDbContext _context;

    public CouponRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Coupon?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.Coupons.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Coupon>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Coupons
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Coupon>> FindAsync(Expression<Func<Coupon, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Coupons
            .Where(c => !c.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Coupon> AddAsync(Coupon entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.Coupons.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Coupon> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.Coupons.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Coupon entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Coupons.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Coupon entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Coupons.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Coupon> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.Coupons.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Coupon, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Coupons.Where(c => !c.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Coupon, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Coupons
            .Where(c => !c.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Coupon> Query()
    {
        return _context.Coupons.Where(c => !c.IsDeleted);
    }

    public async Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Coupons
            .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted, cancellationToken);
    }

    public async Task<int> GetRedemptionCountAsync(string couponId, CancellationToken cancellationToken = default)
    {
        return await _context.CouponRedemptions
            .CountAsync(r => r.CouponId == couponId && !r.IsDeleted, cancellationToken);
    }

    public async Task<int> GetCustomerRedemptionCountAsync(string couponId, string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.CouponRedemptions
            .CountAsync(r => r.CouponId == couponId && r.CustomerId == customerId && !r.IsDeleted, cancellationToken);
    }

    public async Task AddRedemptionAsync(CouponRedemption redemption, CancellationToken cancellationToken = default)
    {
        redemption.CreatedAt = DateTime.UtcNow;
        await _context.CouponRedemptions.AddAsync(redemption, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class MembershipRepository : IMembershipRepository
{
    private readonly AppDbContext _context;

    public MembershipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MembershipPlan?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPlans.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<MembershipPlan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPlans
            .Where(m => !m.IsDeleted)
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MembershipPlan>> FindAsync(Expression<Func<MembershipPlan, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPlans
            .Where(m => !m.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<MembershipPlan> AddAsync(MembershipPlan entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.MembershipPlans.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<MembershipPlan> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.MembershipPlans.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(MembershipPlan entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.MembershipPlans.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(MembershipPlan entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.MembershipPlans.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<MembershipPlan> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.MembershipPlans.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<MembershipPlan, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.MembershipPlans.Where(m => !m.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<MembershipPlan, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPlans
            .Where(m => !m.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<MembershipPlan> Query()
    {
        return _context.MembershipPlans.Where(m => !m.IsDeleted);
    }

    public async Task<MembershipPlan?> GetActiveByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.MembershipPlans
            .FirstOrDefaultAsync(m => m.Id == id && m.IsActive && !m.IsDeleted, cancellationToken);
    }

    public async Task<CustomerMembership?> GetActiveCustomerMembershipAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.CustomerMemberships
            .Where(m => m.CustomerId == customerId && m.Status == "active" && m.ExpiresAt >= now && !m.IsDeleted)
            .OrderByDescending(m => m.ExpiresAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

public class CommissionRuleRepository : ICommissionRuleRepository
{
    private readonly AppDbContext _context;

    public CommissionRuleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CommissionRule?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.CommissionRules.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<CommissionRule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CommissionRules
            .Where(r => !r.IsDeleted)
            .OrderBy(r => r.ProfessionalTier)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CommissionRule>> FindAsync(Expression<Func<CommissionRule, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.CommissionRules
            .Where(r => !r.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<CommissionRule> AddAsync(CommissionRule entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.CommissionRules.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<CommissionRule> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.CommissionRules.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CommissionRule entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.CommissionRules.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(CommissionRule entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.CommissionRules.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<CommissionRule> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.CommissionRules.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<CommissionRule, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.CommissionRules.Where(r => !r.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<CommissionRule, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.CommissionRules
            .Where(r => !r.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<CommissionRule> Query()
    {
        return _context.CommissionRules.Where(r => !r.IsDeleted);
    }

    public async Task<CommissionRule?> GetApplicableRuleAsync(string? serviceId, string? categoryId, string? cityId, string? professionalTier, CancellationToken cancellationToken = default)
    {
        return await _context.CommissionRules
            .Where(r => r.IsActive && !r.IsDeleted)
            .Where(r => serviceId == null || r.ServiceId == null || r.ServiceId == serviceId)
            .Where(r => categoryId == null || r.CategoryId == null || r.CategoryId == categoryId)
            .Where(r => cityId == null || r.CityId == null || r.CityId == cityId)
            .Where(r => professionalTier == null || r.ProfessionalTier == null || r.ProfessionalTier == professionalTier)
            .OrderByDescending(r => r.ServiceId != null)
            .ThenByDescending(r => r.CategoryId != null)
            .ThenByDescending(r => r.CityId != null)
            .ThenByDescending(r => r.ProfessionalTier != null)
            .ThenByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceUsers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceUsers
            .Where(u => !u.IsDeleted)
            .OrderBy(u => u.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<User>> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceUsers
            .Where(u => !u.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.HomeServiceUsers.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.HomeServiceUsers.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceUsers.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceUsers.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.HomeServiceUsers.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<User, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.HomeServiceUsers.Where(u => !u.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceUsers
            .Where(u => !u.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<User> Query()
    {
        return _context.HomeServiceUsers.Where(u => !u.IsDeleted);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceUsers
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Role>> GetRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceRoles
            .Where(r => !r.IsDeleted && r.UserRoles.Any(ur => ur.UserId == userId && !ur.IsDeleted))
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }
}

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomers
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> FindAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomers
            .Where(c => !c.IsDeleted)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer> AddAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.HomeServiceCustomers.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<Customer> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }
        await _context.HomeServiceCustomers.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceCustomers.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.HomeServiceCustomers.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Customer> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
        }
        _context.HomeServiceCustomers.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Customer, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.HomeServiceCustomers.Where(c => !c.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Customer, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomers
            .Where(c => !c.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<Customer> Query()
    {
        return _context.HomeServiceCustomers.Where(c => !c.IsDeleted);
    }

    public async Task<Customer?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomers
            .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted, cancellationToken);
    }

    public async Task<Customer?> GetWithAddressesAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomers
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<CustomerAddress>> GetAddressesAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomerAddresses
            .Where(a => a.CustomerId == customerId && !a.IsDeleted)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.Label)
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerAddress?> GetAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomerAddresses
            .FirstOrDefaultAsync(a => a.CustomerId == customerId && a.Id == addressId && !a.IsDeleted, cancellationToken);
    }
}

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly AppDbContext _context;

    public AnalyticsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CountBookingsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .CountAsync(b => !b.IsDeleted, cancellationToken);
    }

    public async Task<int> CountBookingsAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceBookings
            .CountAsync(b => b.Status == status && !b.IsDeleted, cancellationToken);
    }

    public async Task<decimal> SumRevenueAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BookingItems
            .Where(i => !i.IsDeleted)
            .SumAsync(i => (decimal?)i.LineTotal, cancellationToken) ?? 0;
    }

    public async Task<IReadOnlyList<(DateTime Date, int Count, decimal Revenue)>> GetDailyBookingStatsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var bookings = await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted && b.ScheduledStart >= from && b.ScheduledStart < to)
            .GroupBy(b => b.ScheduledStart.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var revenue = await _context.BookingItems
            .Where(i => !i.IsDeleted && i.Booking.ScheduledStart >= from && i.Booking.ScheduledStart < to)
            .GroupBy(i => i.Booking.ScheduledStart.Date)
            .Select(g => new { Date = g.Key, Amount = g.Sum(i => i.LineTotal) })
            .ToListAsync(cancellationToken);

        var result = new List<(DateTime Date, int Count, decimal Revenue)>();
        foreach (var b in bookings)
        {
            result.Add((b.Date, b.Count, revenue.Where(r => r.Date == b.Date).Sum(r => r.Amount)));
        }
        return result;
    }

    public async Task<IReadOnlyList<(string Name, int Count, decimal Revenue)>> GetTopByCategoryAsync(int limit, CancellationToken cancellationToken = default)
    {
        var byCategory = await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted)
            .GroupBy(b => b.Service.Category.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var revenue = await _context.BookingItems
            .Where(i => !i.IsDeleted)
            .GroupBy(i => i.Booking.Service.Category.Name)
            .Select(g => new { Name = g.Key, Amount = g.Sum(i => i.LineTotal) })
            .ToListAsync(cancellationToken);

        var result = new List<(string Name, int Count, decimal Revenue)>();
        foreach (var c in byCategory)
        {
            result.Add((c.Name, c.Count, revenue.Where(r => r.Name == c.Name).Sum(r => r.Amount)));
        }
        return result;
    }

    public async Task<IReadOnlyList<(string Name, int Count, decimal Revenue)>> GetTopByServiceAsync(int limit, CancellationToken cancellationToken = default)
    {
        var byService = await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted)
            .GroupBy(b => b.Service.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var revenue = await _context.BookingItems
            .Where(i => !i.IsDeleted)
            .GroupBy(i => i.Booking.Service.Name)
            .Select(g => new { Name = g.Key, Amount = g.Sum(i => i.LineTotal) })
            .ToListAsync(cancellationToken);

        var result = new List<(string Name, int Count, decimal Revenue)>();
        foreach (var s in byService)
        {
            result.Add((s.Name, s.Count, revenue.Where(r => r.Name == s.Name).Sum(r => r.Amount)));
        }
        return result;
    }

    public async Task<IReadOnlyList<(string Name, int Count)>> GetTopByCityAsync(int limit, CancellationToken cancellationToken = default)
    {
        var items = await (from b in _context.HomeServiceBookings
                           join a in _context.HomeServiceCustomerAddresses on b.AddressId equals a.Id
                           join c in _context.Cities on a.CityId equals c.Id
                           where !b.IsDeleted && !a.IsDeleted && !c.IsDeleted
                           group b by c.Name into g
                           select new { Name = g.Key, Count = g.Count() })
                           .OrderByDescending(x => x.Count)
                           .Take(limit)
                           .ToListAsync(cancellationToken);

        return items.Select(x => (x.Name, x.Count)).ToList();
    }

    public async Task<(int Total, int Accepted, int Declined, int Expired)> GetAssignmentStatsAsync(CancellationToken cancellationToken = default)
    {
        var total = await _context.BookingAssignments.CountAsync(a => !a.IsDeleted, cancellationToken);
        var accepted = await _context.BookingAssignments.CountAsync(a => a.Response == "accepted" && !a.IsDeleted, cancellationToken);
        var declined = await _context.BookingAssignments.CountAsync(a => a.Response == "declined" && !a.IsDeleted, cancellationToken);
        var expired = await _context.BookingAssignments.CountAsync(a => a.Response == "expired" && !a.IsDeleted, cancellationToken);
        return (total, accepted, declined, expired);
    }

    public async Task<IReadOnlyList<(string Reason, int Count)>> GetCancellationReasonsAsync(CancellationToken cancellationToken = default)
    {
        var items = await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted && b.Status == "cancelled" && b.CancelReason != null)
            .GroupBy(b => b.CancelReason!)
            .Select(g => new { Reason = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync(cancellationToken);

        return items.Select(x => (x.Reason, x.Count)).ToList();
    }

    public async Task<(int OneTime, int Repeat, int Total)> GetCustomerRepeatStatsAsync(CancellationToken cancellationToken = default)
    {
        var grouped = await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted)
            .GroupBy(b => b.CustomerId)
            .Select(g => new { CustomerId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var oneTime = grouped.Count(g => g.Count == 1);
        var repeat = grouped.Count(g => g.Count > 1);
        return (oneTime, repeat, grouped.Count);
    }

    public async Task<IReadOnlyList<(string ProfessionalId, string Name, int Completed, double Rating, double OnTime, decimal Earnings)>> GetProviderPerformanceAsync(int limit, CancellationToken cancellationToken = default)
    {
        var professionals = await _context.Professionals
            .Where(p => !p.IsDeleted)
            .Select(p => new { p.Id, p.DisplayName })
            .ToListAsync(cancellationToken);

        var completed = await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted && b.Status == "completed" && b.AssignedProfessionalId != null)
            .GroupBy(b => b.AssignedProfessionalId!)
            .Select(g => new { ProfessionalId = g.Key, Completed = g.Count() })
            .ToListAsync(cancellationToken);

        var ratings = await _context.HomeServiceReviews
            .Where(r => !r.IsDeleted)
            .GroupBy(r => r.ProfessionalId)
            .Select(g => new { ProfessionalId = g.Key, Rating = g.Average(r => (double)r.Rating) })
            .ToListAsync(cancellationToken);

        var onTime = await _context.HomeServiceBookings
            .Where(b => !b.IsDeleted && b.AssignedProfessionalId != null && b.ActualStartAt != null)
            .Select(b => new { ProfessionalId = b.AssignedProfessionalId!, WasOnTime = b.ActualStartAt!.Value <= b.ScheduledStart })
            .GroupBy(x => x.ProfessionalId)
            .Select(g => new { ProfessionalId = g.Key, OnTimeRate = g.Average(x => x.WasOnTime ? 1.0 : 0.0) })
            .ToListAsync(cancellationToken);

        var earnings = await _context.ProfessionalEarnings
            .Where(e => !e.IsDeleted)
            .GroupBy(e => e.ProfessionalId)
            .Select(g => new { ProfessionalId = g.Key, Earnings = g.Sum(e => e.NetAmount) })
            .ToListAsync(cancellationToken);

        var completedDict = completed.ToDictionary(c => c.ProfessionalId, c => c.Completed);
        var ratingDict = ratings.ToDictionary(r => r.ProfessionalId, r => r.Rating);
        var onTimeDict = onTime.ToDictionary(o => o.ProfessionalId, o => o.OnTimeRate);
        var earningsDict = earnings.ToDictionary(e => e.ProfessionalId, e => e.Earnings);

        var result = new List<(string ProfessionalId, string Name, int Completed, double Rating, double OnTime, decimal Earnings)>();
        foreach (var p in professionals.OrderByDescending(p => completedDict.GetValueOrDefault(p.Id)).Take(limit))
        {
            result.Add((p.Id, p.DisplayName, completedDict.GetValueOrDefault(p.Id), ratingDict.GetValueOrDefault(p.Id), onTimeDict.GetValueOrDefault(p.Id), earningsDict.GetValueOrDefault(p.Id)));
        }
        return result;
    }

    public async Task<(int PaidBookings, int Refunded, int Disputed, decimal RefundedAmount)> GetRefundDisputeStatsAsync(CancellationToken cancellationToken = default)
    {
        var paid = await _context.HomeServiceBookings.CountAsync(b => b.PaymentStatus == "paid" && !b.IsDeleted, cancellationToken);
        var refunded = await _context.Refunds.CountAsync(r => r.Status == "processed" && !r.IsDeleted, cancellationToken);
        var disputed = await _context.Disputes.CountAsync(d => (d.Status == "open" || d.Status == "investigating") && !d.IsDeleted, cancellationToken);
        var refundedAmount = await _context.Refunds
            .Where(r => r.Status == "processed" && !r.IsDeleted)
            .SumAsync(r => (decimal?)r.Amount, cancellationToken) ?? 0;
        return (paid, refunded, disputed, refundedAmount);
    }

    public async Task<int> CountCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.HomeServiceCustomers
            .CountAsync(c => !c.IsDeleted, cancellationToken);
    }

    public async Task<int> CountProfessionalsAsync(string? status, CancellationToken cancellationToken = default)
    {
        var query = _context.Professionals.Where(p => !p.IsDeleted);
        if (status != null)
            query = query.Where(p => p.OnboardingStatus == status);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountAssignmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BookingAssignments
            .CountAsync(a => !a.IsDeleted, cancellationToken);
    }
}