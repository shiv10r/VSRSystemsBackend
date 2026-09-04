using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using FluentValidation.AspNetCore;
using MongoDB.Driver;
using VSRSystemsBackend.Infrastructure.Persistence;
using VSRSystemsBackend.Infrastructure.Persistence.Mongo;
using VSRSystemsBackend.Infrastructure.Data.Seeds;
using VSRSystemsBackend.Api.Infrastructure.Authentication;
using VSRSystemsBackend.Api.Infrastructure.Configuration;
using VSRSystemsBackend.Api.Infrastructure.Observability;
using VSRSystemsBackend.Api.Platform.Chat;
using VSRSystemsBackend.Api.Platform.FeatureFlags;
using VSRSystemsBackend.Api.Platform.Health;
using VSRSystemsBackend.Api.Platform.Maps;
using VSRSystemsBackend.Api.Platform.AI;
using VSRSystemsBackend.Api.Platform.Realtime;
using VSRSystemsBackend.Api.Platform.Storage;
using VSRSystemsBackend.Api.Platform.Settings;
using VSRSystemsBackend.Api.Platform.Weather;
using VSRSystemsBackend.Api.Modules.Railway;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
// JSON: keep default camelCase naming so responses match frontend expectations
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "VSR Systems Backend API", Version = "v1" });
    c.CustomSchemaIds(Program.GetSwaggerSchemaId);
    c.CustomOperationIds(apiDescription =>
    {
        if (!apiDescription.RelativePath?.StartsWith("api/railway", StringComparison.OrdinalIgnoreCase) ?? true)
            return null;

        var controller = apiDescription.ActionDescriptor.RouteValues["controller"] ?? "endpoint";
        var action = apiDescription.ActionDescriptor.RouteValues["action"] ?? apiDescription.HttpMethod ?? "operation";
        return $"railway.{controller}.{action}".ToLowerInvariant();
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "opaque",
        Description = "VSR authenticated bearer session token.",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
        }] = Array.Empty<string>(),
    });
});

var openTelemetry = builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("VSRSystemsBackend.Api"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddMeter(RailwayTelemetry.MeterName));

if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
{
    openTelemetry
        .WithTracing(tracing => tracing.AddOtlpExporter())
        .WithMetrics(metrics => metrics.AddOtlpExporter());
}

// Database
var databaseConnectionString = DatabaseConnectionString.Resolve(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(databaseConnectionString));
builder.Services.AddRailwayModule(builder.Configuration);
builder.Services.AddRateLimiter(options => options.AddPolicy("railway-ingestion", context =>
    RateLimitPartition.GetFixedWindowLimiter(
        context.Request.Headers["X-Railway-Source-Id"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 120, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 })));

// MongoDB is optional and isolated from PostgreSQL-backed modules.
var mongoSection = builder.Configuration.GetSection(MongoDbOptions.SectionName);
builder.Services.Configure<MongoDbOptions>(mongoSection);
var mongoOptions = mongoSection.Get<MongoDbOptions>() ?? new MongoDbOptions();
IMongoClient? mongoClient = null;
IMongoDatabase? mongoDatabase = null;
string? mongoConfigurationError = null;

if (mongoOptions.IsConfigured)
{
    try
    {
        mongoClient = new MongoClient(mongoOptions.ConnectionString);
        mongoDatabase = mongoClient.GetDatabase(mongoOptions.DatabaseName);
        builder.Services.AddSingleton(mongoClient);
        builder.Services.AddSingleton(mongoDatabase);
    }
    catch (Exception)
    {
        mongoConfigurationError = "MongoDB configuration is invalid; document-backed features are disabled.";
        Log.Warning("MongoDB configuration is invalid; document-backed features are disabled");
    }
}

builder.Services.AddSingleton(new MongoDbContext(
    mongoOptions,
    mongoClient,
    mongoDatabase,
    mongoConfigurationError));
builder.Services.AddHealthChecks()
    .AddCheck<MongoDbHealthCheck>(
        "mongodb",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "mongodb", "documents" },
        timeout: TimeSpan.FromSeconds(5));
builder.Services.AddRealtime();
builder.Services.AddChat();

// Distributed cache: Redis with in-memory fallback so the app works with or without Redis
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDistributedCache, VSRSystemsBackend.Api.Infrastructure.Caching.ResilientDistributedCache>();

// Server-side map provider access keeps credentials private and makes Redis caching effective.
builder.Services.Configure<GeoapifyOptions>(builder.Configuration.GetSection(GeoapifyOptions.SectionName));
builder.Services.AddHttpClient<GeoapifyService>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<GeoapifyOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
}).AddStandardResilienceHandler();

// Platform integrations are server-side so provider credentials never reach the browser.
builder.Services.Configure<WeatherOptions>(builder.Configuration.GetSection(WeatherOptions.SectionName));
builder.Services.AddHttpClient<OpenMeteoWeatherService>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<WeatherOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
}).AddStandardResilienceHandler();

builder.Services.Configure<AiGatewayOptions>(builder.Configuration.GetSection(AiGatewayOptions.SectionName));
builder.Services.AddHttpClient<AiGatewayService>(client =>
    client.Timeout = Timeout.InfiniteTimeSpan);

builder.Services.Configure<SupabaseStorageOptions>(builder.Configuration.GetSection(SupabaseStorageOptions.SectionName));
builder.Services.AddHttpClient<SupabaseStorageService>(client =>
    client.Timeout = TimeSpan.FromSeconds(15));
builder.Services.Configure<UploadNotificationOptions>(builder.Configuration.GetSection(UploadNotificationOptions.SectionName));
builder.Services.PostConfigure<UploadNotificationOptions>(options =>
{
    if (string.IsNullOrWhiteSpace(options.ResendApiKey))
        options.ResendApiKey = builder.Configuration["RESEND_API_KEY"] ?? string.Empty;
    if (string.IsNullOrWhiteSpace(options.RecipientEmail))
        options.RecipientEmail = builder.Configuration["UPLOAD_NOTIFICATION_EMAIL"] ?? string.Empty;
});
builder.Services.Configure<FeatureFlagsOptions>(builder.Configuration.GetSection(FeatureFlagsOptions.SectionName));
builder.Services.Configure<SettingsOptions>(builder.Configuration.GetSection(SettingsOptions.SectionName));
builder.Services.AddSingleton<FeatureFlagService>();
builder.Services.AddHttpClient<UploadNotificationService>(client =>
{
    client.BaseAddress = new Uri("https://api.resend.com/");
    client.Timeout = TimeSpan.FromSeconds(10);
}).AddStandardResilienceHandler();

// AutoMapper
builder.Services.AddAutoMapper(config =>
    config.CreateMap<VSRSystemsBackend.Domain.Jobs.JobSource, VSRSystemsBackend.Application.Jobs.DTOs.JobSourceDto>(),
    AppDomain.CurrentDomain.GetAssemblies());

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
}).AddStandardResilienceHandler();
if (builder.Configuration.GetValue<bool>("JobsScraper:SchedulerEnabled"))
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
        var allowedOrigins = new[]
            {
                "http://localhost:5173",
                "http://localhost:3000",
                "https://vsrsystems1.netlify.app",
                "https://luxinfra.netlify.app"
            }
            .Concat(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Authentication
var authentication = builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CacheTokenAuthenticationHandler.SchemeName;
    options.DefaultChallengeScheme = CacheTokenAuthenticationHandler.SchemeName;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddScheme<AuthenticationSchemeOptions, CacheTokenAuthenticationHandler>(CacheTokenAuthenticationHandler.SchemeName, _ => { })
.AddCookie();

var googleClientId = builder.Configuration["Google:ClientId"];
var googleClientSecret = builder.Configuration["Google:ClientSecret"];
if (!string.IsNullOrWhiteSpace(googleClientId) &&
    !string.IsNullOrWhiteSpace(googleClientSecret) &&
    !googleClientId.StartsWith("YOUR_", StringComparison.OrdinalIgnoreCase) &&
    !googleClientSecret.StartsWith("YOUR_", StringComparison.OrdinalIgnoreCase))
{
    authentication.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.CallbackPath = "/signin-google";
        options.SaveTokens = true;
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<CorrelationIdMiddleware>();
// Swagger enabled in all environments so the home-services API can be tested on Render too
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VSR Systems Backend API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowFrontend");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();
app.MapRailwayEndpoints();
app.MapRealtime();
app.MapGet("/", () => Results.Ok(new { service = "VSR Systems Backend API", status = "healthy" }));
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            status = report.Status.ToString().ToLowerInvariant(),
            checks = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => new
                {
                    status = entry.Value.Status.ToString().ToLowerInvariant(),
                    description = entry.Value.Description
                })
        });
    }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

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
        CREATE TABLE IF NOT EXISTS "TravelDepartures" (
            "Id" character varying(50) CONSTRAINT "PK_TravelDepartures" PRIMARY KEY,
            "Code" character varying(50) NOT NULL,
            "Title" character varying(200) NOT NULL,
            "DepartureCity" character varying(100) NOT NULL,
            "PackageId" character varying(50) NOT NULL,
            "DepartureDate" timestamp with time zone NOT NULL,
            "AvailableSeats" integer NOT NULL,
            "TotalSeats" integer NOT NULL,
            "Price" numeric NOT NULL,
            "DiscountedPrice" numeric NULL,
            "ImageUrl" character varying(3000) NULL,
            "Status" text NOT NULL,
            "CreatedAt" timestamp with time zone NOT NULL,
            "UpdatedAt" timestamp with time zone NULL,
            "IsDeleted" boolean NOT NULL DEFAULT FALSE,
            "CreatedBy" text NOT NULL,
            "UpdatedBy" text NULL
        );
        """);
    var seedMode = builder.Configuration["SeedData:Mode"] ?? "None";
    if (seedMode.Equals("Full", StringComparison.OrdinalIgnoreCase))
    {
        await HomeServicesSeeder.SeedAsync(context);
    }
    else if (seedMode.Equals("Sample", StringComparison.OrdinalIgnoreCase))
    {
        await HomeServicesSeeder.SeedSampleAsync(context);
    }

    try
    {
        if (!seedMode.Equals("None", StringComparison.OrdinalIgnoreCase))
        {
            await TravelSeeder.SeedAsync(context);
        }
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Travel seeding skipped: {Message}", ex.Message);
    }
}

app.Run();

public partial class Program
{
    internal static string GetSwaggerSchemaId(Type type)
    {
        if (!type.IsGenericType)
            return type.FullName?.Replace('+', '.') ?? type.Name;

        var typeName = type.Name[..type.Name.IndexOf('`')];
        return $"{typeName}Of{string.Join("And", type.GenericTypeArguments.Select(GetSwaggerSchemaId))}";
    }
}
