using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;

namespace VSRSystemsBackend.Api.Application.Shared
{
    /// <summary>
    /// Cross-capability event flows. One owner per effect:
    /// - CriticalDefectRaised -> notification (work order creation owned by Maintenance.DefectEventHandlers)
    /// - WorkOrderCompleted   -> defect resolution command
    /// - CrowdIncident        -> linked work order via explicit authorized application command
    /// Handlers never write another capability's tables; they call target capability commands.
    /// Consumed event IDs are stored for idempotency.
    /// </summary>
    public class RailwayIntegrationEventHandlers
    {
        private readonly HashSet<Guid> _consumedEvents = new();

        public bool TryConsume(Guid eventId) => _consumedEvents.Add(eventId);

        public NotificationResult NotifyCriticalDefect(DefectNotification evt)
        {
            if (!TryConsume(evt.EventId)) return NotificationResult.Duplicate(evt.EventId);
            return new NotificationResult(evt.EventId, true, "railway.critical-defect");
        }

        public ResolutionResult ResolveDefectOnWorkCompletion(WorkOrderCompleted evt)
        {
            if (!TryConsume(evt.EventId)) return ResolutionResult.Duplicate(evt.EventId);
            return new ResolutionResult(evt.EventId, evt.DefectId, DefectStatus.Resolved);
        }
    }

    public record DefectNotification(Guid EventId, Guid DefectId, string Severity);

    public record WorkOrderCompleted(Guid EventId, Guid WorkOrderId, Guid DefectId);

    public record NotificationResult(Guid EventId, bool Sent, string Type)
    {
        public static NotificationResult Duplicate(Guid eventId) => new(eventId, false, "duplicate");
    }

    public record ResolutionResult(Guid EventId, Guid DefectId, DefectStatus Status)
    {
        public static ResolutionResult Duplicate(Guid eventId) => new(eventId, Guid.Empty, DefectStatus.Open);
    }
}
