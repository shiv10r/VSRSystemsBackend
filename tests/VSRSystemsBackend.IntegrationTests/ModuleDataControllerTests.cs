using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Platform.ModuleData;
using VSRSystemsBackend.Application.Platform.ModuleData;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class ModuleDataControllerTests
{
    [Theory]
    [InlineData(typeof(SchoolDataController), "api/school")]
    [InlineData(typeof(HotelDataController), "api/hotel")]
    [InlineData(typeof(NewsDataController), "api/news")]
    [InlineData(typeof(CommerceDataController), "api/commerce")]
    [InlineData(typeof(BankDataController), "api/bank")]
    [InlineData(typeof(MedicalDataController), "api/medical")]
    [InlineData(typeof(InteriorDataController), "api/interior")]
    [InlineData(typeof(WarehouseDataController), "api/warehouse")]
    [InlineData(typeof(HomeServicesDataController), "api/home-services")]
    [InlineData(typeof(JobsDataController), "api/jobs")]
    [InlineData(typeof(TravelDataController), "api/travel")]
    [InlineData(typeof(PlatformDataController), "api/platform")]
    public void EveryModuleHasADataRoute(Type controllerType, string expectedRoute)
    {
        var route = controllerType.GetCustomAttribute<RouteAttribute>();

        Assert.Equal(expectedRoute, route?.Template);
    }

    [Fact]
    public async Task PutAndGetUseTheSharedPersistenceBoundary()
    {
        var service = new FakeModuleDataService();
        var controller = new SchoolDataController(service);
        using var payload = JsonDocument.Parse("""[{"id":"student-1"}]""");

        var putResult = await controller.Put("students", payload.RootElement, CancellationToken.None);
        var getResult = await controller.Get("students", CancellationToken.None);

        Assert.IsType<NoContentResult>(putResult);
        var content = Assert.IsType<ContentResult>(getResult);
        Assert.Equal("application/json", content.ContentType);
        Assert.Equal(payload.RootElement.GetRawText(), content.Content);
        Assert.Equal("school", service.LastModule);
    }

    private sealed class FakeModuleDataService : IModuleDataService
    {
        private string? _json;

        public string? LastModule { get; private set; }

        public Task<string?> GetAsync(string module, string collection, CancellationToken cancellationToken = default)
        {
            LastModule = module;
            return Task.FromResult(_json);
        }

        public Task SaveAsync(string module, string collection, string json, CancellationToken cancellationToken = default)
        {
            LastModule = module;
            _json = json;
            return Task.CompletedTask;
        }
    }
}
