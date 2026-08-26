using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Storage;
using VSRSystemsBackend.Api.Platform.Storage;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests.Modules.Railway;

public sealed class RailwayInfrastructureTests
{
    [Fact]
    public async Task Evidence_initiation_rejects_an_owner_from_another_organization()
    {
        var divisionId = Guid.NewGuid();
        var scope = Scope(Guid.NewGuid(), divisionId);
        var options = new DbContextOptionsBuilder<RailwayDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        await using var dbContext = new RailwayDbContext(options, new FixedScopeAccessor(scope));
        var foreignStation = new RailwayStation(Guid.NewGuid(), Guid.NewGuid(), divisionId, "FOREIGN", "Foreign station");
        dbContext.Stations.Add(foreignStation);
        await dbContext.SaveChangesAsync();
        var storage = new FakeStorage();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            new RailwayEvidenceService(dbContext, storage).InitiateAsync(
                scope,
                new InitiateRailwayEvidenceRequest(divisionId, foreignStation.Id, "inspection", "image/jpeg", 100, new string('a', 64)),
                CancellationToken.None));
        Assert.Equal(0, storage.UploadRequests);
    }

    [Fact]
    public void Scanner_unavailability_keeps_evidence_quarantined()
    {
        var evidence = new RailwayEvidence(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "inspection",
            "uploads", "railway/path", "image/jpeg", 100, new string('a', 64));
        evidence.FinalizeUpload(new string('a', 64), DateTimeOffset.UtcNow);

        evidence.RecordScan(MalwareScanVerdict.Unavailable, "offline", DateTimeOffset.UtcNow);

        Assert.Equal(RailwayEvidenceScanStatus.Quarantined, evidence.ScanStatus);
    }

    [Fact]
    public void Evidence_rejects_a_checksum_mismatch()
    {
        var evidence = new RailwayEvidence(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "inspection",
            "uploads", "railway/path", "image/jpeg", 100, new string('a', 64));

        Assert.Throws<InvalidOperationException>(() => evidence.FinalizeUpload(new string('b', 64), DateTimeOffset.UtcNow));
        Assert.Equal(RailwayEvidenceScanStatus.PendingUpload, evidence.ScanStatus);
    }

    private static RailwayScope Scope(Guid organizationId, Guid divisionId) => new(
        Guid.NewGuid(), organizationId, new HashSet<Guid> { divisionId },
        new HashSet<string> { "railway.evidence.create" });

    private sealed class FixedScopeAccessor(RailwayScope scope) : IRailwayScopeAccessor
    {
        public RailwayScope GetRequiredScope() => scope;
    }

    private sealed class FakeStorage : IPrivateFileStorage
    {
        public int UploadRequests { get; private set; }
        public Task<SignedUploadResponse> CreateSignedUploadAsync(SignedUploadRequest request, CancellationToken cancellationToken)
        {
            UploadRequests++;
            return Task.FromResult(new SignedUploadResponse(request.Bucket, request.Path, request.ContentType, "https://local.test/upload", 60));
        }
        public Task VerifyObjectExistsAsync(StorageObjectRequest request, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task<Stream> OpenReadAsync(StorageObjectRequest request, CancellationToken cancellationToken) =>
            Task.FromResult<Stream>(new MemoryStream());
    }
}
