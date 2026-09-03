using System.Text.Json;
using VSRSystemsBackend.Api.Modules.Railway.Application.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;

public sealed class ManualCrowdAdapter : ICrowdObservationAdapter
{
    public string AdapterType => "manual-json";

    public ValueTask<IReadOnlyList<NormalizedCrowdObservation>> NormalizeAsync(CrowdAdapterEnvelope envelope, CancellationToken cancellationToken)
    {
        var batch = JsonSerializer.Deserialize<ManualCrowdBatch>(envelope.Body, new JsonSerializerOptions(JsonSerializerDefaults.Web))
                    ?? throw new InvalidDataException("The crowd batch is empty.");
        var observations = batch.Observations.Select(item => new NormalizedCrowdObservation(
            envelope.OrganizationId, envelope.DivisionId, item.StationId, item.StationZoneId, envelope.SourceId,
            item.SourceEventId, item.WindowStart, item.WindowEnd, item.Count, item.Inflow, item.Outflow,
            item.Confidence, item.QualityFlags?.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>())).ToArray();
        return ValueTask.FromResult<IReadOnlyList<NormalizedCrowdObservation>>(observations);
    }
}

public sealed record ManualCrowdBatch(IReadOnlyList<ManualCrowdObservation> Observations);
public sealed record ManualCrowdObservation(Guid StationId, Guid StationZoneId, string SourceEventId,
    DateTimeOffset WindowStart, DateTimeOffset WindowEnd, int Count, int? Inflow, int? Outflow,
    decimal Confidence, IReadOnlyList<string>? QualityFlags);
