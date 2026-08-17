using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class PTMSession : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TeacherId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string TeacherName { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string TimeSlot { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Room { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "open";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}