using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class AdmissionLead : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string GuardianName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Email { get; set; }

    [Required]
    [MaxLength(50)]
    public string Grade { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Source { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Stage { get; set; } = "lead";

    public DateTime? FollowUpDate { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}