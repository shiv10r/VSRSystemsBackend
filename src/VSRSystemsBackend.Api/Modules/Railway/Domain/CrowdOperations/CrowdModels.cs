namespace VSRSystemsBackend.Api.Domain.CrowdOperations
{
    /// <summary>
    /// Aggregate, privacy-safe crowd observation. Contains NO person or device identifiers.
    /// </summary>
    public sealed record NormalizedCrowdObservation(
        Guid OrganizationId,
        Guid DivisionId,
        Guid StationId,
        Guid StationZoneId,
        Guid SourceId,
        string SourceEventId,
        DateTimeOffset WindowStart,
        DateTimeOffset WindowEnd,
        int Count,
        int? Inflow,
        int? Outflow,
        decimal Confidence,
        IReadOnlySet<string> QualityFlags);

    public enum CrowdRiskLevel { Normal, Warning, Critical }

    public enum CrowdDataQuality { Good, Degraded, Stale }

    public class CrowdAlert
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StationZoneId { get; set; }
        public CrowdRiskLevel Level { get; set; } = CrowdRiskLevel.Warning;
        public bool IsOpen { get; set; } = true;
        public DateTime RaisedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcknowledgedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }

    public class CrowdIncident
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StationId { get; set; }
        public string Title { get; set; } = "";
        public string Status { get; set; } = "Open";
        public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
    }

    public class CrowdSource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string AdapterType { get; set; } = "";
        public bool IsEnabled { get; set; } = true;
        public DateTime? LastObservationAt { get; set; }
        public int QuarantineCount { get; set; }
    }
}