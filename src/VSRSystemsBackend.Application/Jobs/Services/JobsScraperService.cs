using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Jobs.DTOs;
using VSRSystemsBackend.Application.Jobs.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

/// <summary>
/// Jobs scraping pipeline. Ported from the LuxInfra reference implementation and
/// re-authored against EF Core repositories (postgres). Responsibilities:
/// seed fixture sources, run configured sources (fetch -> store raw -> normalize ->
/// validate -> dedupe -> upsert canonical Job -> close stale), source health tracking,
/// dashboard aggregation, and the scheduler contract.
/// </summary>
public class JobsScraperService : IJobsScraperService
{
    private readonly IJobSourceRepository _sources;
    private readonly IJobSourceConfigRepository _configs;
    private readonly IRawExternalJobRepository _rawJobs;
    private readonly IScrapeRunRepository _runs;
    private readonly IScrapeLogRepository _logs;
    private readonly IJobSourceMappingRepository _mappings;
    private readonly IDuplicateCandidateRepository _duplicates;
    private readonly IIngestionErrorRepository _errors;
    private readonly IJobRepository _jobs;
    private readonly ICompanyRepository _companies;
    private readonly IMapper _mapper;
    private readonly HttpClient _http;

    private const int MaxJobsPerFetch = 250;
    private const int MaxStaleClosePerRun = 25;
    private const int AutoPauseThreshold = 5;
    private const int HealthyConsecutiveFailures = 0;
    private const int WarningConsecutiveFailures = 2;

    public JobsScraperService(
        IJobSourceRepository sources,
        IJobSourceConfigRepository configs,
        IRawExternalJobRepository rawJobs,
        IScrapeRunRepository runs,
        IScrapeLogRepository logs,
        IJobSourceMappingRepository mappings,
        IDuplicateCandidateRepository duplicates,
        IIngestionErrorRepository errors,
        IJobRepository jobs,
        ICompanyRepository companies,
        IMapper mapper,
        HttpClient http)
    {
        _sources = sources;
        _configs = configs;
        _rawJobs = rawJobs;
        _runs = runs;
        _logs = logs;
        _mappings = mappings;
        _duplicates = duplicates;
        _errors = errors;
        _jobs = jobs;
        _companies = companies;
        _mapper = mapper;
        _http = http;
    }

    // ------------------------------------------------------------------
    // Seed
    // ------------------------------------------------------------------

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        foreach (var c in JobsScraperSeedData.Companies)
        {
            var existing = await _companies.GetBySlugAsync(c.Slug, cancellationToken);
            if (existing == null)
            {
                await _companies.AddAsync(new Company
                {
                    Id = Guid.NewGuid().ToString("N")[..20],
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.About,
                    Size = c.Size,
                    Industry = c.Industry,
                    Location = "India",
                }, cancellationToken);
            }
        }

        foreach (var s in JobsScraperSeedData.Sources)
        {
            var existing = await _sources.GetBySlugAsync(s.Slug, cancellationToken);
            if (existing != null)
                continue;

            var company = await _companies.GetBySlugAsync(
                SlugHelper.GenerateSlug(s.Name.Split(" (")[0]), cancellationToken);

            var source = new JobSource
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                Name = s.Name,
                Slug = s.Slug,
                CompanyId = company?.Id,
                SourceType = s.SourceType,
                FeedUrl = s.FeedUrl,
                AdapterKey = s.AdapterKey,
                IsEnabled = s.Enabled,
                IsAuthorized = s.Authorized,
                AuthorizationNotes = s.Notes,
                RequestIntervalMinutes = s.Interval,
                MaxRequestsPerMinute = 10,
                DefaultCountry = "India",
                DefaultCurrency = "INR",
                UserAgent = "VSR-JobsBot/1.0",
                HealthStatus = s.Health,
            };
            await _sources.AddAsync(source, cancellationToken);

            await _configs.AddAsync(new JobSourceConfig
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                JobSourceId = source.Id,
                ConfigJson = "{}",
                Version = 1,
                IsActive = true,
                CreatedBy = "system",
            }, cancellationToken);
        }
    }

    // ------------------------------------------------------------------
    // Source admin + queries
    // ------------------------------------------------------------------

    public async Task<List<JobSourceDto>> GetSourcesAsync(CancellationToken cancellationToken = default)
    {
        var all = await _sources.GetAllAsync(cancellationToken);
        return _mapper.Map<List<JobSourceDto>>(all);
    }

    public async Task<JobSourceDto?> GetSourceAsync(string id, CancellationToken cancellationToken = default)
    {
        var source = await _sources.GetByIdAsync(id, cancellationToken);
        return source == null ? null : _mapper.Map<JobSourceDto>(source);
    }

    public async Task<Result<JobSourceDto>> CreateSourceAsync(CreateJobSourceDto dto, CancellationToken cancellationToken = default)
    {
        var slug = SlugHelper.GenerateSlug(dto.Name);
        if (await _sources.ExistsAsync(s => s.Slug == slug, cancellationToken))
            return Result<JobSourceDto>.Failure("A source with that name already exists");

        var source = _mapper.Map<JobSource>(dto);
        source.Id = Guid.NewGuid().ToString("N")[..20];
        source.Slug = slug;
        source.HealthStatus = dto.IsEnabled ? JobSourceHealth.Healthy : JobSourceHealth.Paused;
        await _sources.AddAsync(source, cancellationToken);
        return Result<JobSourceDto>.Success(_mapper.Map<JobSourceDto>(source));
    }

    public async Task<Result<JobSourceDto>> UpdateSourceAsync(string id, UpdateJobSourceDto dto, CancellationToken cancellationToken = default)
    {
        var source = await _sources.GetByIdAsync(id, cancellationToken);
        if (source == null)
            return Result<JobSourceDto>.Failure("Source not found");

        if (!string.IsNullOrWhiteSpace(dto.Name)) source.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.SourceType)) source.SourceType = dto.SourceType;
        if (!string.IsNullOrWhiteSpace(dto.BaseUrl)) source.BaseUrl = dto.BaseUrl;
        if (!string.IsNullOrWhiteSpace(dto.FeedUrl)) source.FeedUrl = dto.FeedUrl;
        if (!string.IsNullOrWhiteSpace(dto.CareersUrl)) source.CareersUrl = dto.CareersUrl;
        if (!string.IsNullOrWhiteSpace(dto.AdapterKey)) source.AdapterKey = dto.AdapterKey;
        if (dto.CompanyId != null) source.CompanyId = dto.CompanyId;
        if (dto.IsEnabled.HasValue) source.IsEnabled = dto.IsEnabled.Value;
        if (dto.IsAuthorized.HasValue) source.IsAuthorized = dto.IsAuthorized.Value;
        if (dto.AuthorizationNotes != null) source.AuthorizationNotes = dto.AuthorizationNotes;
        if (dto.RequestIntervalMinutes.HasValue) source.RequestIntervalMinutes = dto.RequestIntervalMinutes.Value;
        if (dto.MaxRequestsPerMinute.HasValue) source.MaxRequestsPerMinute = dto.MaxRequestsPerMinute.Value;
        if (!string.IsNullOrWhiteSpace(dto.DefaultCountry)) source.DefaultCountry = dto.DefaultCountry;
        if (!string.IsNullOrWhiteSpace(dto.DefaultCurrency)) source.DefaultCurrency = dto.DefaultCurrency;
        if (dto.UserAgent != null) source.UserAgent = dto.UserAgent;

        await _sources.UpdateAsync(source, cancellationToken);
        return Result<JobSourceDto>.Success(_mapper.Map<JobSourceDto>(source));
    }

    public async Task<Result<JobSourceDto>> SetSourceEnabledAsync(string id, bool enabled, CancellationToken cancellationToken = default)
    {
        var source = await _sources.GetByIdAsync(id, cancellationToken);
        if (source == null)
            return Result<JobSourceDto>.Failure("Source not found");

        source.IsEnabled = enabled;
        if (enabled && source.HealthStatus == JobSourceHealth.Paused)
            source.HealthStatus = JobSourceHealth.Healthy;
        if (!enabled)
            source.HealthStatus = JobSourceHealth.Paused;
        await _sources.UpdateAsync(source, cancellationToken);
        return Result<JobSourceDto>.Success(_mapper.Map<JobSourceDto>(source));
    }

    public async Task<JobSourceConfigDto?> GetSourceConfigAsync(string sourceId, CancellationToken cancellationToken = default)
    {
        var config = await _configs.GetActiveConfigAsync(sourceId, cancellationToken);
        return config == null ? null : _mapper.Map<JobSourceConfigDto>(config);
    }

    // ------------------------------------------------------------------
    // Run + log + raw + duplicate + error queries
    // ------------------------------------------------------------------

    public async Task<List<ScrapeRunDto>> GetRunsAsync(string? sourceId = null, int limit = 50, CancellationToken cancellationToken = default)
    {
        IQueryable<ScrapeRun> query = _runs.Query().OrderByDescending(r => r.StartedAt);
        if (!string.IsNullOrWhiteSpace(sourceId))
            query = query.Where(r => r.JobSourceId == sourceId);
        var runs = await query.Take(Math.Min(limit, 200)).ToListAsync(cancellationToken);
        return _mapper.Map<List<ScrapeRunDto>>(runs);
    }

    public async Task<ScrapeRunDto?> GetRunAsync(string runId, CancellationToken cancellationToken = default)
    {
        var run = await _runs.GetByIdAsync(runId, cancellationToken);
        return run == null ? null : _mapper.Map<ScrapeRunDto>(run);
    }

    public async Task<List<ScrapeLogDto>> GetRunLogsAsync(string runId, CancellationToken cancellationToken = default)
    {
        var logs = await _logs.GetByRunIdAsync(runId, cancellationToken);
        return _mapper.Map<List<ScrapeLogDto>>(logs);
    }

    public async Task<List<RawExternalJobDto>> GetRawJobsAsync(string? sourceId = null, string? status = null, int limit = 100, CancellationToken cancellationToken = default)
    {
        IQueryable<RawExternalJob> query = _rawJobs.Query().OrderByDescending(r => r.FetchedAt);
        if (!string.IsNullOrWhiteSpace(sourceId))
            query = query.Where(r => r.JobSourceId == sourceId);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.ProcessingStatus == status);
        var items = await query.Take(Math.Min(limit, 200)).ToListAsync(cancellationToken);
        return _mapper.Map<List<RawExternalJobDto>>(items);
    }

    public async Task<List<DuplicateCandidateDto>> GetDuplicatesAsync(int limit = 50, CancellationToken cancellationToken = default)
    {
        var items = await _duplicates.GetPendingAsync(limit, cancellationToken);
        return _mapper.Map<List<DuplicateCandidateDto>>(items);
    }

    public async Task<List<IngestionErrorDto>> GetErrorsAsync(string? sourceId = null, int limit = 50, CancellationToken cancellationToken = default)
    {
        IQueryable<IngestionError> query = _errors.Query().OrderByDescending(e => e.CreatedAt);
        if (!string.IsNullOrWhiteSpace(sourceId))
            query = query.Where(e => e.JobSourceId == sourceId);
        var items = await query.Take(Math.Min(limit, 200)).ToListAsync(cancellationToken);
        return _mapper.Map<List<IngestionErrorDto>>(items);
    }

    // ------------------------------------------------------------------
    // Duplicate resolution + raw reprocessing
    // ------------------------------------------------------------------

    public async Task<DuplicateCandidateDto?> ResolveDuplicateAsync(string id, string action, string by, CancellationToken cancellationToken = default)
    {
        var dup = await _duplicates.GetByIdAsync(id, cancellationToken);
        if (dup == null || dup.Status != "Pending")
            return null;

        dup.Status = action.Equals("keep-b", StringComparison.OrdinalIgnoreCase) ? "ResolvedKeepB" : "Resolved";
        dup.ResolvedAt = DateTime.UtcNow;
        dup.ResolvedBy = by;
        await _duplicates.UpdateAsync(dup, cancellationToken);
        return _mapper.Map<DuplicateCandidateDto>(dup);
    }

    public async Task<JobDto?> ReprocessRawJobAsync(string rawId, CancellationToken cancellationToken = default)
    {
        var raw = await _rawJobs.GetByIdAsync(rawId, cancellationToken);
        if (raw == null)
            return null;

        var normalized = Normalize(raw);
        if (normalized == null)
        {
            raw.ProcessingStatus = JobProcessingStatus.Rejected;
            raw.ProcessingError = "Normalization failed";
            await _rawJobs.UpdateAsync(raw, cancellationToken);
            return null;
        }

        var (jobId, _) = await UpsertCanonicalAsync(raw.JobSourceId, raw, normalized, cancellationToken);
        if (jobId == null)
            return null;

        raw.ProcessingStatus = JobProcessingStatus.Published;
        raw.ProcessingError = null;
        await _rawJobs.UpdateAsync(raw, cancellationToken);

        var job = await _jobs.GetByIdAsync(jobId, cancellationToken);
        return job == null ? null : await MapJobToDtoAsync(job, cancellationToken);
    }

    // ------------------------------------------------------------------
    // Main pipeline
    // ------------------------------------------------------------------

    public async Task<ScrapeRunDto> RunSourceAsync(string sourceId, string triggeredBy = "Manual", CancellationToken cancellationToken = default)
    {
        var source = await _sources.GetByIdAsync(sourceId, cancellationToken);
        if (source == null)
            throw new InvalidOperationException($"Job source {sourceId} not found");

        var run = new ScrapeRun
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            JobSourceId = source.Id,
            StartedAt = DateTime.UtcNow,
            Status = ScrapeRunStatus.Running,
            TriggeredBy = triggeredBy,
            CorrelationId = Guid.NewGuid().ToString("N")[..12],
        };
        await _runs.AddAsync(run, cancellationToken);
        await LogAsync(run.Id, "Info", "RunStarted", $"Started run for {source.Name}");

        try
        {
            if (!source.IsEnabled)
            {
                run.Status = ScrapeRunStatus.Cancelled;
                run.ErrorSummary = "Source disabled";
                await _runs.UpdateAsync(run, cancellationToken);
                return _mapper.Map<ScrapeRunDto>(run);
            }

            var rawItems = await FetchAndStoreRawAsync(source, run, cancellationToken);
            run.JobsDiscovered = rawItems.Count;

            if (rawItems.Count == 0 && source.LastSuccessfulRunAt != null)
            {
                // Zero-result guard: keep the last known good state; do not mark as failure.
                await LogAsync(run.Id, "Warning", "ZeroResults", "Fetch returned no jobs; treating as no-change");
                run.Status = ScrapeRunStatus.Succeeded;
                run.CompletedAt = DateTime.UtcNow;
                run.DurationMs = (long)(run.CompletedAt.Value - run.StartedAt).TotalMilliseconds;
                await _runs.UpdateAsync(run, cancellationToken);
                source.LastSuccessfulRunAt = DateTime.UtcNow;
                source.ConsecutiveFailures = 0;
                source.HealthStatus = JobSourceHealth.Healthy;
                await _sources.UpdateAsync(source, cancellationToken);
                return _mapper.Map<ScrapeRunDto>(run);
            }

            foreach (var raw in rawItems)
            {
                await ProcessRawAsync(source, run, raw, cancellationToken);
            }

            await CloseStaleJobsAsync(source, run, cancellationToken);

            run.Status = run.JobsRejected == 0 ? ScrapeRunStatus.Succeeded : ScrapeRunStatus.PartiallySucceeded;
            run.CompletedAt = DateTime.UtcNow;
            run.DurationMs = (long)(run.CompletedAt.Value - run.StartedAt).TotalMilliseconds;
            await _runs.UpdateAsync(run, cancellationToken);

            await UpdateSourceHealthAsync(source, success: true, null, cancellationToken);
            await LogAsync(run.Id, "Info", "RunCompleted",
                $"Completed: discovered={run.JobsDiscovered} created={run.JobsCreated} updated={run.JobsUpdated} duplicate={run.JobsDuplicate} rejected={run.JobsRejected} closed={run.JobsClosed}");
            return _mapper.Map<ScrapeRunDto>(run);
        }
        catch (Exception ex)
        {
            run.Status = ScrapeRunStatus.Failed;
            run.ErrorSummary = ex.Message.Length > 500 ? ex.Message[..500] : ex.Message;
            run.CompletedAt = DateTime.UtcNow;
            run.DurationMs = (long)(run.CompletedAt.Value - run.StartedAt).TotalMilliseconds;
            await _runs.UpdateAsync(run, cancellationToken);
            await LogAsync(run.Id, "Error", "RunFailed", ex.Message, exceptionType: ex.GetType().Name);
            await UpdateSourceHealthAsync(source, success: false, ex.Message, cancellationToken);
            return _mapper.Map<ScrapeRunDto>(run);
        }
    }

    private async Task ProcessRawAsync(JobSource source, ScrapeRun run, RawExternalJob raw, CancellationToken cancellationToken)
    {
        try
        {
            var normalized = Normalize(raw);
            if (normalized == null)
            {
                raw.ProcessingStatus = JobProcessingStatus.Rejected;
                raw.ProcessingError = "Normalization failed";
                run.JobsRejected++;
                await _rawJobs.UpdateAsync(raw, cancellationToken);
                await LogAsync(run.Id, "Warning", "Rejected", $"Rejected job {raw.ExternalJobId}", externalJobId: raw.ExternalJobId);
                return;
            }

            var validation = Validate(normalized);
            if (!validation.Valid)
            {
                raw.ProcessingStatus = JobProcessingStatus.Rejected;
                raw.ProcessingError = validation.Reason;
                run.JobsRejected++;
                await _rawJobs.UpdateAsync(raw, cancellationToken);
                await LogAsync(run.Id, "Warning", "Rejected", $"Rejected job {raw.ExternalJobId}: {validation.Reason}", externalJobId: raw.ExternalJobId);
                return;
            }

            var (jobId, outcome) = await UpsertCanonicalAsync(source.Id, raw, normalized, cancellationToken);
            switch (outcome)
            {
                case "Created": run.JobsCreated++; break;
                case "Updated": run.JobsUpdated++; break;
                case "Unchanged": run.JobsUnchanged++; break;
                case "Duplicate": run.JobsDuplicate++; break;
                default: run.JobsRejected++; break;
            }

            if (outcome is "Created" or "Updated" or "Unchanged" or "Duplicate")
            {
                raw.ProcessingStatus = outcome == "Duplicate" ? JobProcessingStatus.Duplicate : JobProcessingStatus.Published;
                raw.ProcessingError = null;
            }
            await _rawJobs.UpdateAsync(raw, cancellationToken);
        }
        catch (Exception ex)
        {
            raw.ProcessingStatus = JobProcessingStatus.Error;
            raw.ProcessingError = ex.Message.Length > 500 ? ex.Message[..500] : ex.Message;
            run.JobsRejected++;
            await _rawJobs.UpdateAsync(raw, cancellationToken);
            await LogAsync(run.Id, "Error", "ProcessError", $"Failed job {raw.ExternalJobId}: {ex.Message}", externalJobId: raw.ExternalJobId);
        }
    }

    // ------------------------------------------------------------------
    // Fetch adapters
    // ------------------------------------------------------------------

    private async Task<List<RawExternalJob>> FetchAndStoreRawAsync(JobSource source, ScrapeRun run, CancellationToken cancellationToken)
    {
        var adapter = source.AdapterKey.ToLowerInvariant();
        List<RawExternalJob> items = [];

        if (adapter == "fixture")
        {
            foreach (var demo in JobsScraperSeedData.DemoJobsForSource(source.Slug))
            {
                var raw = new RawExternalJob
                {
                    Id = Guid.NewGuid().ToString("N")[..20],
                    JobSourceId = source.Id,
                    ExternalJobId = demo.Slug,
                    SourceUrl = $"fixture://{source.Slug}/{demo.Slug}",
                    ApplyUrl = demo.ExternalApplyUrl,
                    RawTitle = demo.Title,
                    RawCompany = demo.CompanyName,
                    RawLocation = demo.Location,
                    RawSalary = demo.SalaryText,
                    RawPostedDate = demo.PublishedAt,
                    RawEmploymentType = demo.EmploymentType,
                    RawWorkMode = demo.WorkMode,
                    RawSkills = string.Join(", ", demo.Skills),
                    RawIndustry = demo.Industry,
                    RawDescription = demo.Summary + " " + string.Join(" ", demo.Responsibilities) + " " + string.Join(" ", demo.Requirements),
                    PayloadHash = Sha256(JobsScraperSeedData.RawPayload(demo)),
                    FetchedAt = DateTime.UtcNow,
                    FirstSeenAt = DateTime.UtcNow,
                    LastSeenAt = DateTime.UtcNow,
                    ProcessingStatus = JobProcessingStatus.New,
                };
                items.Add(raw);
            }
        }
        else if (!string.IsNullOrWhiteSpace(source.FeedUrl) &&
                 (source.FeedUrl.StartsWith("http://") || source.FeedUrl.StartsWith("https://")))
        {
            var (statusCode, content, httpError) = await FetchWithPolicyAsync(source, cancellationToken);
            run.HttpRequests++;
            if (statusCode == null || content == null)
            {
                run.HttpErrors++;
                await LogAsync(run.Id, "Error", "FetchFailed", httpError ?? "Fetch failed", url: source.FeedUrl, httpStatusCode: statusCode);
                return items;
            }

            try
            {
                items = adapter switch
                {
                    "rss" or "xmlfeed" => ParseXmlJobs(source, content),
                    "sitemap" => ParseSitemapJobs(source, content),
                    "htmlcareerpage" => ParseHtmlJobs(source, content),
                    _ => ParseJsonJobs(source, content),
                };
                run.JobsFetched = items.Count;
            }
            catch (Exception ex)
            {
                run.ParseErrors++;
                await LogAsync(run.Id, "Error", "ParseFailed", ex.Message, url: source.FeedUrl, exceptionType: ex.GetType().Name);
            }
        }

        foreach (var raw in items)
        {
            var existing = await _rawJobs.GetBySourceAndExternalIdAsync(source.Id, raw.ExternalJobId, cancellationToken);
            if (existing != null)
            {
                existing.RawTitle = raw.RawTitle;
                existing.RawCompany = raw.RawCompany;
                existing.RawLocation = raw.RawLocation;
                existing.RawSalary = raw.RawSalary;
                existing.RawPostedDate = raw.RawPostedDate;
                existing.RawEmploymentType = raw.RawEmploymentType;
                existing.RawWorkMode = raw.RawWorkMode;
                existing.RawSkills = raw.RawSkills;
                existing.RawIndustry = raw.RawIndustry;
                existing.ApplyUrl = raw.ApplyUrl;
                existing.SourceUrl = raw.SourceUrl;
                existing.PayloadHash = raw.PayloadHash;
                existing.LastSeenAt = DateTime.UtcNow;
                existing.ProcessingStatus = JobProcessingStatus.New;
                await _rawJobs.UpdateAsync(existing, cancellationToken);
            }
            else
            {
                await _rawJobs.AddAsync(raw, cancellationToken);
            }
        }

        return items;
    }

    private async Task<(int? StatusCode, string? Content, string? Error)> FetchWithPolicyAsync(JobSource source, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, source.FeedUrl);
        if (!string.IsNullOrWhiteSpace(source.UserAgent))
            request.Headers.UserAgent.ParseAdd(source.UserAgent);
        request.Headers.Add("Accept", "application/json, application/xml, text/html, */*");

        try
        {
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(30);
                await Task.Delay(retryAfter, cancellationToken);
                using var retry = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                if (!retry.IsSuccessStatusCode)
                    return ((int)retry.StatusCode, null, $"HTTP {retry.StatusCode}");
                var body = await retry.Content.ReadAsStringAsync(cancellationToken);
                return ((int)retry.StatusCode, body, null);
            }

            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                return ((int)response.StatusCode, null, $"HTTP {response.StatusCode} - source requires authorization");

            if (!response.IsSuccessStatusCode)
                return ((int)response.StatusCode, null, $"HTTP {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return ((int)response.StatusCode, content, null);
        }
        catch (Exception ex)
        {
            return (null, null, ex.Message);
        }
    }

    private static List<RawExternalJob> ParseJsonJobs(JobSource source, string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        var list = ResolveJobList(root);

        var result = new List<RawExternalJob>();
        foreach (var item in list)
        {
            var externalId = Get(item, "id", "external_id", "jobId", "job_id", "slug") ?? Guid.NewGuid().ToString("N")[..10];
            var title = Get(item, "title", "jobTitle", "job_title") ?? "Untitled";
            var company = Get(item, "company", "companyName", "company_name") ?? source.Name;
            var applyUrl = Get(item, "applyUrl", "apply_url", "url", "applicationUrl", "externalApplyUrl");
            var sourceUrl = Get(item, "sourceUrl", "source_url", "pageUrl") ?? source.CareersUrl ?? source.FeedUrl;
            var posted = Get(item, "posted", "postedAt", "publishedAt", "date", "createdAt") ?? "";

            var raw = new RawExternalJob
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                JobSourceId = source.Id,
                ExternalJobId = externalId,
                SourceUrl = sourceUrl,
                ApplyUrl = applyUrl,
                RawTitle = title,
                RawCompany = company,
                RawLocation = Get(item, "location", "city", "address"),
                RawSalary = Get(item, "salary", "salaryText", "compensation"),
                RawPostedDate = posted,
                RawEmploymentType = Get(item, "employmentType", "type", "jobType"),
                RawWorkMode = Get(item, "workMode", "workplace", "workType"),
                RawSkills = Get(item, "skills"),
                RawIndustry = Get(item, "industry", "department", "team"),
                PayloadHash = Sha256(item.GetRawText()),
                FetchedAt = DateTime.UtcNow,
                FirstSeenAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
                ProcessingStatus = JobProcessingStatus.New,
            };
            result.Add(raw);
        }
        return result;
    }

    private static List<RawExternalJob> ParseXmlJobs(JobSource source, string xml)
    {
        XDocument doc;
        try { doc = XDocument.Parse(xml); }
        catch { return []; }

        var items = doc.Descendants("item").Concat(doc.Descendants("job")).ToList();
        var result = new List<RawExternalJob>();
        foreach (var item in items)
        {
            string? Val(string name) => item.Element(name)?.Value ?? item.Descendants(name).FirstOrDefault()?.Value;
            var title = Val("title") ?? "Untitled";
            var externalId = Val("id") ?? Val("guid") ?? Sha256(title)[..12];
            var raw = new RawExternalJob
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                JobSourceId = source.Id,
                ExternalJobId = externalId,
                SourceUrl = Val("link") ?? Val("url") ?? source.FeedUrl,
                ApplyUrl = Val("applyUrl") ?? Val("applicationUrl") ?? Val("link"),
                RawTitle = title,
                RawCompany = Val("company") ?? Val("companyName") ?? source.Name,
                RawLocation = Val("location") ?? Val("city"),
                RawSalary = Val("salary") ?? Val("compensation"),
                RawPostedDate = Val("pubDate") ?? Val("postedDate") ?? Val("postedAt"),
                RawEmploymentType = Val("employmentType") ?? Val("type"),
                RawWorkMode = Val("workMode") ?? Val("workplaceType"),
                RawSkills = Val("skills"),
                RawIndustry = Val("industry") ?? Val("category"),
                PayloadHash = Sha256(item.ToString()),
                FetchedAt = DateTime.UtcNow,
                FirstSeenAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
                ProcessingStatus = JobProcessingStatus.New,
            };
            result.Add(raw);
        }
        return result;
    }

    private static List<RawExternalJob> ParseSitemapJobs(JobSource source, string xml)
    {
        XDocument doc;
        try { doc = XDocument.Parse(xml); }
        catch { return []; }

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var urls = doc.Descendants(ns + "url").ToList();
        var result = new List<RawExternalJob>();
        foreach (var url in urls)
        {
            var loc = url.Element(ns + "loc")?.Value;
            if (string.IsNullOrWhiteSpace(loc))
                continue;
            var externalId = Sha256(loc)[..12];
            var raw = new RawExternalJob
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                JobSourceId = source.Id,
                ExternalJobId = externalId,
                SourceUrl = loc,
                ApplyUrl = loc,
                RawTitle = ExtractTitleFromUrl(loc),
                RawCompany = source.Name,
                RawLocation = "",
                PayloadHash = Sha256(loc),
                FetchedAt = DateTime.UtcNow,
                FirstSeenAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
                ProcessingStatus = JobProcessingStatus.New,
            };
            result.Add(raw);
        }
        return result;
    }

    private static List<RawExternalJob> ParseHtmlJobs(JobSource source, string html)
    {
        // Lightweight heuristic parse of a careers page: look for JSON-LD / scripts
        // and anchor patterns. Best-effort — real ATS pages get a dedicated adapter.
        var result = new List<RawExternalJob>();
        var jsonLdMatches = Regex.Matches(html, @"<script[^>]*application/ld\+json[^>]*>(.*?)</script>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);
        foreach (Match m in jsonLdMatches)
        {
            try
            {
                using var doc = JsonDocument.Parse(m.Groups[1].Value);
                var el = doc.RootElement;
                if (el.TryGetProperty("@graph", out var graph) && graph.ValueKind == JsonValueKind.Array)
                {
                    foreach (var node in graph.EnumerateArray())
                    {
                        if (node.TryGetProperty("@type", out var type) && type.ToString().Contains("JobPosting"))
                            result.Add(HtmlJobFromLd(source, node));
                    }
                }
                else if (el.TryGetProperty("@type", out var single) && single.ToString().Contains("JobPosting"))
                {
                    result.Add(HtmlJobFromLd(source, el));
                }
            }
            catch { /* skip malformed JSON-LD */ }
        }
        return result;
    }

    private static RawExternalJob HtmlJobFromLd(JobSource source, JsonElement node)
    {
        var title = Get(node, "title") ?? ExtractTitleFromUrl(node.TryGetProperty("url", out var u) ? u.GetString() : null);
        var externalId = Sha256(node.GetRawText())[..12];
        return new RawExternalJob
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            JobSourceId = source.Id,
            ExternalJobId = externalId,
            SourceUrl = node.TryGetProperty("url", out var su) ? su.GetString() : source.CareersUrl,
            ApplyUrl = node.TryGetProperty("url", out var au) ? au.GetString() : null,
            RawTitle = title,
            RawCompany = Get(node, "hiringOrganization", "company") ?? source.Name,
            RawLocation = Get(node, "jobLocation", "address", "location"),
            RawSalary = Get(node, "baseSalary", "salary"),
            RawPostedDate = Get(node, "datePosted"),
            RawEmploymentType = Get(node, "employmentType"),
            PayloadHash = Sha256(node.GetRawText()),
            FetchedAt = DateTime.UtcNow,
            FirstSeenAt = DateTime.UtcNow,
            LastSeenAt = DateTime.UtcNow,
            ProcessingStatus = JobProcessingStatus.New,
        };
    }

    // ------------------------------------------------------------------
    // Normalization
    // ------------------------------------------------------------------

    private sealed record NormalizedJob(
        string Title, string Company, string Category, string Location, string City, string State,
        string Country, string ExperienceText, int MinExperience, int MaxExperience, string SalaryText,
        decimal? SalaryMin, decimal? SalaryMax, string SalaryCurrency, string WorkMode, string EmploymentType,
        string Summary, string[] Skills, string[] Responsibilities, string[] Requirements, string[] Benefits,
        string ApplicationMode, string? ExternalApplyUrl, string? SourceUrl, DateTime? PostedAt,
        bool Featured, bool Verified, string Slug, string Fingerprint);

    private NormalizedJob? Normalize(RawExternalJob raw)
    {
        try
        {
            var title = NormalizeTitle(raw.RawTitle ?? "");
            if (string.IsNullOrWhiteSpace(title))
                return null;

            var company = NormalizeTitle(raw.RawCompany ?? "");
            var (city, state, country, location) = ParseLocation(raw.RawLocation ?? "");
            var (workMode, employmentType) = ParseWorkModeAndType(title, raw.RawEmploymentType, raw.RawWorkMode);
            var (minExp, maxExp, expText) = ParseExperience(title, raw.RawTitle);
            var (salaryMin, salaryMax, currency, salaryText) = ParseSalary(raw.RawSalary ?? "", raw.RawPostedDate ?? "");
            var summary = CleanHtml(raw.RawDescription ?? "");
            var description = CleanHtml(raw.RawTitle ?? "") + "\n\n" + summary;
            var skills = ExtractSkills(title + " " + (raw.RawSkills ?? "") + " " + summary);
            var bullets = SplitBullets(summary);
            var responsibilities = bullets.Count >= 3 ? bullets.Take(3).ToArray() : [];
            var requirements = bullets.Count >= 3 ? bullets.Skip(3).ToArray() : [];
            var postedAt = ParseDate(raw.RawPostedDate);
            var slug = SlugHelper.GenerateSlug(title + " " + company);
            var fingerprint = Fingerprint(company, title, location, summary);

            return new NormalizedJob(
                title, company, "Engineering", location, city, state, country, expText, minExp, maxExp,
                salaryText, salaryMin, salaryMax, currency, workMode, employmentType, summary, skills,
                responsibilities, requirements, [], "EasyApply", raw.ApplyUrl, raw.SourceUrl, postedAt,
                false, false, slug, fingerprint);
        }
        catch
        {
            return null;
        }
    }

    private static (bool Valid, string Reason) Validate(NormalizedJob j)
    {
        if (string.IsNullOrWhiteSpace(j.Title) || j.Title.Length < 5)
            return (false, "Title too short");
        if (string.IsNullOrWhiteSpace(j.Company))
            return (false, "Missing company");
        if (string.IsNullOrWhiteSpace(j.ExternalApplyUrl) && string.IsNullOrWhiteSpace(j.SourceUrl))
            return (false, "Missing apply/source URL");
        var combined = $"{j.Title} {j.Company} {j.Summary}".ToLowerInvariant();
        foreach (var flag in new[] { "test job", "demo only", "sample job", "apply through our app only", "upload your resume to win" })
        {
            if (combined.Contains(flag))
                return (false, $"Flagged text: {flag}");
        }
        return (true, string.Empty);
    }

    private async Task<(string? JobId, string Outcome)> UpsertCanonicalAsync(string sourceId, RawExternalJob raw, NormalizedJob n, CancellationToken cancellationToken)
    {
        // 1. Existing mapping -> update or no-change.
        var existingMapping = await _mappings.GetBySourceAndExternalIdAsync(sourceId, raw.ExternalJobId, cancellationToken);
        if (existingMapping != null)
        {
            var existingJob = await _jobs.GetByIdAsync(existingMapping.JobId, cancellationToken);
            if (existingJob == null)
                return (null, "Rejected");

            if (existingJob.CanonicalFingerprint == n.Fingerprint)
                return (existingJob.Id, "Unchanged");

            await ApplyNormalizedToJobAsync(existingJob, n, raw, sourceId);
            await _jobs.UpdateAsync(existingJob, cancellationToken);
            return (existingJob.Id, "Updated");
        }

        // 2. Fingerprint dedup across canonical jobs.
        var fingerprintMatch = await _jobs.Query()
            .FirstOrDefaultAsync(j => j.CanonicalFingerprint == n.Fingerprint && j.Status != "closed", cancellationToken);
        if (fingerprintMatch != null)
        {
            var dup = new DuplicateCandidate
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                JobIdA = fingerprintMatch.Id,
                JobIdB = existingMapping?.JobId ?? "",
                Score = 80,
                Status = "Pending",
            };
            await _duplicates.AddAsync(dup, cancellationToken);
            return (null, "Duplicate");
        }

        // 3. Create canonical Job + mapping.
        var company = await FindOrCreateCompanyAsync(n.Company, cancellationToken);
        var job = new Job
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            CompanyId = company.Id,
            Status = "published",
        };
        await ApplyNormalizedToJobAsync(job, n, raw, sourceId);
        job.Slug = await SlugHelper.EnsureUniqueSlugAsync(n.Slug,
            slug => _jobs.ExistsAsync(j => j.Slug == slug, cancellationToken));

        await _jobs.AddAsync(job, cancellationToken);
        await _mappings.AddAsync(new JobSourceMapping
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            JobId = job.Id,
            JobSourceId = sourceId,
            ExternalJobId = raw.ExternalJobId,
            IsPrimary = true,
        }, cancellationToken);
        return (job.Id, "Created");
    }

    private async Task ApplyNormalizedToJobAsync(Job job, NormalizedJob n, RawExternalJob raw, string sourceId)
    {
        job.Title = n.Title.Length > 200 ? n.Title[..200] : n.Title;
        job.Description = BuildDescription(n).Length > 5000 ? BuildDescription(n)[..5000] : BuildDescription(n);
        job.Requirements = string.Join("\n", n.Requirements);
        job.Category = n.Category;
        job.Type = n.EmploymentType.Equals("Internship", StringComparison.OrdinalIgnoreCase) ? "internship" : "full-time";
        job.ExperienceLevel = n.ExperienceText;
        job.Location = n.Location;
        job.IsRemote = n.WorkMode.Equals("Remote", StringComparison.OrdinalIgnoreCase);
        job.SalaryMin = n.SalaryMin;
        job.SalaryMax = n.SalaryMax;
        job.SalaryCurrency = n.SalaryCurrency;
        job.Status = job.Status == "closed" ? "closed" : "published";
        job.PublishedAt ??= n.PostedAt ?? DateTime.UtcNow;
        job.ExpiresAt = DateTime.UtcNow.AddDays(30);

        job.CompanyName = n.Company.Length > 200 ? n.Company[..200] : n.Company;
        job.CompanyInitials = Initials(n.Company);
        job.Industry = n.Category;
        job.City = string.IsNullOrWhiteSpace(n.City) ? null : n.City;
        job.State = string.IsNullOrWhiteSpace(n.State) ? null : n.State;
        job.Country = string.IsNullOrWhiteSpace(n.Country) ? "India" : n.Country;
        job.ExperienceText = n.ExperienceText;
        job.MinExperience = n.MinExperience;
        job.MaxExperience = n.MaxExperience;
        job.SalaryText = n.SalaryText;
        job.SalaryVisible = n.SalaryMin.HasValue;
        job.WorkMode = n.WorkMode;
        job.EmploymentType = n.EmploymentType;
        job.Summary = n.Summary.Length > 500 ? n.Summary[..500] : n.Summary;
        job.SkillsJson = JsonSerializer.Serialize(n.Skills);
        job.ResponsibilitiesJson = JsonSerializer.Serialize(n.Responsibilities);
        job.BenefitsJson = JsonSerializer.Serialize(n.Benefits);
        job.ApplicationMode = n.ApplicationMode;
        job.ExternalApplyUrl = n.ExternalApplyUrl;
        job.OriginalSourceUrl = n.SourceUrl;
        job.SourceType = JobSourceType.JsonFeed;
        job.IsAggregated = true;
        job.Featured = n.Featured;
        job.Verified = n.Verified;
        job.ExternalJobId = raw.ExternalJobId.Length > 200 ? raw.ExternalJobId[..200] : raw.ExternalJobId;
        job.PrimaryJobSourceId = sourceId;
        job.PostedAtSource = raw.RawPostedDate;
        job.LastSeenAtSource = DateTime.UtcNow;
        job.CanonicalFingerprint = n.Fingerprint;
    }

    private async Task CloseStaleJobsAsync(JobSource source, ScrapeRun run, CancellationToken cancellationToken)
    {
        // Jobs mapped to this source that were NOT seen in this fetch are stale.
        var mappings = await _mappings.Query().Where(m => m.JobSourceId == source.Id).ToListAsync(cancellationToken);
        if (mappings.Count == 0)
            return;

        var seenExternalIds = (await _rawJobs.GetBySourceIdAsync(source.Id, MaxJobsPerFetch, cancellationToken))
            .Where(r => r.ProcessingStatus != JobProcessingStatus.Rejected && r.ProcessingStatus != JobProcessingStatus.Error)
            .Select(r => r.ExternalJobId)
            .ToHashSet();

        var closed = 0;
        foreach (var mapping in mappings)
        {
            if (closed >= MaxStaleClosePerRun)
                break;
            if (seenExternalIds.Contains(mapping.ExternalJobId))
                continue;

            var job = await _jobs.GetByIdAsync(mapping.JobId, cancellationToken);
            if (job == null || job.Status == "closed" || job.IsAggregated == false)
                continue;

            var latestRun = await _runs.GetLatestRunAsync(source.Id, cancellationToken);
            if (latestRun != null && latestRun.JobsDiscovered > 0 && latestRun.Status == ScrapeRunStatus.Succeeded)
            {
                job.Status = "closed";
                job.UpdatedAt = DateTime.UtcNow;
                await _jobs.UpdateAsync(job, cancellationToken);
                closed++;
            }
        }
        run.JobsClosed += closed;
    }

    private async Task UpdateSourceHealthAsync(JobSource source, bool success, string? error, CancellationToken cancellationToken)
    {
        if (success)
        {
            source.ConsecutiveFailures = 0;
            source.LastSuccessfulRunAt = DateTime.UtcNow;
            source.HealthStatus = JobSourceHealth.Healthy;
        }
        else
        {
            source.ConsecutiveFailures++;
            source.LastFailedRunAt = DateTime.UtcNow;
            source.HealthStatus = source.ConsecutiveFailures >= AutoPauseThreshold
                ? JobSourceHealth.Failing
                : source.ConsecutiveFailures >= WarningConsecutiveFailures
                    ? JobSourceHealth.Warning
                    : source.HealthStatus;
            if (source.ConsecutiveFailures >= AutoPauseThreshold)
            {
                source.IsEnabled = false;
                source.HealthStatus = JobSourceHealth.Paused;
            }
        }
        await _sources.UpdateAsync(source, cancellationToken);
    }

    // ------------------------------------------------------------------
    // Dashboard
    // ------------------------------------------------------------------

    public async Task<ScraperDashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var dto = new ScraperDashboardDto();
        var allSources = await _sources.GetAllAsync(cancellationToken);
        dto.Sources = _mapper.Map<List<JobSourceDto>>(allSources);

        dto.Cards.EnabledSources = allSources.Count(s => s.IsEnabled);
        dto.Cards.HealthySources = allSources.Count(s => s.HealthStatus == JobSourceHealth.Healthy);
        dto.Cards.FailingSources = allSources.Count(s => s.HealthStatus is JobSourceHealth.Failing or JobSourceHealth.Warning);

        var today = DateTime.UtcNow.Date;
        var recentRuns = await _runs.Query()
            .Where(r => r.StartedAt >= today.AddDays(-14))
            .ToListAsync(cancellationToken);
        var todayRuns = recentRuns.Where(r => r.StartedAt.Date == today).ToList();

        dto.Cards.JobsImportedToday = todayRuns.Sum(r => r.JobsCreated);
        dto.Cards.JobsUpdatedToday = todayRuns.Sum(r => r.JobsUpdated);
        dto.Cards.JobsClosedToday = todayRuns.Sum(r => r.JobsClosed);
        dto.Cards.DuplicatesDetected = await _duplicates.CountAsync(d => d.Status == "Pending", cancellationToken);
        dto.Cards.ParseErrors = recentRuns.Sum(r => r.ParseErrors);
        dto.Cards.HttpErrors = recentRuns.Sum(r => r.HttpErrors);
        dto.Cards.AverageRunMs = recentRuns.Count == 0 ? 0 : (long)recentRuns.Average(r => r.DurationMs);
        dto.Cards.TotalJobs = await _jobs.CountAsync(j => j.Status != "closed", cancellationToken);
        dto.Cards.TotalRaw = await _rawJobs.CountAsync(null, cancellationToken);

        dto.Charts.RunsOverTime = recentRuns
            .GroupBy(r => r.StartedAt.Date)
            .OrderBy(g => g.Key)
            .Select(g => new RunsOverTimePointDto
            {
                Date = g.Key.ToString("MM-dd"),
                Runs = g.Count(),
                Created = g.Sum(r => r.JobsCreated),
                Closed = g.Sum(r => r.JobsClosed),
            })
            .ToList();

        dto.Charts.JobsBySource = allSources.Select(s => new JobsBySourcePointDto
        {
            SourceId = s.Id,
            Discovered = recentRuns.Where(r => r.JobSourceId == s.Id).Sum(r => r.JobsDiscovered),
            Created = recentRuns.Where(r => r.JobSourceId == s.Id).Sum(r => r.JobsCreated),
        }).ToList();

        var errorsBySource = await _errors.Query().Where(e => e.CreatedAt >= today.AddDays(-14)).ToListAsync(cancellationToken);
        dto.Charts.ErrorsBySource = errorsBySource
            .GroupBy(e => e.JobSourceId ?? "")
            .Select(g => new ErrorsBySourcePointDto { SourceId = g.Key, Count = g.Count() })
            .ToList();

        return dto;
    }

    // ------------------------------------------------------------------
    // Scheduler contract
    // ------------------------------------------------------------------

    public async Task<List<JobSourceDto>> GetDueSourcesAsync(CancellationToken cancellationToken = default)
    {
        var due = await _sources.GetDueSourcesAsync(cancellationToken);
        return _mapper.Map<List<JobSourceDto>>(due);
    }

    public void ScheduleNextRun(string sourceId, int intervalMinutes)
    {
        // Interval is stored on the JobSource (RequestIntervalMinutes) and evaluated
        // by GetDueSourcesAsync. Nothing extra to persist here.
    }

    // ------------------------------------------------------------------
    // SSRF guard
    // ------------------------------------------------------------------

    public Task<(bool Ok, string Error)> ValidateUrlAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return Task.FromResult<(bool, string)>((false, "URL is required"));

        if (url.StartsWith("fixture://", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult<(bool, string)>((true, string.Empty));

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return Task.FromResult<(bool, string)>((false, "Invalid URL"));

        if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            return Task.FromResult<(bool, string)>((false, "Only http(s) URLs are allowed"));

        if (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
            || uri.Host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase)
            || uri.Host.Equals("::1", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult<(bool, string)>((false, "Localhost URLs are blocked"));

        if (IPAddress.TryParse(uri.Host, out var ip))
        {
            if (IPAddress.IsLoopback(ip) || IsPrivateIp(ip))
                return Task.FromResult<(bool, string)>((false, "Private/internal IP URLs are blocked"));
        }
        else if (uri.Host.EndsWith(".internal", StringComparison.OrdinalIgnoreCase)
            || uri.Host.EndsWith(".local", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult<(bool, string)>((false, "Internal hostnames are blocked"));
        }

        return Task.FromResult<(bool, string)>((true, string.Empty));
    }

    private static bool IsPrivateIp(IPAddress ip)
    {
        if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            return ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 && ip.IsIPv6LinkLocal;
        var bytes = ip.GetAddressBytes();
        return bytes[0] == 10
            || (bytes[0] == 172 && bytes[1] is >= 16 and <= 31)
            || (bytes[0] == 192 && bytes[1] == 168)
            || bytes[0] == 169 && bytes[1] == 254;
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private async Task LogAsync(string runId, string level, string eventType, string message,
        string? url = null, string? externalJobId = null, int? httpStatusCode = null,
        string? exceptionType = null, CancellationToken cancellationToken = default)
    {
        await _logs.AddAsync(new ScrapeLog
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            ScrapeRunId = runId,
            Level = level,
            EventType = eventType,
            Message = message.Length > 1000 ? message[..1000] : message,
            Url = url,
            ExternalJobId = externalJobId,
            HttpStatusCode = httpStatusCode,
            ExceptionType = exceptionType,
            CorrelationId = null,
            CreatedAt = DateTime.UtcNow,
        }, cancellationToken);
    }

    private async Task<Company> FindOrCreateCompanyAsync(string companyName, CancellationToken cancellationToken)
    {
        var slug = SlugHelper.GenerateSlug(companyName);
        var company = await _companies.GetBySlugAsync(slug, cancellationToken);
        if (company != null)
            return company;

        company = new Company
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            Name = companyName.Length > 200 ? companyName[..200] : companyName,
            Slug = slug,
            Description = companyName,
            Location = "India",
            Industry = "Technology",
        };
        await _companies.AddAsync(company, cancellationToken);
        return company;
    }

    private async Task<JobDto> MapJobToDtoAsync(Job job, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<JobDto>(job);
        if (!string.IsNullOrEmpty(job.CompanyId))
        {
            var company = await _companies.GetByIdAsync(job.CompanyId, cancellationToken);
            dto.CompanyName = company?.Name ?? job.CompanyName;
        }
        return dto;
    }

    private static string BuildDescription(NormalizedJob n)
    {
        var sb = new StringBuilder();
        sb.AppendLine(n.Summary);
        if (n.Responsibilities.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine("What you will do:");
            foreach (var r in n.Responsibilities)
                sb.AppendLine($"- {r}");
        }
        if (n.Requirements.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine("What we are looking for:");
            foreach (var r in n.Requirements)
                sb.AppendLine($"- {r}");
        }
        return sb.ToString().Trim();
    }

    private static string NormalizeTitle(string title)
    {
        var t = CleanHtml(title).Trim();
        t = Regex.Replace(t, @"\s+", " ");
        t = Regex.Replace(t, @"\b(Remote|Hybrid|On-site|Onsite)\b.*$", "", RegexOptions.IgnoreCase).Trim();
        t = Regex.Replace(t, @"\b(full[- ]?time|part[- ]?time|internship)\b.*$", "", RegexOptions.IgnoreCase).Trim();
        t = t.TrimEnd('-', '|', ',', ' ');
        return t.Length > 200 ? t[..200] : t;
    }

    private static string CleanHtml(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
        var text = Regex.Replace(input, "<[^>]+>", " ");
        text = WebUtility.HtmlDecode(text);
        text = Regex.Replace(text, @"\s+", " ").Trim();
        return text;
    }

    private static (string City, string State, string Country, string Location) ParseLocation(string raw)
    {
        var parts = raw.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var city = parts.Length > 0 ? parts[0] : "";
        var state = parts.Length > 1 ? parts[1] : "";
        var country = parts.Length > 2 ? parts[2] : (parts.Length > 1 ? "" : "");
        if (parts.Length > 2 && parts[2].Length <= 3)
            country = "India";
        var location = string.Join(", ", parts).Trim();
        return (city, state, country.Length == 0 ? "India" : country, location.Length == 0 ? city : location);
    }

    private static (string WorkMode, string EmploymentType) ParseWorkModeAndType(string title, string? rawType, string? rawWorkMode)
    {
        var workMode = rawWorkMode ?? "";
        if (string.IsNullOrWhiteSpace(workMode))
            workMode = title.Contains("Remote", StringComparison.OrdinalIgnoreCase) ? "Remote"
                : title.Contains("Hybrid", StringComparison.OrdinalIgnoreCase) ? "Hybrid" : "On-site";

        var employmentType = rawType ?? "";
        if (string.IsNullOrWhiteSpace(employmentType))
            employmentType = title.Contains("Intern", StringComparison.OrdinalIgnoreCase) ? "Internship" : "Full-time";
        else
            employmentType = employmentType switch
            {
                "INTERN" or "internship" => "Internship",
                "PART_TIME" or "Part-time" or "part time" => "Part-time",
                "CONTRACT" or "Contract" => "Contract",
                _ => "Full-time",
            };
        return (workMode, employmentType);
    }

    private static (int Min, int Max, string Text) ParseExperience(string title, string? rawTitle)
    {
        var combined = (rawTitle ?? "") + " " + title;
        var match = Regex.Match(combined, @"(\d+)\s*(?:-|to)\s*(\d+)\s*(?:yrs?|years?)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var min = int.Parse(match.Groups[1].Value);
            var max = int.Parse(match.Groups[2].Value);
            return (min, max, $"{min}-{max} years");
        }
        match = Regex.Match(combined, @"(\d+)\s*\+?\s*(?:yrs?|years?)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var min = int.Parse(match.Groups[1].Value);
            return (min, min + 2, $"{min}+ years");
        }
        if (title.Contains("Intern", StringComparison.OrdinalIgnoreCase))
            return (0, 0, "Fresher");
        if (title.Contains("Senior", StringComparison.OrdinalIgnoreCase))
            return (5, 8, "5-8 years");
        if (title.Contains("Lead", StringComparison.OrdinalIgnoreCase) || title.Contains("Manager", StringComparison.OrdinalIgnoreCase))
            return (7, 12, "7-12 years");
        return (2, 5, "2-5 years");
    }

    private static (decimal? Min, decimal? Max, string Currency, string Text) ParseSalary(string raw, string fallbackText)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (null, null, "INR", fallbackText);
        var text = raw.Trim();
        var currency = text.Contains("$") ? "USD" : "INR";
        var match = Regex.Match(text, @"(\d+(?:\.\d+)?)\s*(?:-|to)\s*(\d+(?:\.\d+)?)\s*(?:LPA|lakh|Lacs)?", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var min = decimal.Parse(match.Groups[1].Value);
            var max = decimal.Parse(match.Groups[2].Value);
            return (min, max, currency, text);
        }
        match = Regex.Match(text, @"(\d+(?:\.\d+)?)\s*(LPA|lakh|Lacs)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var val = decimal.Parse(match.Groups[1].Value);
            return (val, val, currency, text);
        }
        match = Regex.Match(text, @"(\d+(?:\.\d+)?)\s*(?:k|K)(?:/)?(?:month|mo)?", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var val = decimal.Parse(match.Groups[1].Value);
            return (val, val, currency, text);
        }
        return (null, null, currency, text);
    }

    private static List<string> SplitBullets(string text)
    {
        var parts = Regex.Split(text, @"(?:\r?\n|\u2022|\-)\s*")
            .Select(p => p.Trim())
            .Where(p => p.Length > 3)
            .ToList();
        return parts;
    }

    private static string[] ExtractSkills(string text)
    {
        var found = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (canonical, aliases) in JobsScraperSeedData.SkillCache)
        {
            if (aliases.Any(a => text.Contains(a, StringComparison.OrdinalIgnoreCase)))
                found.Add(canonical);
        }
        return found.OrderBy(s => s).Take(12).ToArray();
    }

    private static DateTime? ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        if (DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed))
            return parsed.ToUniversalTime();
        return null;
    }

    private static string Initials(string name)
    {
        var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
            return "NA";
        var initials = string.Concat(words.Take(2).Select(w => w[0])).ToUpperInvariant();
        return initials.Length > 10 ? initials[..10] : initials;
    }

    private static string Fingerprint(string company, string title, string location, string summary)
    {
        return Sha256($"{company}|{title}|{location}|{summary}");
    }

    private static string Sha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string? Get(JsonElement el, params string[] names)
    {
        foreach (var name in names)
        {
            if (el.ValueKind == JsonValueKind.Object && el.TryGetProperty(name, out var prop))
            {
                switch (prop.ValueKind)
                {
                    case JsonValueKind.String when !string.IsNullOrWhiteSpace(prop.GetString()):
                        return prop.GetString();
                    case JsonValueKind.Object:
                        // nested object — try "name" or "@id" or "url" keys
                        foreach (var nested in new[] { "name", "@id", "url", "value" })
                        {
                            if (prop.TryGetProperty(nested, out var inner) && inner.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(inner.GetString()))
                                return inner.GetString();
                        }
                        break;
                }
            }
        }
        return null;
    }

    private static List<JsonElement> ResolveJobList(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array)
            return root.EnumerateArray().Take(MaxJobsPerFetch).ToList();
        if (root.ValueKind != JsonValueKind.Object)
            return [];
        foreach (var key in new[] { "jobs", "results", "data", "items", "postings", "positions" })
        {
            if (root.TryGetProperty(key, out var arr) && arr.ValueKind == JsonValueKind.Array)
                return arr.EnumerateArray().Take(MaxJobsPerFetch).ToList();
        }
        if (root.TryGetProperty("jobs", out var single) && single.ValueKind == JsonValueKind.Object)
            return [single];
        return [];
    }

    private static string ExtractTitleFromUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "Untitled";
        var segment = url.TrimEnd('/').Split('/').LastOrDefault() ?? "untitled";
        segment = Regex.Replace(segment, @"[-_+]", " ");
        segment = Regex.Replace(segment, @"(\d{4,})", "").Trim();
        return string.IsNullOrWhiteSpace(segment) ? "Untitled" : char.ToUpperInvariant(segment[0]) + segment[1..];
    }
}