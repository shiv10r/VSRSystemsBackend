using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

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
        services.AddDbContext<RailwayDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "railway")));
        return services;
    }

    public static IEndpointRouteBuilder MapRailwayEndpoints(this IEndpointRouteBuilder endpoints) => endpoints;
}
