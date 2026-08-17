using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class IncidentRecord : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Severity { get; set; } = "low";

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Actions { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "reported";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}