using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.CrowdOperations;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Ingestion;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests.Modules.Railway;

public sealed class CrowdIngestionSecurityTests
{
    [Fact]
    public async Task Duplicate_event_is_idempotent_and_nonce_replay_is_rejected()
    {
        var fixture = await CreateFixtureAsync();
        await using var dbContext = fixture.Context;
        var service = CreateService(dbContext);

        var first = await service.IngestAsync(fixture.Source.Id, fixture.Now, "nonce-1", Sign(fixture.Secret, fixture.Now, "nonce-1", fixture.Body), fixture.Body, CancellationToken.None);
        var duplicate = await service.IngestAsync(fixture.Source.Id, fixture.Now, "nonce-2", Sign(fixture.Secret, fixture.Now, "nonce-2", fixture.Body), fixture.Body, CancellationToken.None);
        var replay = await service.IngestAsync(fixture.Source.Id, fixture.Now, "nonce-1", Sign(fixture.Secret, fixture.Now, "nonce-1", fixture.Body), fixture.Body, CancellationToken.None);

        Assert.True(first.Accepted);
        Assert.Equal(1, first.AcceptedCount);
        Assert.True(duplicate.Accepted);
        Assert.Equal(1, duplicate.DuplicateCount);
        Assert.Equal("replayed_nonce", replay.FailureCode);
        Assert.Equal(1, await dbContext.CrowdObservations.IgnoreQueryFilters().CountAsync());
    }

    [Fact]
    public async Task Invalid_signature_quarantine_stores_hash_not_raw_payload()
    {
        var fixture = await CreateFixtureAsync();
        await using var dbContext = fixture.Context;

        var result = await CreateService(dbContext).IngestAsync(fixture.Source.Id, fixture.Now, "nonce-1",
            Sign("wrong-secret", fixture.Now, "nonce-1", fixture.Body), fixture.Body, CancellationToken.None);

        Assert.Equal("invalid_signature", result.FailureCode);
        var quarantine = await dbContext.CrowdQuarantine.IgnoreQueryFilters().SingleAsync();
        Assert.Equal(64, quarantine.PayloadHash.Length);
        Assert.DoesNotContain("observations", quarantine.PayloadHash, StringComparison.OrdinalIgnoreCase);
    }

    private static CrowdIngestionService CreateService(RailwayDbContext context) =>
        new(context, new CrowdAdapterAuthenticator(new IdentityProtector()), new ICrowdObservationAdapter[] { new ManualCrowdAdapter() });

    private static async Task<Fixture> CreateFixtureAsync()
    {
        var organizationId = Guid.NewGuid(); var divisionId = Guid.NewGuid(); var sourceId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow; const string secret = "test-secret";
        var source = new CrowdSource(sourceId, organizationId, divisionId, Guid.NewGuid(), Guid.NewGuid(), "Gate", "manual-json", secret);
        var body = JsonSerializer.SerializeToUtf8Bytes(new ManualCrowdBatch(new[]
        {
            new ManualCrowdObservation(source.StationId, source.StationZoneId, "event-1", now.AddMinutes(-2), now.AddMinutes(-1), 25, 5, 2, .9m, Array.Empty<string>())
        }), new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var options = new DbContextOptionsBuilder<RailwayDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new RailwayDbContext(options, new FixedScopeAccessor(new RailwayScope(Guid.NewGuid(), organizationId,
            new HashSet<Guid> { divisionId }, new HashSet<string>())));
        context.CrowdSources.Add(source); await context.SaveChangesAsync();
        return new Fixture(context, source, now, secret, body);
    }

    private static string Sign(string secret, DateTimeOffset timestamp, string nonce, byte[] body)
    {
        var digest = Convert.ToHexString(SHA256.HashData(body)).ToLowerInvariant();
        var message = Encoding.UTF8.GetBytes($"{timestamp.ToUnixTimeSeconds()}.{nonce}.{digest}");
        return Convert.ToBase64String(HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), message));
    }

    private sealed record Fixture(RailwayDbContext Context, CrowdSource Source, DateTimeOffset Now, string Secret, byte[] Body);
    private sealed class FixedScopeAccessor(RailwayScope scope) : IRailwayScopeAccessor { public RailwayScope GetRequiredScope() => scope; }
    private sealed class IdentityProtector : ICrowdSourceSecretProtector
    {
        public string Protect(string secret) => secret;
        public string Unprotect(string ciphertext) => ciphertext;
    }
}
