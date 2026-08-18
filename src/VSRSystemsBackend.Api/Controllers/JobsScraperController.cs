using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;

namespace VSRSystemsBackend.Api.Controllers;

[ApiController]
[Route("api/jobs-admin")]
public class JobsScraperController : ControllerBase
{
    private readonly IJobsScraperService _scraper;

    public JobsScraperController(IJobsScraperService scraper)
    {
        _scraper = scraper;
    }

    // ------------------------------------------------------------------
    // Seed + dashboard
    // ------------------------------------------------------------------

    [HttpPost("seed")]
    public async Task<IActionResult> Seed(CancellationToken cancellationToken)
    {
        await _scraper.SeedAsync(cancellationToken);
        return Ok(new { message = "Fixture companies and sources seeded" });
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<ScraperDashboardDto>> Dashboard(CancellationToken cancellationToken)
    {
        return Ok(await _scraper.GetDashboardAsync(cancellationToken));
    }

    // ------------------------------------------------------------------
    // Sources
    // ------------------------------------------------------------------

    [HttpGet("sources")]
    public async Task<ActionResult<List<JobSourceDto>>> GetSources(CancellationToken cancellationToken)
    {
        return Ok(await _scraper.GetSourcesAsync(cancellationToken));
    }

    [HttpGet("sources/{id}")]
    public async Task<ActionResult<JobSourceDto>> GetSource(string id, CancellationToken cancellationToken)
    {
        var source = await _scraper.GetSourceAsync(id, cancellationToken);
        if (source == null)
            return NotFound();
        return Ok(source);
    }

    [HttpPost("sources")]
    public async Task<IActionResult> CreateSource([FromBody] CreateJobSourceDto dto, CancellationToken cancellationToken)
    {
        var result = await _scraper.CreateSourceAsync(dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    [HttpPut("sources/{id}")]
    public async Task<IActionResult> UpdateSource(string id, [FromBody] UpdateJobSourceDto dto, CancellationToken cancellationToken)
    {
        var result = await _scraper.UpdateSourceAsync(id, dto, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    [HttpPatch("sources/{id}/enabled/{enabled:bool}")]
    public async Task<IActionResult> SetSourceEnabled(string id, bool enabled, CancellationToken cancellationToken)
    {
        var result = await _scraper.SetSourceEnabledAsync(id, enabled, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    [HttpGet("sources/{id}/config")]
    public async Task<ActionResult<JobSourceConfigDto>> GetSourceConfig(string id, CancellationToken cancellationToken)
    {
        var config = await _scraper.GetSourceConfigAsync(id, cancellationToken);
        if (config == null)
            return NotFound();
        return Ok(config);
    }

    [HttpGet("sources/due")]
    public async Task<ActionResult<List<JobSourceDto>>> GetDueSources(CancellationToken cancellationToken)
    {
        return Ok(await _scraper.GetDueSourcesAsync(cancellationToken));
    }

    // ------------------------------------------------------------------
    // Runs
    // ------------------------------------------------------------------

    [HttpGet("runs")]
    public async Task<ActionResult<List<ScrapeRunDto>>> GetRuns([FromQuery] string? sourceId, [FromQuery] int limit = 50, CancellationToken cancellationToken = default)
    {
        return Ok(await _scraper.GetRunsAsync(sourceId, limit, cancellationToken));
    }

    [HttpGet("runs/{runId}")]
    public async Task<ActionResult<ScrapeRunDto>> GetRun(string runId, CancellationToken cancellationToken)
    {
        var run = await _scraper.GetRunAsync(runId, cancellationToken);
        if (run == null)
            return NotFound();
        return Ok(run);
    }

    [HttpGet("runs/{runId}/logs")]
    public async Task<ActionResult<List<ScrapeLogDto>>> GetRunLogs(string runId, CancellationToken cancellationToken)
    {
        return Ok(await _scraper.GetRunLogsAsync(runId, cancellationToken));
    }

    [HttpPost("sources/{id}/run")]
    public async Task<ActionResult<ScrapeRunDto>> RunSource(string id, [FromQuery] string triggeredBy = "Manual", CancellationToken cancellationToken = default)
    {
        var run = await _scraper.RunSourceAsync(id, triggeredBy, cancellationToken);
        return Ok(run);
    }

    // ------------------------------------------------------------------
    // Raw jobs, duplicates, errors
    // ------------------------------------------------------------------

    [HttpGet("raw")]
    public async Task<ActionResult<List<RawExternalJobDto>>> GetRawJobs([FromQuery] string? sourceId, [FromQuery] string? status, [FromQuery] int limit = 100, CancellationToken cancellationToken = default)
    {
        return Ok(await _scraper.GetRawJobsAsync(sourceId, status, limit, cancellationToken));
    }

    [HttpGet("duplicates")]
    public async Task<ActionResult<List<DuplicateCandidateDto>>> GetDuplicates([FromQuery] int limit = 50, CancellationToken cancellationToken = default)
    {
        return Ok(await _scraper.GetDuplicatesAsync(limit, cancellationToken));
    }

    [HttpPost("duplicates/{id}/resolve")]
    public async Task<IActionResult> ResolveDuplicate(string id, [FromQuery] string action = "keep-a", [FromQuery] string by = "admin", CancellationToken cancellationToken = default)
    {
        var dup = await _scraper.ResolveDuplicateAsync(id, action, by, cancellationToken);
        if (dup == null)
            return NotFound();
        return Ok(dup);
    }

    [HttpGet("errors")]
    public async Task<ActionResult<List<IngestionErrorDto>>> GetErrors([FromQuery] string? sourceId, [FromQuery] int limit = 50, CancellationToken cancellationToken = default)
    {
        return Ok(await _scraper.GetErrorsAsync(sourceId, limit, cancellationToken));
    }

    [HttpPost("raw/{rawId}/reprocess")]
    public async Task<IActionResult> ReprocessRaw(string rawId, CancellationToken cancellationToken)
    {
        var job = await _scraper.ReprocessRawJobAsync(rawId, cancellationToken);
        if (job == null)
            return NotFound();
        return Ok(job);
    }

    // ------------------------------------------------------------------
    // SSRF guard (used by admin before registering external feeds)
    // ------------------------------------------------------------------

    [HttpPost("validate-url")]
    public async Task<IActionResult> ValidateUrl([FromBody] ValidateUrlRequest request)
    {
        var (ok, error) = await _scraper.ValidateUrlAsync(request.Url);
        return ok ? Ok(new { ok = true }) : BadRequest(new { ok = false, error });
    }
}

public class ValidateUrlRequest
{
    public string Url { get; set; } = string.Empty;
}