using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;

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

public sealed class CrowdObservation : RailwayEntity
{
    private CrowdObservation() { }
    public CrowdObservation(Guid id, NormalizedCrowdObservation value)
        : base(id, value.OrganizationId, value.DivisionId)
    {
        if (value.Count < 0 || value.Confidence is < 0 or > 1 || value.WindowEnd <= value.WindowStart)
            throw new ArgumentException("Crowd observation values are invalid.");
        StationId = value.StationId; StationZoneId = value.StationZoneId; SourceId = value.SourceId;
        SourceEventId = value.SourceEventId; WindowStart = value.WindowStart; WindowEnd = value.WindowEnd;
        Count = value.Count; Inflow = value.Inflow; Outflow = value.Outflow; Confidence = value.Confidence;
        QualityFlags = string.Join(',', value.QualityFlags.Order());
    }
    public Guid StationId { get; private set; }
    public Guid StationZoneId { get; private set; }
    public Guid SourceId { get; private set; }
    public string SourceEventId { get; private set; } = string.Empty;
    public DateTimeOffset WindowStart { get; private set; }
    public DateTimeOffset WindowEnd { get; private set; }
    public int Count { get; private set; }
    public int? Inflow { get; private set; }
    public int? Outflow { get; private set; }
    public decimal Confidence { get; private set; }
    public string QualityFlags { get; private set; } = string.Empty;
}

public sealed class CrowdSource : RailwayEntity
{
    private CrowdSource() { }
    public CrowdSource(Guid id, Guid organizationId, Guid divisionId, Guid stationId, Guid stationZoneId, string name, string adapterType, string signingSecretCiphertext)
        : base(id, organizationId, divisionId)
    { StationId = stationId; StationZoneId = stationZoneId; Name = name; AdapterType = adapterType; SigningSecretCiphertext = signingSecretCiphertext; }
    public Guid StationId { get; private set; }
    public Guid StationZoneId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string AdapterType { get; private set; } = string.Empty;
    public string SigningSecretCiphertext { get; private set; } = string.Empty;
    public string? PreviousSigningSecretCiphertext { get; private set; }
    public DateTimeOffset? PreviousSecretValidUntil { get; private set; }
    public bool Enabled { get; private set; } = true;
    public DateTimeOffset? LastObservationAt { get; private set; }
    public void RecordObservation(DateTimeOffset at) { LastObservationAt = at; Version++; }
    public void Disable() { Enabled = false; Version++; }
    public void RotateSigningSecret(string signingSecretCiphertext, DateTimeOffset previousValidUntil)
    {
        PreviousSigningSecretCiphertext = SigningSecretCiphertext; PreviousSecretValidUntil = previousValidUntil;
        SigningSecretCiphertext = signingSecretCiphertext; Version++;
    }
}

public sealed class CrowdThresholdPolicy : RailwayEntity
{
    private CrowdThresholdPolicy() { }
    public CrowdThresholdPolicy(Guid id, Guid organizationId, Guid divisionId, Guid stationId, Guid stationZoneId,
        int warningThreshold, int criticalThreshold, DateTimeOffset effectiveFrom, Guid createdBy)
        : base(id, organizationId, divisionId)
    {
        if (warningThreshold < 0 || criticalThreshold <= warningThreshold)
            throw new ArgumentException("Critical threshold must be greater than the warning threshold.");
        StationId = stationId; StationZoneId = stationZoneId; OriginalWarningThreshold = warningThreshold;
        OriginalCriticalThreshold = criticalThreshold; EffectiveFrom = effectiveFrom; CreatedBy = createdBy;
    }
    public Guid StationId { get; private set; }
    public Guid StationZoneId { get; private set; }
    public int OriginalWarningThreshold { get; private set; }
    public int OriginalCriticalThreshold { get; private set; }
    public int? OverrideWarningThreshold { get; private set; }
    public int? OverrideCriticalThreshold { get; private set; }
    public string? OverrideReason { get; private set; }
    public Guid? OverriddenBy { get; private set; }
    public DateTimeOffset EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveUntil { get; private set; }
    public Guid CreatedBy { get; private set; }
    public int WarningThreshold => OverrideWarningThreshold ?? OriginalWarningThreshold;
    public int CriticalThreshold => OverrideCriticalThreshold ?? OriginalCriticalThreshold;
    public void Override(int warning, int critical, string reason, Guid actor)
    {
        if (warning < 0 || critical <= warning || string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Override is invalid.");
        OverrideWarningThreshold = warning; OverrideCriticalThreshold = critical; OverrideReason = reason.Trim(); OverriddenBy = actor; Version++;
    }
    public void End(DateTimeOffset effectiveUntil)
    {
        if (effectiveUntil <= EffectiveFrom) throw new ArgumentException("Policy end must follow its start.");
        EffectiveUntil = effectiveUntil; Version++;
    }
}

public sealed class CrowdAlert : RailwayEntity
{
    private CrowdAlert() { }
    public CrowdAlert(Guid id, Guid organizationId, Guid divisionId, Guid stationId, Guid zoneId, CrowdRiskLevel level, DateTimeOffset raisedAt)
        : base(id, organizationId, divisionId) { StationId = stationId; StationZoneId = zoneId; Level = level; RaisedAt = raisedAt; }
    public Guid StationId { get; private set; }
    public Guid StationZoneId { get; private set; }
    public CrowdRiskLevel Level { get; private set; }
    public bool IsOpen { get; private set; } = true;
    public DateTimeOffset RaisedAt { get; private set; }
    public DateTimeOffset? AcknowledgedAt { get; private set; }
    public Guid? AcknowledgedBy { get; private set; }
    public void Acknowledge(Guid actor, DateTimeOffset now) { if (!IsOpen || AcknowledgedAt.HasValue) throw new InvalidOperationException(); AcknowledgedAt = now; AcknowledgedBy = actor; Version++; }
    public void Close(DateTimeOffset now) { if (!AcknowledgedAt.HasValue) throw new InvalidOperationException("Alert must be acknowledged before closing."); IsOpen = false; Version++; }
}

public sealed class CrowdIncident : RailwayEntity
{
    private CrowdIncident() { }
    public CrowdIncident(Guid id, Guid organizationId, Guid divisionId, Guid stationId, string title, Guid openedBy, DateTimeOffset openedAt)
        : base(id, organizationId, divisionId) { StationId = stationId; Title = title; OpenedBy = openedBy; OpenedAt = openedAt; }
    public Guid StationId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Open";
    public Guid OpenedBy { get; private set; }
    public DateTimeOffset OpenedAt { get; private set; }
    public string ResponseLog { get; private set; } = string.Empty;
    public DateTimeOffset? ClosedAt { get; private set; }
    public Guid? ClosedBy { get; private set; }
    public void RecordResponse(string action, Guid actor, DateTimeOffset now)
    {
        if (Status != "Open" || string.IsNullOrWhiteSpace(action)) throw new InvalidOperationException("Response action is invalid.");
        ResponseLog += $"{now:O}|{actor}|{action.Trim()}\n"; Version++;
    }
    public void Close(Guid actor, DateTimeOffset now)
    {
        if (Status != "Open") throw new InvalidOperationException("Incident is already closed.");
        Status = "Closed"; ClosedBy = actor; ClosedAt = now; Version++;
    }
}

public sealed class CrowdIngestionNonce
{
    private CrowdIngestionNonce() { }
    public CrowdIngestionNonce(Guid sourceId, string nonce, DateTimeOffset acceptedAt) { SourceId = sourceId; Nonce = nonce; AcceptedAt = acceptedAt; }
    public Guid SourceId { get; private set; }
    public string Nonce { get; private set; } = string.Empty;
    public DateTimeOffset AcceptedAt { get; private set; }
}

public sealed class CrowdQuarantineRecord : RailwayEntity
{
    private CrowdQuarantineRecord() { }
    public CrowdQuarantineRecord(Guid id, Guid organizationId, Guid? divisionId, Guid sourceId, string reason, string payloadHash, DateTimeOffset createdAt)
        : base(id, organizationId, divisionId) { SourceId = sourceId; Reason = reason; PayloadHash = payloadHash; CreatedAt = createdAt; }
    public Guid SourceId { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public string PayloadHash { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
}

public static class CrowdRiskPolicy
{
    public static CrowdRiskLevel Calculate(int count, int warningThreshold, int criticalThreshold) =>
        count >= criticalThreshold ? CrowdRiskLevel.Critical : count >= warningThreshold ? CrowdRiskLevel.Warning : CrowdRiskLevel.Normal;
    public static CrowdDataQuality Quality(NormalizedCrowdObservation observation, DateTimeOffset now) =>
        now - observation.WindowEnd > TimeSpan.FromMinutes(5) ? CrowdDataQuality.Stale :
        observation.Confidence < .5m ? CrowdDataQuality.Degraded : CrowdDataQuality.Good;
}

public sealed record CrowdThresholdBreached(
    Guid EventId,
    Guid OrganizationId,
    Guid StationId,
    Guid StationZoneId,
    Guid AlertId,
    CrowdRiskLevel Level,
    int Count,
    DateTimeOffset OccurredAt,
    string CorrelationId) : IRailwayDomainEvent
{
    public string EventName => "railway.crowd.threshold-breached";
    public int SchemaVersion => 1;
    public Guid? CausationId => null;
}
