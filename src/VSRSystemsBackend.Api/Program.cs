using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using FluentValidation.AspNetCore;
using VSRSystemsBackend.Infrastructure.Persistence;
using VSRSystemsBackend.Infrastructure.Data.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
// JSON: keep default camelCase naming so responses match frontend expectations
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "VSR Systems Backend API", Version = "v1" });
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Distributed cache: Redis with in-memory fallback so the app works with or without Redis
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDistributedCache, VSRSystemsBackend.Api.Infrastructure.Caching.ResilientDistributedCache>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(VSRSystemsBackend.Application.Warehouse.Services.WarehouseService).Assembly));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Repository registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IWarehouseRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.WarehouseRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ILocationBinRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.LocationBinRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IInventoryRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.InventoryRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ISupplierRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.SupplierRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ICustomerRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.CustomerRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IPurchaseOrderRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.PurchaseOrderRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IGrnRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.GrnRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ISalesOrderRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.SalesOrderRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IStockTransferRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.StockTransferRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IPickListRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.PickListRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IPackageRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.PackageRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IDispatchRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.DispatchRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IReturnRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.ReturnRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IStockCountRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.StockCountRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IStaffRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.StaffRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IProjectRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.ProjectRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IStockMovementRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.StockMovementRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IStockAdjustmentRepository, VSRSystemsBackend.Infrastructure.Repositories.Warehouse.StockAdjustmentRepository>();

// Jobs repository registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IJobRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.JobRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.ICompanyRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.CompanyRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IJobApplicationRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.JobApplicationRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.ICandidateRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.CandidateRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.ISavedJobRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.SavedJobRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IScreeningQuestionRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.ScreeningQuestionRepository>();

// Jobs scraper repository registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IJobSourceRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.JobSourceRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IJobSourceConfigRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.JobSourceConfigRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IRawExternalJobRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.RawExternalJobRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IScrapeRunRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.ScrapeRunRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IScrapeLogRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.ScrapeLogRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IJobSourceMappingRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.JobSourceMappingRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IDuplicateCandidateRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.DuplicateCandidateRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IIngestionErrorRepository, VSRSystemsBackend.Infrastructure.Repositories.Jobs.IngestionErrorRepository>();

// Jobs scraper service
builder.Services.AddHttpClient<VSRSystemsBackend.Application.Jobs.Interfaces.IJobsScraperService, VSRSystemsBackend.Application.Jobs.Services.JobsScraperService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
    client.DefaultRequestHeaders.Add("Accept", "application/json, application/xml, text/html, */*");
});
builder.Services.AddHostedService<VSRSystemsBackend.Api.Modules.Jobs.Services.JobsScraperScheduler>();

// Jobs service registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IJobService, VSRSystemsBackend.Application.Jobs.Services.JobService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.ICompanyService, VSRSystemsBackend.Application.Jobs.Services.CompanyService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IJobApplicationService, VSRSystemsBackend.Application.Jobs.Services.JobApplicationService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.ICandidateService, VSRSystemsBackend.Application.Jobs.Services.CandidateService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.ISavedJobService, VSRSystemsBackend.Application.Jobs.Services.SavedJobService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Jobs.Interfaces.IScreeningQuestionService, VSRSystemsBackend.Application.Jobs.Services.ScreeningQuestionService>();

// Service registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IWarehouseService, VSRSystemsBackend.Application.Warehouse.Services.WarehouseService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ILocationBinService, VSRSystemsBackend.Application.Warehouse.Services.LocationBinService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IInventoryService, VSRSystemsBackend.Application.Warehouse.Services.InventoryService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ISupplierService, VSRSystemsBackend.Application.Warehouse.Services.SupplierService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ICustomerService, VSRSystemsBackend.Application.Warehouse.Services.CustomerService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.IPurchaseOrderService, VSRSystemsBackend.Application.Warehouse.Services.PurchaseOrderService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Warehouse.Interfaces.ISalesOrderService, VSRSystemsBackend.Application.Warehouse.Services.SalesOrderService>();

// HomeServices repository registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IServiceCatalogRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.ServiceCatalogRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ILocationRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.LocationRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IProfessionalRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.ProfessionalRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IBookingRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.BookingRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IRecurringBookingRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.RecurringBookingRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IAmcContractRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.AmcContractRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IPriceQuoteRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.PriceQuoteRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IPriceRuleRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.PriceRuleRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ICouponRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.CouponRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IMembershipRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.MembershipRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IPaymentRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.PaymentRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IRefundRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.RefundRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ICreditTransactionRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.CreditTransactionRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IEarningsRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.EarningsRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IPayoutRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.PayoutRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IReviewRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.ReviewRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ISupportRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.SupportRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IDisputeRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.DisputeRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.INotificationRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.NotificationRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ICommissionRuleRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.CommissionRuleRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IUserRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.UserRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ICustomerRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.CustomerRepository>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IAnalyticsRepository, VSRSystemsBackend.Infrastructure.Repositories.HomeServices.AnalyticsRepository>();

// HomeServices service registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IServiceCatalogService, VSRSystemsBackend.Application.HomeServices.Services.ServiceCatalogService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ILocationService, VSRSystemsBackend.Application.HomeServices.Services.LocationService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IProfessionalService, VSRSystemsBackend.Application.HomeServices.Services.ProfessionalService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IPriceQuoteService, VSRSystemsBackend.Application.HomeServices.Services.PriceQuoteService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IBookingService, VSRSystemsBackend.Application.HomeServices.Services.BookingService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IAssignmentService, VSRSystemsBackend.Application.HomeServices.Services.AssignmentService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IPaymentService, VSRSystemsBackend.Application.HomeServices.Services.PaymentService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IEarningsService, VSRSystemsBackend.Application.HomeServices.Services.EarningsService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IPayoutService, VSRSystemsBackend.Application.HomeServices.Services.PayoutService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IAnalyticsService, VSRSystemsBackend.Application.HomeServices.Services.AnalyticsService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.IReviewService, VSRSystemsBackend.Application.HomeServices.Services.ReviewService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ISupportService, VSRSystemsBackend.Application.HomeServices.Services.SupportService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.HomeServices.Interfaces.ICustomerAddressesService, VSRSystemsBackend.Application.HomeServices.Services.CustomerAddressesService>();

// Travel repository registrations (generic repository covers travel entities)
builder.Services.AddScoped(typeof(VSRSystemsBackend.Core.Interfaces.IRepository<>), typeof(VSRSystemsBackend.Infrastructure.Repositories.Repository<>));
builder.Services.AddScoped<VSRSystemsBackend.Application.Platform.ModuleData.IModuleDataService, VSRSystemsBackend.Infrastructure.Platform.ModuleData.ModuleDataService>();

// Travel service registrations
builder.Services.AddScoped<VSRSystemsBackend.Application.Travel.Interfaces.ITravelDestinationService, VSRSystemsBackend.Application.Travel.Services.TravelDestinationService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Travel.Interfaces.ITravelPackageService, VSRSystemsBackend.Application.Travel.Services.TravelPackageService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Travel.Interfaces.ITravelDepartureService, VSRSystemsBackend.Application.Travel.Services.TravelDepartureService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Travel.Interfaces.ITravelBookingService, VSRSystemsBackend.Application.Travel.Services.TravelBookingService>();
builder.Services.AddScoped<VSRSystemsBackend.Application.Travel.Interfaces.ITravelPaymentService, VSRSystemsBackend.Application.Travel.Services.TravelPaymentService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                ?? new[] { "http://localhost:5173", "http://localhost:3000" })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? "";
    options.CallbackPath = "/signin-google";
    options.SaveTokens = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
// Swagger enabled in all environments so the home-services API can be tested on Render too
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VSR Systems Backend API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseAuthorization();
app.MapControllers();

// Ensure database is created (skip seeder in production to save memory)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();
    await context.Database.ExecuteSqlRawAsync("""
        CREATE TABLE IF NOT EXISTS "ModuleDataDocuments" (
            "Id" uuid NOT NULL,
            "Module" character varying(50) NOT NULL,
            "Collection" character varying(150) NOT NULL,
            "Json" text NOT NULL,
            "CreatedAt" timestamp with time zone NOT NULL,
            "UpdatedAt" timestamp with time zone NULL,
            "IsDeleted" boolean NOT NULL DEFAULT FALSE,
            CONSTRAINT "PK_ModuleDataDocuments" PRIMARY KEY ("Id")
        );
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_ModuleDataDocuments_Module_Collection"
            ON "ModuleDataDocuments" ("Module", "Collection");
        """);
    if (app.Environment.IsDevelopment())
    {
        await HomeServicesSeeder.SeedAsync(context);
    }

    try
    {
        await TravelSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Travel seeding skipped: {Message}", ex.Message);
    }
}

app.Run();
