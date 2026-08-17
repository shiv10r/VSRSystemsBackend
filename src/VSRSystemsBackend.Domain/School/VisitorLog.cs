using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class VisitorLog : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Purpose { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string PersonToMeet { get; set; } = string.Empty;

    public DateTime InTime { get; set; } = DateTime.UtcNow;

    public DateTime? OutTime { get; set; }

    [Required]
    [MaxLength(50)]
    public string Badge { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}