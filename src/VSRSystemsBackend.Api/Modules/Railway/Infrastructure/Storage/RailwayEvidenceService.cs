using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Platform.Storage;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Storage;

public sealed record InitiateRailwayEvidenceRequest(
    Guid DivisionId, Guid OwnerRecordId, string Category, string ContentType, long SizeBytes, string Sha256);
public sealed record RailwayEvidenceUpload(Guid EvidenceId, string SignedUrl, int ExpiresInSeconds);

public interface IRailwayEvidenceService
{
    Task<RailwayEvidenceUpload> InitiateAsync(RailwayScope scope, InitiateRailwayEvidenceRequest request, CancellationToken cancellationToken);
    Task FinalizeAsync(RailwayScope scope, Guid evidenceId, string sha256, CancellationToken cancellationToken);
}

public sealed class RailwayEvidenceService(
    RailwayDbContext dbContext,
    IPrivateFileStorage storage) : IRailwayEvidenceService
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp", "application/pdf",
    };

    public async Task<RailwayEvidenceUpload> InitiateAsync(
        RailwayScope scope,
        InitiateRailwayEvidenceRequest request,
        CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.evidence.create");
        scope.RequireDivision(request.DivisionId);
        if (request.SizeBytes is < 1 or > 25_000_000 || !AllowedContentTypes.Contains(request.ContentType))
            throw new ArgumentException("Evidence size or content type is not allowed.");
        var ownerExists = await dbContext.Set<RailwayMasterRecord>().AnyAsync(record =>
            record.Id == request.OwnerRecordId && record.DivisionId == request.DivisionId, cancellationToken);
        if (!ownerExists) throw new UnauthorizedAccessException();

        var evidenceId = Guid.NewGuid();
        const string bucket = "uploads";
        var path = $"railway/{scope.OrganizationId:N}/{request.OwnerRecordId:N}/{evidenceId:N}";
        var evidence = new RailwayEvidence(
            evidenceId, scope.OrganizationId, request.DivisionId, request.OwnerRecordId,
            request.Category, bucket, path, request.ContentType, request.SizeBytes, request.Sha256);
        var upload = await storage.CreateSignedUploadAsync(
            new SignedUploadRequest(bucket, path, request.ContentType, BillingConfirmed: true), cancellationToken);
        dbContext.Evidence.Add(evidence);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new RailwayEvidenceUpload(evidenceId, upload.SignedUrl, upload.ExpiresInSeconds);
    }

    public async Task FinalizeAsync(RailwayScope scope, Guid evidenceId, string sha256, CancellationToken cancellationToken)
    {
        scope.RequirePermission("railway.evidence.create");
        var evidence = await dbContext.Evidence.SingleOrDefaultAsync(item => item.Id == evidenceId, cancellationToken)
            ?? throw new KeyNotFoundException();
        scope.RequireDivision(evidence.DivisionId!.Value);
        await storage.VerifyObjectExistsAsync(new StorageObjectRequest(evidence.Bucket, evidence.Path), cancellationToken);
        evidence.FinalizeUpload(sha256, DateTimeOffset.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
