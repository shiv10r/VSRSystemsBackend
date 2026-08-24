using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging.Abstractions;
using VSRSystemsBackend.Api.Controllers;
using VSRSystemsBackend.Api.Platform.Chat;
using VSRSystemsBackend.Api.Platform.Health;
using VSRSystemsBackend.Api.Platform.Realtime;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.Platform.Realtime;
using VSRSystemsBackend.Infrastructure.Persistence.Mongo;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class RealtimeAndMongoContractTests
{
    [Fact]
    public void RealtimeHubRequiresAuthenticationAndExposesOnlySpecificSubscriptionMethods()
    {
        Assert.NotNull(typeof(RealtimeHub).GetCustomAttribute<AuthorizeAttribute>());
        Assert.Equal("/hubs/realtime", RealtimeRegistration.HubPath);

        var declaredMethods = typeof(RealtimeHub)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(method => method.Name)
            .ToArray();

        Assert.Contains(nameof(RealtimeHub.SubscribeToHomeServicesBooking), declaredMethods);
        Assert.Contains(nameof(RealtimeHub.UnsubscribeFromHomeServicesBooking), declaredMethods);
        Assert.Contains(nameof(RealtimeHub.SubscribeToChatConversation), declaredMethods);
        Assert.Contains(nameof(RealtimeHub.UnsubscribeFromChatConversation), declaredMethods);
        Assert.DoesNotContain("SubscribeToGroup", declaredMethods);
        Assert.DoesNotContain("JoinGroup", declaredMethods);
    }

    [Fact]
    public void ChatEndpointsRequireAuthenticationAndMatchConversationContract()
    {
        Assert.NotNull(typeof(ChatController).GetCustomAttribute<AuthorizeAttribute>());
        Assert.Equal(
            "api/v1/chat/conversations/{conversationId}/messages",
            typeof(ChatController).GetCustomAttribute<RouteAttribute>()?.Template);

        Assert.NotNull(typeof(ChatController)
            .GetMethod(nameof(ChatController.GetMessages))!
            .GetCustomAttribute<HttpGetAttribute>());
        Assert.NotNull(typeof(ChatController)
            .GetMethod(nameof(ChatController.SendMessage))!
            .GetCustomAttribute<HttpPostAttribute>());
    }

    [Theory]
    [InlineData(nameof(HomeServiceBookingsController.Cancel), "{id}/cancel")]
    [InlineData(nameof(HomeServiceBookingsController.Assign), "{id}/assign")]
    [InlineData(nameof(HomeServiceBookingsController.Confirm), "{id}/confirm")]
    [InlineData(nameof(HomeServiceBookingsController.Start), "{id}/start")]
    [InlineData(nameof(HomeServiceBookingsController.Complete), "{id}/complete")]
    [InlineData(nameof(HomeServiceBookingsController.Reschedule), "{id}/reschedule")]
    public void BookingMutationRoutesRemainAvailable(string methodName, string route)
    {
        var attribute = typeof(HomeServiceBookingsController)
            .GetMethod(methodName)!
            .GetCustomAttributes<HttpPostAttribute>()
            .Single();

        Assert.Equal(route, attribute.Template);
    }

    [Fact]
    public void BookingStatusEventUsesStableVersionedContract()
    {
        var payload = new BookingStatusChangedPayload("booking-123", "completed", "professional-1", null);
        var message = new RealtimeEventEnvelope<BookingStatusChangedPayload>(
            Guid.NewGuid(),
            RealtimeEventTypes.HomeServicesBookingStatusChanged,
            1,
            DateTimeOffset.UtcNow,
            "trace-1",
            null,
            payload);

        Assert.Equal("home-services.booking.status-changed", message.EventType);
        Assert.Equal(1, message.Version);
        Assert.Equal("context:home-services.booking:booking-123", RealtimeGroups.HomeServicesBooking(payload.BookingId));
        Assert.Equal("realtimeEvent", RealtimeGroups.ClientMethod);
    }

    [Fact]
    public async Task MongoHealthCheckIsDegradedRatherThanUnhealthyWhenUnconfigured()
    {
        var mongoDb = new MongoDbContext(new MongoDbOptions(), null, null);
        var healthCheck = new MongoDbHealthCheck(mongoDb, NullLogger<MongoDbHealthCheck>.Instance);

        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        Assert.Equal(HealthStatus.Degraded, result.Status);
        Assert.False(mongoDb.IsConfigured);
        Assert.Throws<InvalidOperationException>(() => mongoDb.GetCollection<object>(MongoCollectionNames.ChatMessages));
    }
}
