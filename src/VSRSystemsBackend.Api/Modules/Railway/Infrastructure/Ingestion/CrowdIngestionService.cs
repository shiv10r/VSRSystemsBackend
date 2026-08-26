using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;

public sealed record CrowdIngestionResult(bool Accepted, int AcceptedCount, int DuplicateCount, string? FailureCode = null);

public sealed class CrowdIngestionService(
    RailwayDbContext dbContext,
    CrowdAdapterAuthenticator authenticator,
    IEnumerable<ICrowdObservationAdapter> adapters)
{
    public async Task<CrowdIngestionResult> IngestAsync(Guid sourceId, DateTimeOffset timestamp, string nonce,
        string signature, byte[] body, CancellationToken cancellationToken)
    {
        var source = await dbContext.CrowdSources.IgnoreQueryFilters().SingleOrDefaultAsync(item => item.Id == sourceId, cancellationToken);
        if (source is null) return new(false, 0, 0, "source_not_found");

        var authentication = authenticator.Authenticate(source, timestamp, nonce, signature, body, DateTimeOffset.UtcNow);
        if (!authentication.Succeeded)
        {
            await QuarantineAsync(source, authentication.FailureCode!, body, cancellationToken);
            return new(false, 0, 0, authentication.FailureCode);
        }
        if (await dbContext.CrowdIngestionNonces.AnyAsync(item => item.SourceId == sourceId && item.Nonce == nonce, cancellationToken))
            return new(false, 0, 0, "replayed_nonce");

        var adapter = adapters.SingleOrDefault(item => item.AdapterType == source.AdapterType);
        if (adapter is null) return new(false, 0, 0, "adapter_not_found");
        IReadOnlyList<NormalizedCrowdObservation> normalized;
        try
        {
            normalized = await adapter.NormalizeAsync(new CrowdAdapterEnvelope(source.Id, source.OrganizationId,
                source.DivisionId ?? throw new InvalidOperationException("Crowd source requires a division."), timestamp, nonce, signature, body), cancellationToken);
        }
        catch (Exception exception) when (exception is JsonException or InvalidDataException or ArgumentException)
        {
            await QuarantineAsync(source, "malformed_payload", body, cancellationToken);
            return new(false, 0, 0, "malformed_payload");
        }
        if (normalized.Count == 0)
        {
            await QuarantineAsync(source, "empty_batch", body, cancellationToken);
            return new(false, 0, 0, "empty_batch");
        }

        var accepted = 0;
        var duplicates = 0;
        foreach (var value in normalized)
        {
            if (value.SourceId != source.Id || value.OrganizationId != source.OrganizationId || value.DivisionId != source.DivisionId ||
                value.WindowEnd > DateTimeOffset.UtcNow.AddMinutes(1))
            {
                await QuarantineAsync(source, "invalid_observation_scope_or_time", body, cancellationToken);
                return new(false, 0, 0, "invalid_observation_scope_or_time");
            }
            if (await dbContext.CrowdObservations.IgnoreQueryFilters().AnyAsync(item =>
                    item.OrganizationId == source.OrganizationId && item.SourceId == source.Id && item.SourceEventId == value.SourceEventId, cancellationToken))
            { duplicates++; continue; }
            dbContext.CrowdObservations.Add(new CrowdObservation(Guid.NewGuid(), value));
            accepted++;
        }
        dbContext.CrowdIngestionNonces.Add(new CrowdIngestionNonce(source.Id, nonce, DateTimeOffset.UtcNow));
        source.RecordObservation(normalized.Max(item => item.WindowEnd));
        await dbContext.SaveChangesAsync(cancellationToken);
        return new(true, accepted, duplicates);
    }

    private async Task QuarantineAsync(CrowdSource source, string reason, byte[] body, CancellationToken cancellationToken)
    {
        dbContext.CrowdQuarantine.Add(new CrowdQuarantineRecord(Guid.NewGuid(), source.OrganizationId, source.DivisionId,
            source.Id, reason, Convert.ToHexString(SHA256.HashData(body)).ToLowerInvariant(), DateTimeOffset.UtcNow));
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
