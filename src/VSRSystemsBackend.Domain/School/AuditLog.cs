using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class AuditLog : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(200)]
    public string User { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Action { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Entity { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Details { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}