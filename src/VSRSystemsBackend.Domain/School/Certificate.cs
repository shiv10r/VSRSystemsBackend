using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class Certificate : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TemplateId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string TemplateName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string Number { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "issued";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}