using VSRSystemsBackend.Api.Domain.CrowdOperations;

namespace VSRSystemsBackend.Api.Application.CrowdOperations
{
    /// <summary>
    /// One stable adapter interface for approved gate, CCTV-analytics,
    /// Wi-Fi aggregate, and IoT providers. Implementations normalize
    /// provider payloads into privacy-safe aggregate observations.
    /// </summary>
    public interface ICrowdObservationAdapter
    {
        string AdapterType { get; }

        ValueTask<IReadOnlyList<NormalizedCrowdObservation>> NormalizeAsync(
            CrowdAdapterEnvelope envelope,
            CancellationToken cancellationToken);
    }

    public sealed record CrowdAdapterEnvelope(
        Guid SourceId,
        Guid OrganizationId,
        Guid DivisionId,
        DateTimeOffset Timestamp,
        string Nonce,
        string Signature,
        byte[] Body);
}