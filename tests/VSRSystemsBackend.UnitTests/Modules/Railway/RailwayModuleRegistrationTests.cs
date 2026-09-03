using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VSRSystemsBackend.Api.Modules.Railway;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.BackgroundJobs;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using Xunit;

namespace VSRSystemsBackend.UnitTests.Modules.Railway;

public sealed class RailwayModuleRegistrationTests
{
    [Fact]
    public void AddRailwayModule_registers_scope_feature_gate_and_database_boundary()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=railway;Username=test;Password=test",
            })
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);

        services.AddRailwayModule(configuration);

        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IRailwayScopeAccessor));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IRailwayFeatureGate));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(RailwayDbContext));
        Assert.DoesNotContain(services, descriptor => descriptor.ImplementationType == typeof(RailwayEvidenceScanWorker));
    }

    [Fact]
    public async Task Capabilities_respect_master_and_capability_flags()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RAILWAY_ENABLED"] = "true",
                ["RAILWAY_INSPECTION_ENABLED"] = "true",
                ["RAILWAY_MAINTENANCE_ENABLED"] = "false",
                ["RAILWAY_OFFLINE_PACK_MAX_AGE_HOURS"] = "200",
            })
            .Build();
        var scope = new RailwayScope(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new HashSet<Guid>(),
            new HashSet<string> { "railway.inspections.read" });

        var result = await new RailwayFeatureGate(configuration).GetAsync(scope, CancellationToken.None);

        Assert.True(result.RailwayEnabled);
        Assert.True(result.InspectionEnabled);
        Assert.False(result.MaintenanceEnabled);
        Assert.Equal(72, result.OfflinePackMaxAgeHours);
    }
}
