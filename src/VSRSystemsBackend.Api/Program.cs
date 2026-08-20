using Serilog;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "VSR Systems Backend API", Version = "v1" });
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddHostedService<VSRSystemsBackend.Api.Services.JobsScraperScheduler>();

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

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VSR Systems Backend API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseSerilogRequestLogging();
app.UseAuthorization();
app.MapControllers();

// Ensure database is created (skip seeder in production to save memory)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();
    if (app.Environment.IsDevelopment())
    {
        await HomeServicesSeeder.SeedAsync(context);
    }
}

app.Run();