using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using VSRSystemsBackend.Api.Platform.AI;
using VSRSystemsBackend.Api.Platform.Storage;
using VSRSystemsBackend.Api.Platform.Weather;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class PlatformControllerContractTests
{
    [Fact]
    public void AiRoutesIncludeCanonicalAndCurrentFrontendContracts()
    {
        var statusRoutes = Routes<AiController>(nameof(AiController.Status), typeof(HttpGetAttribute));
        var chatRoutes = Routes<AiController>(nameof(AiController.Chat), typeof(HttpPostAttribute));

        Assert.Contains("/api/ai/status", statusRoutes);
        Assert.Contains("/api/assistant/ai/status", statusRoutes);
        Assert.Contains("/api/ai/chat", chatRoutes);
        Assert.Contains("/api/assistant/ai", chatRoutes);
    }

    [Theory]
    [InlineData(typeof(WeatherController))]
    [InlineData(typeof(AiController))]
    [InlineData(typeof(StorageController))]
    public void PlatformProviderEndpointsRequireAuthentication(Type controllerType)
    {
        Assert.NotNull(controllerType.GetCustomAttribute<AuthorizeAttribute>());
    }

    [Fact]
    public void WeatherAndStorageRoutesMatchContracts()
    {
        Assert.Equal("api/weather", typeof(WeatherController).GetCustomAttribute<RouteAttribute>()?.Template);
        Assert.Equal("api/storage", typeof(StorageController).GetCustomAttribute<RouteAttribute>()?.Template);
        Assert.Contains("uploads/sign", Routes<StorageController>(nameof(StorageController.SignUpload), typeof(HttpPostAttribute)));
        Assert.Contains("downloads/sign", Routes<StorageController>(nameof(StorageController.SignDownload), typeof(HttpPostAttribute)));
        Assert.Contains("objects", Routes<StorageController>(nameof(StorageController.DeleteObject), typeof(HttpDeleteAttribute)));
    }

    private static string?[] Routes<TController>(string methodName, Type attributeType) =>
        typeof(TController)
            .GetMethod(methodName)!
            .GetCustomAttributes(attributeType)
            .Cast<HttpMethodAttribute>()
            .Select(attribute => attribute.Template)
            .ToArray();
}
