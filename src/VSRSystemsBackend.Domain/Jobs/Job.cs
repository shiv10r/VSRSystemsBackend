using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public class Job : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CompanyId { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Requirements { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "full-time";

    [MaxLength(30)]
    public string ExperienceLevel { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    public bool IsRemote { get; set; } = false;

    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    [MaxLength(3)]
    public string SalaryCurrency { get; set; } = "INR";

    [MaxLength(20)]
    public string Status { get; set; } = "draft";

    public DateTime? PublishedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    // Aggregated / scraper fields (mirrors frontend JobListing contract)
    [MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(10)]
    public string CompanyInitials { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Industry { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? State { get; set; }

    [MaxLength(50)]
    public string Country { get; set; } = "India";

    [MaxLength(30)]
    public string ExperienceText { get; set; } = string.Empty;

    public int MinExperience { get; set; }
    public int MaxExperience { get; set; }

    [MaxLength(100)]
    public string SalaryText { get; set; } = string.Empty;

    public bool SalaryVisible { get; set; }

    [MaxLength(20)]
    public string WorkMode { get; set; } = "On-site";

    [MaxLength(20)]
    public string EmploymentType { get; set; } = "Full-time";

    [MaxLength(500)]
    public string Summary { get; set; } = string.Empty;

    public string SkillsJson { get; set; } = "[]";
    public string ResponsibilitiesJson { get; set; } = "[]";
    public string BenefitsJson { get; set; } = "[]";

    [MaxLength(20)]
    public string ApplicationMode { get; set; } = "EasyApply";

    [MaxLength(1000)]
    public string? ExternalApplyUrl { get; set; }

    [MaxLength(1000)]
    public string? OriginalSourceUrl { get; set; }

    [MaxLength(30)]
    public string? SourceType { get; set; }

    public bool IsAggregated { get; set; }
    public bool Featured { get; set; }
    public bool Verified { get; set; }

    [MaxLength(200)]
    public string? ExternalJobId { get; set; }

    [MaxLength(50)]
    public string? PrimaryJobSourceId { get; set; }

    [MaxLength(100)]
    public string? PostedAtSource { get; set; }

    public DateTime? LastSeenAtSource { get; set; }

    [MaxLength(64)]
    public string CanonicalFingerprint { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}