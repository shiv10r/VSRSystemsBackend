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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}