using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class Grievance : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public bool Anonymous { get; set; } = false;

    [Required]
    [MaxLength(200)]
    public string RaisedBy { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string Status { get; set; } = "open";

    [MaxLength(1000)]
    public string Resolution { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}