using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class ResultRecord : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ExamId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ExamName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ClassName { get; set; } = string.Empty;

    public int Total { get; set; }
    public int MaxTotal { get; set; }
    public decimal Percentage { get; set; }

    [MaxLength(20)]
    public string Grade { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}