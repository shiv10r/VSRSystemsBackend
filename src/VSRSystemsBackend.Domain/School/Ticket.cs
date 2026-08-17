using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class Ticket : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Priority { get; set; } = "medium";

    [MaxLength(20)]
    public string Status { get; set; } = "open";

    [Required]
    [MaxLength(200)]
    public string Assignee { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Requester { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string Sla { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}