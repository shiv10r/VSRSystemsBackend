using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class LeaveRequest : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string PersonType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PersonId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string PersonName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Kind { get; set; } = string.Empty;

    public DateTime DateFrom { get; set; } = DateTime.UtcNow;
    public DateTime DateTo { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}