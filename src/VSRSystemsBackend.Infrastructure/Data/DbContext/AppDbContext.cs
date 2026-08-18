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
using VSRSystemsBackend.Infrastructure.Data.Configurations;

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
    public DbSet<Customer> Customers => Set<Customer>();
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
    public DbSet<Notification> Notifications => Set<Notification>();
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
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
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
    public DbSet<Review> Reviews => Set<Review>();

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
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<GroupTrip> GroupTrips => Set<GroupTrip>();
    public DbSet<TravelWishlist> TravelWishlists => Set<TravelWishlist>();
    public DbSet<TravelWishlistItem> TravelWishlistItems => Set<TravelWishlistItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

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