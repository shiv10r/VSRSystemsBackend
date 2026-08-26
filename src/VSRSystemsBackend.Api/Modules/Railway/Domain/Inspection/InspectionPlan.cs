using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;

public sealed class InspectionPlan : RailwayEntity
{
    private InspectionPlan() { }
    public InspectionPlan(Guid id, Guid organizationId, Guid divisionId, Guid templateId, int templateVersion, Guid targetId, Guid inspectorId, string schedule, string timeZone, DateTimeOffset nextDueAt)
        : base(id, organizationId, divisionId)
    {
        TemplateId = templateId;
        TemplateVersion = templateVersion;
        TargetId = targetId;
        InspectorId = inspectorId;
        Schedule = schedule;
        TimeZone = timeZone;
        NextDueAt = nextDueAt;
    }
    public Guid TemplateId { get; private set; }
    public int TemplateVersion { get; private set; }
    public Guid TargetId { get; private set; }
    public Guid InspectorId { get; private set; }
    public string Schedule { get; private set; } = string.Empty;
    public string TimeZone { get; private set; } = "UTC";
    public bool Enabled { get; private set; } = true;
    public DateTimeOffset NextDueAt { get; private set; }
    public void Disable() { Enabled = false; Version++; }
    public void Advance(DateTimeOffset nextDueAt) { if (!Enabled) throw new InvalidOperationException("Disabled plans cannot advance."); NextDueAt = nextDueAt; Version++; }
}

public sealed class InspectionAssignment : RailwayEntity
{
    private InspectionAssignment() { }
    public InspectionAssignment(Guid id, Guid organizationId, Guid divisionId, Guid planId, Guid templateId, int templateVersion, Guid targetId, Guid inspectorId, DateTimeOffset dueAt, string occurrenceKey)
        : base(id, organizationId, divisionId)
    { PlanId = planId; TemplateId = templateId; TemplateVersion = templateVersion; TargetId = targetId; InspectorId = inspectorId; DueAt = dueAt; OccurrenceKey = occurrenceKey; }
    public Guid PlanId { get; private set; }
    public Guid TemplateId { get; private set; }
    public int TemplateVersion { get; private set; }
    public Guid TargetId { get; private set; }
    public Guid InspectorId { get; private set; }
    public DateTimeOffset DueAt { get; private set; }
    public string OccurrenceKey { get; private set; } = string.Empty;
}
