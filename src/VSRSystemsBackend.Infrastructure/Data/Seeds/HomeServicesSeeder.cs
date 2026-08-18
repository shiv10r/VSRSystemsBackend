using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.Services;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Data.Seeds;

/// <summary>
/// Persists the Home Services seed bundle (§152/§153) into the database.
/// Idempotent: skips when the module is already seeded (ServiceCategory table non-empty).
/// Inserts in dependency order so FK constraints are satisfied.
/// </summary>
public static class HomeServicesSeeder
{
    public static async Task SeedAsync(AppDbContext context, CancellationToken ct = default)
    {
        if (await context.ServiceCategories.AnyAsync(ct))
        {
            return;
        }

        var data = HomeServicesSeedData.Build();

        await using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {

        // 1. Identity base (roles, permissions, role-permissions, membership plans)
        context.HomeServiceRoles.AddRange(data.Roles);
        context.HomeServicePermissions.AddRange(data.Permissions);
        context.HomeServiceRolePermissions.AddRange(data.RolePermissions);
        context.MembershipPlans.AddRange(data.MembershipPlans);
        context.PaymentGatewaySettings.AddRange(data.PaymentGatewaySettings);
        await context.SaveChangesAsync(ct);

        // 2. Locations (cities, zones, localities, pincodes, service areas)
        context.Cities.AddRange(data.Cities);
        context.Zones.AddRange(data.Zones);
        context.Localities.AddRange(data.Localities);
        context.Pincodes.AddRange(data.Pincodes);
        context.ServiceAreas.AddRange(data.ServiceAreas);
        await context.SaveChangesAsync(ct);

        // 3. Catalog (categories, services, problems, packages, add-ons, package-add-ons, warranties)
        context.ServiceCategories.AddRange(data.Categories);
        context.Services.AddRange(data.Services);
        context.ServiceProblems.AddRange(data.Problems);
        context.ServicePackages.AddRange(data.Packages);
        context.ServiceAddOns.AddRange(data.AddOns);
        context.ServicePackageAddOns.AddRange(data.PackageAddOns);
        context.ServiceWarranties.AddRange(data.Warranties);
        context.ServiceAreaServices.AddRange(data.ServiceAreaServices);
        await context.SaveChangesAsync(ct);

        // 4. Users + roles + customers + addresses + memberships
        context.HomeServiceUsers.AddRange(data.Users);
        context.HomeServiceUserRoles.AddRange(data.UserRoles);
        context.HomeServiceCustomers.AddRange(data.Customers);
        context.HomeServiceCustomerAddresses.AddRange(data.CustomerAddresses);
        context.CustomerMemberships.AddRange(data.CustomerMemberships);
        context.CreditTransactions.AddRange(data.CreditTransactions);
        await context.SaveChangesAsync(ct);

        // 5. Professionals + documents/skills/service-areas/availabilities/performances
        context.Professionals.AddRange(data.Professionals);
        context.ProfessionalDocuments.AddRange(data.ProfessionalDocuments);
        context.ProfessionalSkills.AddRange(data.ProfessionalSkills);
        context.ProfessionalServiceAreas.AddRange(data.ProfessionalServiceAreas);
        context.ProfessionalAvailabilities.AddRange(data.ProfessionalAvailabilities);
        context.ProfessionalPerformances.AddRange(data.ProfessionalPerformances);
        await context.SaveChangesAsync(ct);

        // 6. Pricing + commerce base
        context.PriceRules.AddRange(data.PriceRules);
        context.CommissionRules.AddRange(data.CommissionRules);
        context.Coupons.AddRange(data.Coupons);
        await context.SaveChangesAsync(ct);

        // 7. Bookings + their children
        context.HomeServiceBookings.AddRange(data.Bookings);
        context.BookingItems.AddRange(data.BookingItems);
        context.BookingAddOns.AddRange(data.BookingAddOns);
        context.BookingMaterials.AddRange(data.BookingMaterials);
        context.BookingAssignments.AddRange(data.BookingAssignments);
        context.BookingStatusHistories.AddRange(data.BookingStatusHistories);
        context.BookingNotes.AddRange(data.BookingNotes);
        await context.SaveChangesAsync(ct);

        // 8. Finance
        context.PriceQuotes.AddRange(data.PriceQuotes);
        context.QuoteRevisions.AddRange(data.QuoteRevisions);
        context.Payments.AddRange(data.Payments);
        context.Refunds.AddRange(data.Refunds);
        context.ProfessionalEarnings.AddRange(data.ProfessionalEarnings);
        context.Payouts.AddRange(data.Payouts);
        context.ProfessionalAdjustments.AddRange(data.ProfessionalAdjustments);
        context.ProfessionalIncentives.AddRange(data.ProfessionalIncentives);
        await context.SaveChangesAsync(ct);

        // 9. Commerce + social + support
        context.CouponRedemptions.AddRange(data.CouponRedemptions);
        context.HomeServiceReviews.AddRange(data.Reviews);
        context.ReviewMedia.AddRange(data.ReviewMediaItems);
        context.RecurringBookings.AddRange(data.RecurringBookings);
        context.AmcContracts.AddRange(data.AmcContracts);
        await context.SaveChangesAsync(ct);

        // 10. Support/ops content
        context.SupportTickets.AddRange(data.SupportTickets);
        context.Disputes.AddRange(data.Disputes);
        context.HomeServiceNotifications.AddRange(data.Notifications);
        context.CmsPages.AddRange(data.CmsPages);
        context.Banners.AddRange(data.Banners);
        context.Faqs.AddRange(data.Faqs);
        context.HomeServiceAuditLogs.AddRange(data.AuditLogs);
        await context.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}