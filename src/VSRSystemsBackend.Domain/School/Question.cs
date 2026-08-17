using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class Question : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ClassName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Difficulty { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Options { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Answer { get; set; } = string.Empty;

    public int Marks { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}