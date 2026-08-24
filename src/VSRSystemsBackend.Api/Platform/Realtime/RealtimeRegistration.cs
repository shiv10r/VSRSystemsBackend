using VSRSystemsBackend.Application.Platform.Realtime;
using VSRSystemsBackend.Infrastructure.Platform.Realtime;

namespace VSRSystemsBackend.Api.Platform.Realtime;

public static class RealtimeRegistration
{
    public const string HubPath = "/hubs/realtime";

    public static IServiceCollection AddRealtime(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IRealtimePublisher, SignalRRealtimePublisher>();
        services.AddScoped<IRealtimeSubscriptionAuthorizer, HomeServicesRealtimeSubscriptionAuthorizer>();
        return services;
    }

    public static IEndpointRouteBuilder MapRealtime(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<RealtimeHub>(HubPath);
        return endpoints;
    }
}
