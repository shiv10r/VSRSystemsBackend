using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Interfaces;

public interface IServiceCatalogRepository : IRepository<Service>
{
    Task<IReadOnlyList<ServiceCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    Task<ServiceCategory?> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Service>> GetActiveServicesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Service>> GetServicesByCategoryAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<Service?> GetServiceBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServicePackage>> GetPackagesByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceAddOn>> GetAddOnsByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServiceProblem>> GetProblemsByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ServicePackage>> GetActivePackagesByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Service>> GetEmergencyServicesAsync(CancellationToken cancellationToken = default);
}

public interface ILocationRepository : IRepository<City>
{
    Task<IReadOnlyList<City>> GetActiveCitiesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Zone>> GetZonesByCityAsync(string cityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Locality>> GetLocalitiesByZoneAsync(string zoneId, CancellationToken cancellationToken = default);
    Task<Pincode?> GetPincodeAsync(string code, CancellationToken cancellationToken = default);
    Task<ServiceArea?> GetServiceAreaAsync(string cityId, string zoneId, CancellationToken cancellationToken = default);
    Task<bool> IsServiceAreaActiveAsync(string serviceAreaId, CancellationToken cancellationToken = default);
    Task<bool> IsServiceInAreaAsync(string serviceAreaId, string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetServiceIdsInAreaAsync(string serviceAreaId, CancellationToken cancellationToken = default);
}

public interface IProfessionalRepository : IRepository<Professional>
{
    Task<IReadOnlyList<Professional>> GetVerifiedProfessionalsByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Professional>> GetEligibleProfessionalsAsync(string serviceId, string cityId, string zoneId, CancellationToken cancellationToken = default);
    Task<Professional?> GetWithDetailsAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProfessionalDocument>> GetDocumentsAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProfessionalAvailability>> GetAvailabilitiesAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProfessionalSkill>> GetSkillsAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProfessionalServiceArea>> GetServiceAreasAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<ProfessionalPerformance?> GetCurrentPerformanceAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Professional>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<bool> IsAvailableAtAsync(string professionalId, DateTime slotStart, TimeSpan duration, CancellationToken cancellationToken = default);
}

public interface IBookingRepository : IRepository<Booking>
{
    Task<Booking?> GetWithDetailsAsync(string id, CancellationToken cancellationToken = default);
    Task<Booking?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetUpcomingForProfessionalAsync(string professionalId, DateTime from, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetInDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> FindConflictsAsync(string professionalId, DateTime start, DateTime end, CancellationToken cancellationToken = default);
    Task AddStatusHistoryAsync(BookingStatusHistory history, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookingStatusHistory>> GetStatusHistoryAsync(string bookingId, CancellationToken cancellationToken = default);
    Task AddAssignmentAsync(BookingAssignment assignment, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookingAssignment>> GetAssignmentsAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> GetCustomerCompletedAsync(string customerId, CancellationToken cancellationToken = default);
}

public interface IRecurringBookingRepository : IRepository<RecurringBooking>
{
    Task<IReadOnlyList<RecurringBooking>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RecurringBooking>> GetDueAsync(DateTime upTo, CancellationToken cancellationToken = default);
}

public interface IAmcContractRepository : IRepository<AmcContract>
{
    Task<IReadOnlyList<AmcContract>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AmcContract>> GetActiveAsync(CancellationToken cancellationToken = default);
}

public interface IPriceQuoteRepository : IRepository<PriceQuote>
{
    Task<PriceQuote?> GetByQuoteNumberAsync(string quoteNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PriceQuote>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<QuoteRevision>> GetRevisionsAsync(string priceQuoteId, CancellationToken cancellationToken = default);
    Task<PriceQuote?> GetActiveForBookingAsync(string bookingId, CancellationToken cancellationToken = default);
}

public interface IPriceRuleRepository : IRepository<PriceRule>
{
    Task<IReadOnlyList<PriceRule>> GetActiveForServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PriceRule>> GetActiveForServiceAndCityAsync(string serviceId, string? cityId, CancellationToken cancellationToken = default);
}

public interface ICouponRepository : IRepository<Coupon>
{
    Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<int> GetRedemptionCountAsync(string couponId, CancellationToken cancellationToken = default);
    Task<int> GetCustomerRedemptionCountAsync(string couponId, string customerId, CancellationToken cancellationToken = default);
    Task AddRedemptionAsync(CouponRedemption redemption, CancellationToken cancellationToken = default);
}

public interface IMembershipRepository : IRepository<MembershipPlan>
{
    Task<MembershipPlan?> GetActiveByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<CustomerMembership?> GetActiveCustomerMembershipAsync(string customerId, CancellationToken cancellationToken = default);
}

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment?> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Payment?> GetByGatewayOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IRefundRepository : IRepository<Refund>
{
    Task<IReadOnlyList<Refund>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Refund>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface ICreditTransactionRepository : IRepository<CreditTransaction>
{
    Task<IReadOnlyList<CreditTransaction>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<decimal> GetWalletBalanceAsync(string customerId, CancellationToken cancellationToken = default);
}

public interface IEarningsRepository : IRepository<ProfessionalEarning>
{
    Task<IReadOnlyList<ProfessionalEarning>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<ProfessionalEarning?> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProfessionalEarning>> GetSettledInRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}

public interface IPayoutRepository : IRepository<Payout>
{
    Task<IReadOnlyList<Payout>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payout>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Payout?> GetPendingForPeriodAsync(string professionalId, DateTime periodStart, DateTime periodEnd, CancellationToken cancellationToken = default);
}

public interface IReviewRepository : IRepository<Review>
{
    Task<IReadOnlyList<Review>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<Review?> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<bool> ExistsForBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetRecentAsync(int limit, CancellationToken cancellationToken = default);
}

public interface ISupportRepository : IRepository<SupportTicket>
{
    Task<SupportTicket?> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupportTicket>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupportTicket>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupportTicket>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
}

public interface IDisputeRepository : IRepository<Dispute>
{
    Task<IReadOnlyList<Dispute>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Dispute>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface INotificationRepository : IRepository<Notification>
{
    Task<IReadOnlyList<Notification>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default);
}

public interface ICommissionRuleRepository : IRepository<CommissionRule>
{
    Task<CommissionRule?> GetApplicableRuleAsync(string? serviceId, string? categoryId, string? cityId, string? professionalTier, CancellationToken cancellationToken = default);
}

public interface IProfessionalEarningRepository : IEarningsRepository { }

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Role>> GetRolesAsync(string userId, CancellationToken cancellationToken = default);
}

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Customer?> GetWithAddressesAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CustomerAddress>> GetAddressesAsync(string customerId, CancellationToken cancellationToken = default);
    Task<CustomerAddress?> GetAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default);
}

public interface IAnalyticsRepository
{
    Task<int> CountBookingsAsync(CancellationToken cancellationToken = default);
    Task<int> CountBookingsAsync(string status, CancellationToken cancellationToken = default);
    Task<decimal> SumRevenueAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(DateTime Date, int Count, decimal Revenue)>> GetDailyBookingStatsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(string Name, int Count, decimal Revenue)>> GetTopByCategoryAsync(int limit, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(string Name, int Count, decimal Revenue)>> GetTopByServiceAsync(int limit, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(string Name, int Count)>> GetTopByCityAsync(int limit, CancellationToken cancellationToken = default);
    Task<(int Total, int Accepted, int Declined, int Expired)> GetAssignmentStatsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(string Reason, int Count)>> GetCancellationReasonsAsync(CancellationToken cancellationToken = default);
    Task<(int OneTime, int Repeat, int Total)> GetCustomerRepeatStatsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<(string ProfessionalId, string Name, int Completed, double Rating, double OnTime, decimal Earnings)>> GetProviderPerformanceAsync(int limit, CancellationToken cancellationToken = default);
    Task<(int PaidBookings, int Refunded, int Disputed, decimal RefundedAmount)> GetRefundDisputeStatsAsync(CancellationToken cancellationToken = default);
    Task<int> CountCustomersAsync(CancellationToken cancellationToken = default);
    Task<int> CountProfessionalsAsync(string? status, CancellationToken cancellationToken = default);
    Task<int> CountAssignmentsAsync(CancellationToken cancellationToken = default);
}