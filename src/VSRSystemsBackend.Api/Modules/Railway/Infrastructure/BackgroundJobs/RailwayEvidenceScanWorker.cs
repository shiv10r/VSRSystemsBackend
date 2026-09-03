using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Platform.Storage;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.BackgroundJobs;

public sealed class RailwayEvidenceScanWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<RailwayEvidenceScanWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunOnceAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    public async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RailwayDbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IPrivateFileStorage>();
        var scanner = scope.ServiceProvider.GetRequiredService<IFileMalwareScanner>();
        var evidence = await dbContext.Evidence.IgnoreQueryFilters()
            .Where(item => item.ScanStatus == RailwayEvidenceScanStatus.Quarantined)
            .OrderBy(item => item.FinalizedAt)
            .Take(10)
            .ToListAsync(cancellationToken);
        foreach (var item in evidence)
        {
            try
            {
                await using var content = await storage.OpenReadAsync(new StorageObjectRequest(item.Bucket, item.Path), cancellationToken);
                var result = await scanner.ScanAsync(content, cancellationToken);
                item.RecordScan(result.Verdict, result.Detail, DateTimeOffset.UtcNow);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Evidence {EvidenceId} remains quarantined after scan failure", item.Id);
            }
        }
    }
}
