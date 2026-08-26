using VSRSystemsBackend.Application.Platform.Realtime;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Realtime;

public sealed record RailwayRealtimeEvent(Guid EventId, string Type, Guid ResourceId, DateTimeOffset OccurredAt);

public interface IRailwayRealtimePublisher
{
    Task PublishToStationAsync(Guid organizationId, Guid stationId, RailwayRealtimeEvent message, CancellationToken cancellationToken);
}

public sealed class RailwayRealtimePublisher(IRealtimePublisher publisher) : IRailwayRealtimePublisher
{
    public Task PublishToStationAsync(
        Guid organizationId,
        Guid stationId,
        RailwayRealtimeEvent message,
        CancellationToken cancellationToken) =>
        publisher.SendToGroupAsync(
            RailwayRealtimeGroups.Station(organizationId, stationId),
            new RealtimeEventEnvelope<RailwayRealtimeEvent>(
                message.EventId, message.Type, 1, message.OccurredAt, null,
                organizationId.ToString(), message),
            cancellationToken);
}

public static class RailwayRealtimeGroups
{
    public static string Station(Guid organizationId, Guid stationId) => $"railway:{organizationId}:station:{stationId}";
}
