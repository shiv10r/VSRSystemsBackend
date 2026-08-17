using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public class ScreeningQuestion : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobId { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "text";

    public bool IsRequired { get; set; } = true;

    public int Order { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}