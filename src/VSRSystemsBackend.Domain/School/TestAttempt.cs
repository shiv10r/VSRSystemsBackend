using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class TestAttempt : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ExamId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ExamName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;

    public int Score { get; set; } = 0;

    public int TotalMarks { get; set; } = 0;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}