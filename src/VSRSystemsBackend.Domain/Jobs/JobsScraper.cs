using System.ComponentModel.DataAnnotations;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public class JobSourceMapping : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobId { get; set; } = string.Empty;

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

    public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
    public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
    public bool IsPrimary { get; set; } = true;
    public bool IsActive { get; set; } = true;

    [MaxLength(64)]
    public string PayloadHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class DuplicateCandidate : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobIdA { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobIdB { get; set; } = string.Empty;

    public double Score { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    public DateTime? ResolvedAt { get; set; }

    [MaxLength(100)]
    public string? ResolvedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public class IngestionError : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? RawExternalJobId { get; set; }

    [MaxLength(50)]
    public string? JobSourceId { get; set; }

    [MaxLength(50)]
    public string ErrorCode { get; set; } = "Unknown";

    public string Message { get; set; } = string.Empty;

    public int RetryCount { get; set; }
    public DateTime? NextRetryAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}