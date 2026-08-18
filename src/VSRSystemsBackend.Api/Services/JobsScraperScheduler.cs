using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Api.Services;

/// <summary>
/// Runs due job sources every 30 seconds. Seeds fixture sources once at startup,
/// then polls for sources whose interval has elapsed and kicks off a scrape run
/// for each one (sequentially, to avoid overlapping DB writes).
/// </summary>
public class JobsScraperScheduler : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JobsScraperScheduler> _logger;
    private bool _seeded;

    public JobsScraperScheduler(IServiceScopeFactory scopeFactory, ILogger<JobsScraperScheduler> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Jobs scraper scheduler started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var scraper = scope.ServiceProvider.GetRequiredService<IJobsScraperService>();

                if (!_seeded)
                {
                    await scraper.SeedAsync(stoppingToken);
                    _seeded = true;
                    _logger.LogInformation("Seeded job sources and fixture companies");
                }

                var due = await scraper.GetDueSourcesAsync(stoppingToken);
                if (due.Count > 0)
                    _logger.LogInformation("Found {Count} due job sources", due.Count);

                foreach (var source in due)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;
                    var run = await scraper.RunSourceAsync(source.Id, "Scheduler", stoppingToken);
                    _logger.LogInformation("Source {Name}: {Status} (discovered={Discovered} created={Created} updated={Updated})",
                        source.Name, run.Status, run.JobsDiscovered, run.JobsCreated, run.JobsUpdated);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Jobs scraper scheduler tick failed");
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("Jobs scraper scheduler stopped");
    }
}