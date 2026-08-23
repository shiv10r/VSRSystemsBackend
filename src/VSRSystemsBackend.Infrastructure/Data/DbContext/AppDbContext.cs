using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Warehouse;
using VSRSystemsBackend.Domain.Interior;
using VSRSystemsBackend.Domain.School;
using VSRSystemsBackend.Domain.Hotel;
using VSRSystemsBackend.Domain.Bank;
using VSRSystemsBackend.Domain.Commerce;
using VSRSystemsBackend.Domain.Jobs;
using VSRSystemsBackend.Domain.Medical;
using VSRSystemsBackend.Domain.News;
using VSRSystemsBackend.Domain.Travel;
using VSRSystemsBackend.Domain.HomeServices;
using VSRSystemsBackend.Domain.Platform;
using VSRSystemsBackend.Infrastructure.Data.Configurations;
// Aliases for HomeServices entities whose names collide with other domains
using HsBooking = VSRSystemsBackend.Domain.HomeServices.Booking;
using HsReview = VSRSystemsBackend.Domain.HomeServices.Review;
using HsCustomer = VSRSystemsBackend.Domain.HomeServices.Customer;
using HsNotification = VSRSystemsBackend.Domain.HomeServices.Notification;
using HsAuditLog = VSRSystemsBackend.Domain.HomeServices.AuditLog;
// Aliases for pre-existing domain entities now shadowed by the HomeServices import
using WarehouseCustomer = VSRSystemsBackend.Domain.Warehouse.Customer;
using SchoolNotification = VSRSystemsBackend.Domain.School.Notification;
using SchoolAuditLog = VSRSystemsBackend.Domain.School.AuditLog;
using CommerceReview = VSRSystemsBackend.Domain.Commerce.Review;
using TravelBooking = VSRSystemsBackend.Domain.Travel.Booking;

namespace VSRSystemsBackend.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Warehouse Domain
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<LocationBin> LocationBins => Set<LocationBin>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<WarehouseCustomer> Customers => Set<WarehouseCustomer>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<GrnRecord> GrnRecords => Set<GrnRecord>();
    public DbSet<GrnLine> GrnLines => Set<GrnLine>();
    public DbSet<PutawayBin> PutawayBins => Set<PutawayBin>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderLine> SalesOrderLines => Set<SalesOrderLine>();
    public DbSet<StockTransfer> StockTransfers => Set<StockTransfer>();
    public DbSet<StockTransferLine> StockTransferLines => Set<StockTransferLine>();
    public DbSet<PickList> PickLists => Set<PickList>();
    public DbSet<PickLine> PickLines => Set<PickLine>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageItem> PackageItems => Set<PackageItem>();
    public DbSet<Dispatch> Dispatches => Set<Dispatch>();
    public DbSet<ReturnRecord> ReturnRecords => Set<ReturnRecord>();
    public DbSet<ReturnLine> ReturnLines => Set<ReturnLine>();
    public DbSet<StockCount> StockCounts => Set<StockCount>();
    public DbSet<StockCountLine> StockCountLines => Set<StockCountLine>();
    public DbSet<StaffMember> StaffMembers => Set<StaffMember>();
    public DbSet<ProjectRecord> Projects => Set<ProjectRecord>();
    public DbSet<ProjectAttendance> ProjectAttendances => Set<ProjectAttendance>();
    public DbSet<ProjectLog> ProjectLogs => Set<ProjectLog>();
    public DbSet<ProjectTransaction> ProjectTransactions => Set<ProjectTransaction>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
    public DbSet<ProjectParty> ProjectParties => Set<ProjectParty>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<StockAdjustment> StockAdjustments => Set<StockAdjustment>();

    // Interior Domain
    public DbSet<InteriorProject> InteriorProjects => Set<InteriorProject>();
    public DbSet<InteriorRoom> InteriorRooms => Set<InteriorRoom>();
    public DbSet<InteriorDesign> InteriorDesigns => Set<InteriorDesign>();
    public DbSet<DesignVersion> DesignVersions => Set<DesignVersion>();
    public DbSet<InteriorProduct> InteriorProducts => Set<InteriorProduct>();

    // School Domain
    public DbSet<Student> Students => Set<Student>();
    public DbSet<SchoolClass> SchoolClasses => Set<SchoolClass>();
    public DbSet<SchoolStaffMember> SchoolStaffMembers => Set<SchoolStaffMember>();
    public DbSet<ParentRecord> ParentRecords => Set<ParentRecord>();
    public DbSet<SchoolProject> SchoolProjects => Set<SchoolProject>();
    public DbSet<StockItem> StockItems => Set<StockItem>();
    public DbSet<AdmissionLead> AdmissionLeads => Set<AdmissionLead>();
    public DbSet<AcademicSession> AcademicSessions => Set<AcademicSession>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TimetableSlot> TimetableSlots => Set<TimetableSlot>();
    public DbSet<Homework> Homework => Set<Homework>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<OnlineExam> OnlineExams => Set<OnlineExam>();
    public DbSet<TestAttempt> TestAttempts => Set<TestAttempt>();
    public DbSet<ExamSchedule> ExamSchedules => Set<ExamSchedule>();
    public DbSet<MarksEntry> MarksEntries => Set<MarksEntry>();
    public DbSet<ResultRecord> ResultRecords => Set<ResultRecord>();
    public DbSet<FeeRecord> FeeRecords => Set<FeeRecord>();
    public DbSet<FeeStructure> FeeStructures => Set<FeeStructure>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<ExpenseRecord> ExpenseRecords => Set<ExpenseRecord>();
    public DbSet<PayrollRecord> PayrollRecords => Set<PayrollRecord>();
    public DbSet<JobOpening> JobOpenings => Set<JobOpening>();
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<PerformanceReview> PerformanceReviews => Set<PerformanceReview>();
    public DbSet<TrainingProgram> TrainingPrograms => Set<TrainingProgram>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<TransportRoute> TransportRoutes => Set<TransportRoute>();
    public DbSet<LibraryBook> LibraryBooks => Set<LibraryBook>();
    public DbSet<LibraryIssue> LibraryIssues => Set<LibraryIssue>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<SchoolPurchaseOrder> SchoolPurchaseOrders => Set<SchoolPurchaseOrder>();
    public DbSet<AssetRecord> AssetRecords => Set<AssetRecord>();
    public DbSet<VisitorLog> VisitorLogs => Set<VisitorLog>();
    public DbSet<HostelRoom> HostelRooms => Set<HostelRoom>();
    public DbSet<HostelAllocation> HostelAllocations => Set<HostelAllocation>();
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<SportsTeam> SportsTeams => Set<SportsTeam>();
    public DbSet<Fixture> Fixtures => Set<Fixture>();
    public DbSet<House> Houses => Set<House>();
    public DbSet<HousePoint> HousePoints => Set<HousePoint>();
    public DbSet<DisciplineRecord> DisciplineRecords => Set<DisciplineRecord>();
    public DbSet<CounsellingSession> CounsellingSessions => Set<CounsellingSession>();
    public DbSet<Notice> Notices => Set<Notice>();
    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<SchoolNotification> Notifications => Set<SchoolNotification>();
    public DbSet<PTMSession> PTMSessions => Set<PTMSession>();
    public DbSet<Survey> Surveys => Set<Survey>();
    public DbSet<DocumentRecord> DocumentRecords => Set<DocumentRecord>();
    public DbSet<CertificateTemplate> CertificateTemplates => Set<CertificateTemplate>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Grievance> Grievances => Set<Grievance>();
    public DbSet<IncidentRecord> IncidentRecords => Set<IncidentRecord>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<SchoolAuditLog> AuditLogs => Set<SchoolAuditLog>();
    public DbSet<SchoolSetting> SchoolSettings => Set<SchoolSetting>();

    // Hotel Domain
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<HousekeepingTask> HousekeepingTasks => Set<HousekeepingTask>();

    // Bank Domain
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Beneficiary> Beneficiaries => Set<Beneficiary>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Deposit> Deposits => Set<Deposit>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BankDocument> BankDocuments => Set<BankDocument>();
    public DbSet<BankNotification> BankNotifications => Set<BankNotification>();

    // Commerce Domain
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<CommerceReview> Reviews => Set<CommerceReview>();

    // Jobs Domain
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<SavedJob> SavedJobs => Set<SavedJob>();
    public DbSet<ScreeningQuestion> ScreeningQuestions => Set<ScreeningQuestion>();
    public DbSet<JobSource> JobSources => Set<JobSource>();
    public DbSet<JobSourceConfig> JobSourceConfigs => Set<JobSourceConfig>();
    public DbSet<RawExternalJob> RawExternalJobs => Set<RawExternalJob>();
    public DbSet<ScrapeRun> ScrapeRuns => Set<ScrapeRun>();
    public DbSet<ScrapeLog> ScrapeLogs => Set<ScrapeLog>();
    public DbSet<JobSourceMapping> JobSourceMappings => Set<JobSourceMapping>();
    public DbSet<DuplicateCandidate> DuplicateCandidates => Set<DuplicateCandidate>();
    public DbSet<IngestionError> IngestionErrors => Set<IngestionError>();

    // Medical Domain
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PharmacyItem> PharmacyItems => Set<PharmacyItem>();
    public DbSet<LabTest> LabTests => Set<LabTest>();
    public DbSet<MedicalBilling> MedicalBillings => Set<MedicalBilling>();

    // News Domain
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<NewsCategory> NewsCategories => Set<NewsCategory>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();

    // Travel Domain
    public DbSet<TravelPackage> TravelPackages => Set<TravelPackage>();
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<TravelBooking> Bookings => Set<TravelBooking>();
    public DbSet<GroupTrip> GroupTrips => Set<GroupTrip>();
    public DbSet<TravelWishlist> TravelWishlists => Set<TravelWishlist>();
    public DbSet<TravelWishlistItem> TravelWishlistItems => Set<TravelWishlistItem>();
    public DbSet<TravelDeparture> TravelDepartures => Set<TravelDeparture>();
    public DbSet<TravelBookingSession> TravelBookingSessions => Set<TravelBookingSession>();
    public DbSet<TravelPayment> TravelPayments => Set<TravelPayment>();
    public DbSet<Lead> Leads => Set<Lead>();

    // HomeServices Domain
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServiceProblem> ServiceProblems => Set<ServiceProblem>();
    public DbSet<ServicePackage> ServicePackages => Set<ServicePackage>();
    public DbSet<ServiceAddOn> ServiceAddOns => Set<ServiceAddOn>();
    public DbSet<ServicePackageAddOn> ServicePackageAddOns => Set<ServicePackageAddOn>();
    public DbSet<ServiceWarranty> ServiceWarranties => Set<ServiceWarranty>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Locality> Localities => Set<Locality>();
    public DbSet<Pincode> Pincodes => Set<Pincode>();
    public DbSet<ServiceArea> ServiceAreas => Set<ServiceArea>();
    public DbSet<ServiceAreaService> ServiceAreaServices => Set<ServiceAreaService>();
    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<ProfessionalDocument> ProfessionalDocuments => Set<ProfessionalDocument>();
    public DbSet<ProfessionalSkill> ProfessionalSkills => Set<ProfessionalSkill>();
    public DbSet<ProfessionalServiceArea> ProfessionalServiceAreas => Set<ProfessionalServiceArea>();
    public DbSet<ProfessionalAvailability> ProfessionalAvailabilities => Set<ProfessionalAvailability>();
    public DbSet<ProfessionalTimeOff> ProfessionalTimeOffs => Set<ProfessionalTimeOff>();
    public DbSet<ProfessionalPerformance> ProfessionalPerformances => Set<ProfessionalPerformance>();
    public DbSet<HsBooking> HomeServiceBookings => Set<HsBooking>();
    public DbSet<BookingItem> BookingItems => Set<BookingItem>();
    public DbSet<BookingAddOn> BookingAddOns => Set<BookingAddOn>();
    public DbSet<BookingMaterial> BookingMaterials => Set<BookingMaterial>();
    public DbSet<BookingAssignment> BookingAssignments => Set<BookingAssignment>();
    public DbSet<BookingStatusHistory> BookingStatusHistories => Set<BookingStatusHistory>();
    public DbSet<BookingNote> BookingNotes => Set<BookingNote>();
    public DbSet<RecurringBooking> RecurringBookings => Set<RecurringBooking>();
    public DbSet<AmcContract> AmcContracts => Set<AmcContract>();
    public DbSet<PriceRule> PriceRules => Set<PriceRule>();
    public DbSet<PriceQuote> PriceQuotes => Set<PriceQuote>();
    public DbSet<QuoteRevision> QuoteRevisions => Set<QuoteRevision>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<CreditTransaction> CreditTransactions => Set<CreditTransaction>();
    public DbSet<CommissionRule> CommissionRules => Set<CommissionRule>();
    public DbSet<ProfessionalEarning> ProfessionalEarnings => Set<ProfessionalEarning>();
    public DbSet<Payout> Payouts => Set<Payout>();
    public DbSet<ProfessionalAdjustment> ProfessionalAdjustments => Set<ProfessionalAdjustment>();
    public DbSet<ProfessionalIncentive> ProfessionalIncentives => Set<ProfessionalIncentive>();
    public DbSet<PaymentGatewayWebhookEvent> PaymentGatewayWebhookEvents => Set<PaymentGatewayWebhookEvent>();
    public DbSet<PaymentGatewaySetting> PaymentGatewaySettings => Set<PaymentGatewaySetting>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponRedemption> CouponRedemptions => Set<CouponRedemption>();
    public DbSet<Referral> Referrals => Set<Referral>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<CustomerMembership> CustomerMemberships => Set<CustomerMembership>();
    public DbSet<HsReview> HomeServiceReviews => Set<HsReview>();
    public DbSet<ReviewMedia> ReviewMedia => Set<ReviewMedia>();
    public DbSet<User> HomeServiceUsers => Set<User>();
    public DbSet<Role> HomeServiceRoles => Set<Role>();
    public DbSet<UserRole> HomeServiceUserRoles => Set<UserRole>();
    public DbSet<Permission> HomeServicePermissions => Set<Permission>();
    public DbSet<RolePermission> HomeServiceRolePermissions => Set<RolePermission>();
    public DbSet<HsCustomer> HomeServiceCustomers => Set<HsCustomer>();
    public DbSet<CustomerAddress> HomeServiceCustomerAddresses => Set<CustomerAddress>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<Dispute> Disputes => Set<Dispute>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();
    public DbSet<HsNotification> HomeServiceNotifications => Set<HsNotification>();
    public DbSet<CmsPage> CmsPages => Set<CmsPage>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<Faq> Faqs => Set<Faq>();
    public DbSet<HsAuditLog> HomeServiceAuditLogs => Set<HsAuditLog>();

    // Shared persistence transport for frontend module collections.
    public DbSet<ModuleDataDocument> ModuleDataDocuments => Set<ModuleDataDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<ModuleDataDocument>()
            .HasIndex(document => new { document.Module, document.Collection })
            .IsUnique();

        // Global query filters for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AppDbContext).GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(null, new object[] { modelBuilder });
            }
        }
    }

    private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
