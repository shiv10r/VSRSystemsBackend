using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;

public enum InspectionTemplateStatus { Draft, Published }

public sealed class InspectionTemplate : RailwayEntity
{
    private readonly List<InspectionTemplateItem> items = [];
    private InspectionTemplate() { }

    public InspectionTemplate(Guid id, Guid organizationId, Guid divisionId, string name)
        : base(id, organizationId, divisionId)
    {
        Name = Required(name, nameof(name));
    }

    public string Name { get; private set; } = string.Empty;
    public int TemplateVersion { get; private set; } = 1;
    public InspectionTemplateStatus Status { get; private set; }
    public IReadOnlyCollection<InspectionTemplateItem> Items => items;

    public void Rename(string name)
    {
        RequireDraft();
        Name = Required(name, nameof(name));
        Version++;
    }

    public void AddItem(string itemId, string label, bool required, bool evidenceRequired, double? minimum = null, double? maximum = null)
    {
        RequireDraft();
        if (items.Any(item => item.ItemId == itemId)) throw new InvalidOperationException("Checklist item IDs must be unique.");
        items.Add(new InspectionTemplateItem(Guid.NewGuid(), itemId, label, required, evidenceRequired, minimum, maximum));
        Version++;
    }

    public void Publish()
    {
        RequireDraft();
        if (items.Count == 0) throw new InvalidOperationException("A template requires at least one checklist item.");
        Status = InspectionTemplateStatus.Published;
        Version++;
    }

    private void RequireDraft()
    {
        if (Status != InspectionTemplateStatus.Draft) throw new InvalidOperationException("Published template versions are immutable.");
    }

    private static string Required(string value, string name) =>
        !string.IsNullOrWhiteSpace(value) ? value.Trim() : throw new ArgumentException("A value is required.", name);
}

public sealed class InspectionTemplateItem
{
    private InspectionTemplateItem() { }
    internal InspectionTemplateItem(Guid id, string itemId, string label, bool required, bool evidenceRequired, double? minimum, double? maximum)
    {
        Id = id;
        ItemId = itemId;
        Label = label;
        Required = required;
        EvidenceRequired = evidenceRequired;
        Minimum = minimum;
        Maximum = maximum;
    }
    public Guid Id { get; private set; }
    public string ItemId { get; private set; } = string.Empty;
    public string Label { get; private set; } = string.Empty;
    public bool Required { get; private set; }
    public bool EvidenceRequired { get; private set; }
    public double? Minimum { get; private set; }
    public double? Maximum { get; private set; }
}
