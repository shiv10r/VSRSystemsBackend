using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class PerformanceReview : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string StaffId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string StaffName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Period { get; set; } = string.Empty;

    public int Rating { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}