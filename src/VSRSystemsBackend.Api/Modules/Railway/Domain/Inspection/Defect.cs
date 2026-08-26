using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;

public sealed class Defect : RailwayEntity
{
    private Defect() { }
    public Defect(Guid id, Guid organizationId, Guid divisionId, Guid inspectionRunId, Guid targetId, string description, DefectSeverity severity, DateTimeOffset raisedAt)
        : base(id, organizationId, divisionId)
    {
        InspectionRunId = inspectionRunId;
        TargetId = targetId;
        Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Description is required.") : description.Trim();
        Severity = severity;
        RaisedAt = raisedAt;
    }
    public Guid InspectionRunId { get; private set; }
    public Guid TargetId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DefectSeverity Severity { get; private set; }
    public DefectStatus Status { get; private set; } = DefectStatus.Open;
    public DateTimeOffset RaisedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public Guid? AssignedWorkOrderId { get; private set; }

    public void Triage() { Require(DefectStatus.Open); Status = DefectStatus.Triaged; Version++; }
    public void LinkWorkOrder(Guid workOrderId) { if (Status is not (DefectStatus.Open or DefectStatus.Triaged)) throw new InvalidOperationException(); AssignedWorkOrderId = workOrderId; Status = DefectStatus.WorkPlanned; Version++; }
    public void Resolve(DateTimeOffset now) { Require(DefectStatus.WorkPlanned); Status = DefectStatus.Resolved; ResolvedAt = now; Version++; }
    public void Verify(bool accepted) { Require(DefectStatus.Resolved); Status = accepted ? DefectStatus.Verified : DefectStatus.Rejected; Version++; }
    public void Close() { Require(DefectStatus.Verified); Status = DefectStatus.Closed; Version++; }
    private void Require(DefectStatus status) { if (Status != status) throw new InvalidOperationException($"Defect must be {status}."); }
}

public sealed record CriticalDefectRaised(
    Guid EventId,
    Guid OrganizationId,
    Guid DefectId,
    Guid TargetId,
    DateTimeOffset OccurredAt,
    string CorrelationId) : IRailwayDomainEvent
{
    public string EventName => "railway.defect.critical-raised";
    public int SchemaVersion => 1;
    public Guid? CausationId => null;
}
