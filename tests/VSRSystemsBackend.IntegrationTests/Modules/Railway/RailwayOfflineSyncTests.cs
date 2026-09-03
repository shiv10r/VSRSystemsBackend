using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests.Modules.Railway;

public sealed class RailwayOfflineSyncTests
{
    [Fact]
    public void Registry_rejects_duplicate_command_types()
    {
        Assert.Throws<InvalidOperationException>(() =>
            new RailwayOfflineCommandRegistry([new AcceptingHandler(), new AcceptingHandler()]));
    }

    [Fact]
    public async Task Mixed_batch_continues_and_duplicate_retry_returns_original_version()
    {
        var organizationId = Guid.NewGuid();
        var scope = new RailwayScope(Guid.NewGuid(), organizationId, new HashSet<Guid>(), new HashSet<string>());
        var options = new DbContextOptionsBuilder<RailwayDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var dbContext = new RailwayDbContext(options, new FixedScopeAccessor(scope));
        var acceptingHandler = new AcceptingHandler();
        var sync = new RailwayOfflineSyncHandler(
            dbContext,
            new RailwayOfflineCommandRegistry([acceptingHandler]));
        var accepted = Command("accept", "accepted-key", expectedVersion: 4);
        var unknown = Command("unknown", "unknown-key", expectedVersion: 0);

        var first = await sync.HandleAsync(scope, [accepted, unknown], CancellationToken.None);
        var retry = await sync.HandleAsync(scope, [accepted], CancellationToken.None);

        Assert.Equal(RailwayOfflineCommandStatus.Accepted, first[0].Status);
        Assert.Equal(5, first[0].AuthoritativeVersion);
        Assert.Equal(RailwayOfflineCommandStatus.Rejected, first[1].Status);
        Assert.Equal("unknown_command_type", first[1].Code);
        Assert.Equal(RailwayOfflineCommandStatus.Duplicate, retry[0].Status);
        Assert.Equal(5, retry[0].AuthoritativeVersion);
        Assert.Equal(1, acceptingHandler.InvocationCount);
        Assert.Equal(2, await dbContext.CommandReceipts.CountAsync());
    }

    private static RailwayOfflineCommandEnvelope Command(string type, string key, long expectedVersion) => new(
        Guid.NewGuid(), key, Guid.NewGuid(), expectedVersion, type,
        JsonDocument.Parse("{}").RootElement.Clone(), DateTimeOffset.UtcNow, []);

    private sealed class AcceptingHandler : IRailwayOfflineCommandHandler
    {
        public string CommandType => "accept";
        public int InvocationCount { get; private set; }

        public ValueTask<RailwayOfflineCommandResult> HandleAsync(
            RailwayScope scope,
            RailwayOfflineCommandEnvelope command,
            CancellationToken cancellationToken)
        {
            InvocationCount++;
            return ValueTask.FromResult(new RailwayOfflineCommandResult(
                command.CommandId,
                RailwayOfflineCommandStatus.Accepted,
                command.ExpectedVersion + 1));
        }
    }

    private sealed class FixedScopeAccessor(RailwayScope scope) : IRailwayScopeAccessor
    {
        public RailwayScope GetRequiredScope() => scope;
    }
}
