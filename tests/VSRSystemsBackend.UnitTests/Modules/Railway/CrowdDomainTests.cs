using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using Xunit;

namespace VSRSystemsBackend.UnitTests.Modules.Railway;

public sealed class CrowdDomainTests
{
    [Fact]
    public void Observation_contract_contains_no_person_or_device_identifier()
    {
        var names = typeof(NormalizedCrowdObservation).GetProperties().Select(property => property.Name).ToHashSet();

        Assert.DoesNotContain("PersonId", names);
        Assert.DoesNotContain("DeviceId", names);
        Assert.DoesNotContain("Face", names);
        Assert.DoesNotContain("Video", names);
    }

    [Fact]
    public void Stale_low_confidence_data_is_not_normal()
    {
        var now = DateTimeOffset.UtcNow;
        var observation = CreateObservation(now.AddMinutes(-12), now.AddMinutes(-10), .3m);

        Assert.Equal(CrowdDataQuality.Stale, CrowdRiskPolicy.Quality(observation, now));
    }

    [Theory]
    [InlineData(19, CrowdRiskLevel.Normal)]
    [InlineData(20, CrowdRiskLevel.Warning)]
    [InlineData(40, CrowdRiskLevel.Critical)]
    public void Thresholds_are_deterministic(int count, CrowdRiskLevel expected) =>
        Assert.Equal(expected, CrowdRiskPolicy.Calculate(count, 20, 40));

    [Fact]
    public void Threshold_override_preserves_original_values_and_reason()
    {
        var actor = Guid.NewGuid();
        var policy = new CrowdThresholdPolicy(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 20, 40, DateTimeOffset.UtcNow, actor);

        policy.Override(15, 30, "Event-day capacity", actor);

        Assert.Equal(20, policy.OriginalWarningThreshold);
        Assert.Equal(40, policy.OriginalCriticalThreshold);
        Assert.Equal(15, policy.WarningThreshold);
        Assert.Equal(30, policy.CriticalThreshold);
        Assert.Equal("Event-day capacity", policy.OverrideReason);
    }

    [Fact]
    public void Alert_requires_acknowledgement_before_closure()
    {
        var alert = new CrowdAlert(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), CrowdRiskLevel.Critical, DateTimeOffset.UtcNow);

        Assert.Throws<InvalidOperationException>(() => alert.Close(DateTimeOffset.UtcNow));
        alert.Acknowledge(Guid.NewGuid(), DateTimeOffset.UtcNow);
        alert.Close(DateTimeOffset.UtcNow);

        Assert.False(alert.IsOpen);
    }

    private static NormalizedCrowdObservation CreateObservation(DateTimeOffset start, DateTimeOffset end, decimal confidence) =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "event-1", start, end, 10, null, null, confidence, new HashSet<string>());
}
