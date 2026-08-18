using System.ComponentModel.DataAnnotations;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public class RawExternalJob : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobSourceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ExternalJobId { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? SourceUrl { get; set; }

    [MaxLength(1000)]
    public string? ApplyUrl { get; set; }

    [MaxLength(500)]
    public string? RawTitle { get; set; }

    [MaxLength(300)]
    public string? RawCompany { get; set; }

    [MaxLength(300)]
    public string? RawLocation { get; set; }

    public string? RawDescription { get; set; }

    [MaxLength(500)]
    public string? RawSalary { get; set; }

    [MaxLength(100)]
    public string? RawPostedDate { get; set; }

    [MaxLength(50)]
    public string? RawEmploymentType { get; set; }

    [MaxLength(50)]
    public string? RawWorkMode { get; set; }

    [MaxLength(2000)]
    public string? RawSkills { get; set; }

    [MaxLength(200)]
    public string? RawIndustry { get; set; }

    [MaxLength(64)]
    public string PayloadHash { get; set; } = string.Empty;

    public string? RawPayload { get; set; }

    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
    public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string ProcessingStatus { get; set; } = JobProcessingStatus.New;

    [MaxLength(2000)]
    public string? ProcessingError { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}