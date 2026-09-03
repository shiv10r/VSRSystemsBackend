using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;
using Xunit;

namespace VSRSystemsBackend.UnitTests.Modules.Railway;

public sealed class InspectionDomainTests
{
    [Fact]
    public void Published_template_version_is_immutable()
    {
        var template = Template(requiredEvidence: false);
        template.Publish();

        Assert.Throws<InvalidOperationException>(() => template.Rename("Changed"));
    }

    [Fact]
    public void Submit_rejects_missing_required_evidence()
    {
        var inspectorId = Guid.NewGuid();
        var template = Template(requiredEvidence: true);
        var run = Run(template, inspectorId);
        run.Answer("item-1", "Pass", null, []);

        Assert.Throws<InspectionValidationException>(() => run.Submit(inspectorId, DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Accepted_run_is_changed_only_by_amendment()
    {
        var inspectorId = Guid.NewGuid();
        var template = Template(requiredEvidence: false);
        var run = Run(template, inspectorId);
        run.Answer("item-1", "Pass", null, []);
        run.Submit(inspectorId, DateTimeOffset.UtcNow);
        run.Review(true, Guid.NewGuid(), DateTimeOffset.UtcNow, null);

        Assert.Throws<InvalidOperationException>(() => run.Answer("item-1", "Fail", null, []));
        var amendment = run.CreateAmendment(inspectorId, DateTimeOffset.UtcNow);

        Assert.Equal(run.Id, amendment.AmendsInspectionRunId);
        Assert.Equal(InspectionRunStatus.Amended, run.Status);
    }

    [Fact]
    public void Measurement_limit_creates_a_versioned_domain_event()
    {
        var template = new InspectionTemplate(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Track");
        template.AddItem("gauge", "Track gauge", true, false, 10, 20);
        template.Publish();
        var run = Run(template, Guid.NewGuid());

        run.Answer("gauge", "Measured", 25, []);

        Assert.Contains(run.DomainEvents, item => item.EventName == "railway.inspection.measurement-limit-exceeded");
    }

    private static InspectionTemplate Template(bool requiredEvidence)
    {
        var template = new InspectionTemplate(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Signals");
        template.AddItem("item-1", "Visual condition", true, requiredEvidence);
        return template;
    }

    private static InspectionRun Run(InspectionTemplate template, Guid inspectorId) => new(
        Guid.NewGuid(), template.OrganizationId, template.DivisionId!.Value, Guid.NewGuid(), template.Id,
        template.TemplateVersion, Guid.NewGuid(), inspectorId, template.Items, DateTimeOffset.UtcNow);
}
