using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

public sealed class MaintenancePlan : RailwayEntity
{
    private MaintenancePlan() { }
    public MaintenancePlan(Guid id, Guid organizationId, Guid divisionId, Guid targetId, string name, string recurrenceRule, int slaDays, DateTimeOffset nextDueAt)
        : base(id, organizationId, divisionId)
    { TargetId = targetId; Name = name; RecurrenceRule = recurrenceRule; SlaDays = slaDays; NextDueAt = nextDueAt; }
    public Guid TargetId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string RecurrenceRule { get; private set; } = string.Empty;
    public int SlaDays { get; private set; }
    public bool Enabled { get; private set; } = true;
    public DateTimeOffset NextDueAt { get; private set; }
    public void Advance(DateTimeOffset nextDueAt) { NextDueAt = nextDueAt; Version++; }
    public void Disable() { Enabled = false; Version++; }
}
