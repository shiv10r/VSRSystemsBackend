using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Hotel;

public class HousekeepingTask : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Assignee { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Task { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Priority { get; set; } = "normal";

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    public DateTime Scheduled { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}