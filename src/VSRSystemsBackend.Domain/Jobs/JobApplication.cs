using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public class JobApplication : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CandidateId { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string CoverLetter { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ResumeUrl { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "applied";

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}