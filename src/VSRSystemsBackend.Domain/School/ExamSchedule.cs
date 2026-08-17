using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class ExamSchedule : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ClassName { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Time { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Subject { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "scheduled";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}