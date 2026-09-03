using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;

public sealed class InspectionRun : RailwayEntity
{
    private readonly List<InspectionAnswer> answers = [];
    private readonly List<InspectionRunRequirement> requirements = [];
    private readonly List<IRailwayDomainEvent> domainEvents = [];
    private InspectionRun() { }

    public InspectionRun(
        Guid id,
        Guid organizationId,
        Guid divisionId,
        Guid assignmentId,
        Guid templateId,
        int templateVersion,
        Guid targetId,
        Guid assignedInspectorId,
        IEnumerable<InspectionTemplateItem> templateItems,
        DateTimeOffset startedAt,
        Guid? amendsInspectionRunId = null)
        : base(id, organizationId, divisionId)
    {
        AssignmentId = assignmentId;
        TemplateId = templateId;
        TemplateVersion = templateVersion;
        TargetId = targetId;
        AssignedInspectorId = assignedInspectorId;
        StartedAt = startedAt;
        AmendsInspectionRunId = amendsInspectionRunId;
        requirements.AddRange(templateItems.Select(item => new InspectionRunRequirement(
            Guid.NewGuid(), item.ItemId, item.Required, item.EvidenceRequired, item.Minimum, item.Maximum)));
    }

    public Guid AssignmentId { get; private set; }
    public Guid TemplateId { get; private set; }
    public int TemplateVersion { get; private set; }
    public Guid TargetId { get; private set; }
    public Guid AssignedInspectorId { get; private set; }
    public InspectionRunStatus Status { get; private set; } = InspectionRunStatus.Draft;
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? SubmittedAt { get; private set; }
    public Guid? ReviewedBy { get; private set; }
    public string? ReviewReason { get; private set; }
    public Guid? AmendsInspectionRunId { get; private set; }
    public IReadOnlyCollection<InspectionAnswer> Answers => answers;
    public IReadOnlyCollection<InspectionRunRequirement> Requirements => requirements;
    public IReadOnlyCollection<IRailwayDomainEvent> DomainEvents => domainEvents;

    public void Answer(string itemId, string response, double? measurement, IReadOnlyList<Guid> evidenceIds)
    {
        RequireDraft();
        var requirement = requirements.SingleOrDefault(item => item.ItemId == itemId)
            ?? throw new ArgumentException("Checklist item does not belong to the pinned template version.", nameof(itemId));
        var answer = answers.SingleOrDefault(item => item.ItemId == itemId);
        if (answer is null)
            answers.Add(new InspectionAnswer(Guid.NewGuid(), itemId, response, measurement, evidenceIds));
        else
            answer.Replace(response, measurement, evidenceIds);
        if (measurement.HasValue && ((requirement.Minimum.HasValue && measurement < requirement.Minimum) ||
            (requirement.Maximum.HasValue && measurement > requirement.Maximum)))
            domainEvents.Add(new RailwayDomainEvent(Guid.NewGuid(), "railway.inspection.measurement-limit-exceeded", 1, OrganizationId, DateTimeOffset.UtcNow, Id.ToString()));
        Version++;
    }

    public void Submit(Guid actorId, DateTimeOffset now)
    {
        RequireDraft();
        if (actorId != AssignedInspectorId) throw new UnauthorizedAccessException("Only the assigned inspector may submit this run.");
        foreach (var requirement in requirements.Where(item => item.Required))
        {
            var answer = answers.SingleOrDefault(item => item.ItemId == requirement.ItemId)
                ?? throw new InspectionValidationException($"Required item {requirement.ItemId} is unanswered.");
            if (requirement.EvidenceRequired && answer.EvidenceIds.Count == 0)
                throw new InspectionValidationException($"Required evidence is missing for {requirement.ItemId}.");
        }
        Status = InspectionRunStatus.Submitted;
        SubmittedAt = now;
        Version++;
    }

    public void Review(bool accepted, Guid reviewerId, DateTimeOffset now, string? reason)
    {
        if (Status != InspectionRunStatus.Submitted) throw new InvalidOperationException("Only submitted inspections can be reviewed.");
        if (!accepted && string.IsNullOrWhiteSpace(reason)) throw new InspectionValidationException("A rejection reason is required.");
        Status = accepted ? InspectionRunStatus.Accepted : InspectionRunStatus.Rejected;
        ReviewedBy = reviewerId;
        ReviewReason = reason;
        Version++;
    }

    public InspectionRun CreateAmendment(Guid actorId, DateTimeOffset now)
    {
        if (Status is not (InspectionRunStatus.Accepted or InspectionRunStatus.Rejected))
            throw new InvalidOperationException("Only reviewed inspections can be amended.");
        Status = InspectionRunStatus.Amended;
        Version++;
        var amendment = new InspectionRun(
            Guid.NewGuid(), OrganizationId, DivisionId!.Value, AssignmentId, TemplateId, TemplateVersion,
            TargetId, actorId, [], now, Id);
        amendment.requirements.AddRange(requirements.Select(item => item.Copy()));
        return amendment;
    }

    private void RequireDraft()
    {
        if (Status != InspectionRunStatus.Draft) throw new InvalidOperationException("Reviewed or submitted inspection answers are immutable.");
    }
}

public sealed class InspectionRunRequirement
{
    private InspectionRunRequirement() { }
    internal InspectionRunRequirement(Guid id, string itemId, bool required, bool evidenceRequired, double? minimum, double? maximum)
    { Id = id; ItemId = itemId; Required = required; EvidenceRequired = evidenceRequired; Minimum = minimum; Maximum = maximum; }
    public Guid Id { get; private set; }
    public string ItemId { get; private set; } = string.Empty;
    public bool Required { get; private set; }
    public bool EvidenceRequired { get; private set; }
    public double? Minimum { get; private set; }
    public double? Maximum { get; private set; }
    internal InspectionRunRequirement Copy() => new(Guid.NewGuid(), ItemId, Required, EvidenceRequired, Minimum, Maximum);
}

public sealed class InspectionAnswer
{
    private InspectionAnswer() { }
    internal InspectionAnswer(Guid id, string itemId, string response, double? measurement, IReadOnlyList<Guid> evidenceIds)
    { Id = id; ItemId = itemId; Replace(response, measurement, evidenceIds); }
    public Guid Id { get; private set; }
    public string ItemId { get; private set; } = string.Empty;
    public string Response { get; private set; } = string.Empty;
    public double? Measurement { get; private set; }
    public string EvidenceIdList { get; private set; } = string.Empty;
    public IReadOnlyList<Guid> EvidenceIds => EvidenceIdList.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList();
    internal void Replace(string response, double? measurement, IReadOnlyList<Guid> evidenceIds)
    { Response = response; Measurement = measurement; EvidenceIdList = string.Join(',', evidenceIds); }
}

public sealed class InspectionValidationException(string message) : Exception(message);
