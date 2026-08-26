using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Realtime;
using VSRSystemsBackend.Api.Modules.Railway.API.Hubs;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.BackgroundJobs;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Storage;
using VSRSystemsBackend.Api.Platform.Storage;
using VSRSystemsBackend.Api.Modules.Railway.Application.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Application.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Application.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;

namespace VSRSystemsBackend.Api.Modules.Railway;

public static class RailwayModule
{
    public static IServiceCollection AddRailwayModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddAuthorization();
        services.AddScoped<IRailwayScopeAccessor, RailwayScopeAccessor>();
        services.AddScoped<IRailwayFeatureGate, RailwayFeatureGate>();
        services.AddScoped<MasterDataHandlers>();
        services.AddScoped<RailwayOfflineCommandRegistry>();
        services.AddScoped<RailwayOfflineSyncHandler>();
        services.AddScoped<IRailwayRealtimePublisher, RailwayRealtimePublisher>();
        services.AddHttpClient<PrivateFileStorage>();
        services.AddScoped<IPrivateFileStorage>(provider => provider.GetRequiredService<PrivateFileStorage>());
        services.AddSingleton<IFileMalwareScanner, ClamAvFileMalwareScanner>();
        services.AddScoped<IRailwayEvidenceService, RailwayEvidenceService>();
        services.AddScoped<IRailwayEventPublisher, RailwayEventPublisher>();
        services.AddScoped<InspectionHandlers>();
        services.AddScoped<IRailwayOfflineCommandHandler, StartInspectionOfflineHandler>();
        services.AddScoped<IRailwayOfflineCommandHandler, SaveInspectionAnswerOfflineHandler>();
        services.AddScoped<IRailwayOfflineCommandHandler, SubmitInspectionOfflineHandler>();
        services.AddScoped<MaintenanceHandlers>();
        services.AddScoped<CrowdHandlers>();
        services.AddDataProtection();
        services.AddScoped<ICrowdSourceSecretProtector, CrowdSourceSecretProtector>();
        services.AddScoped<CrowdAdapterAuthenticator>();
        services.AddScoped<CrowdIngestionService>();
        services.AddScoped<ICrowdObservationAdapter, ManualCrowdAdapter>();
        services.AddHostedService<RailwayEvidenceScanWorker>();
        services.AddHostedService<CrowdRiskWorker>();
        services.AddHostedService<InspectionScheduleWorker>();
        services.AddHostedService<MaintenanceScheduleWorker>();
        services.AddSingleton<IRailwayIntegrationEventSink, RailwayIntegrationEventLogSink>();
        services.AddHostedService<RailwayOutboxDispatcher>();
        services.AddDbContext<RailwayDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql =>
                {
                    npgsql.UseNetTopologySuite();
                    npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "railway");
                }));
        return services;
    }

    public static IEndpointRouteBuilder MapRailwayEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<RailwayHub>("/hubs/railway");
        return endpoints;
    }
}
