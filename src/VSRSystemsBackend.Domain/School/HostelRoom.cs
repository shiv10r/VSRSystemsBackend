using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class HostelRoom : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Hostel { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Number { get; set; } = string.Empty;

    public int Capacity { get; set; }
    public int Occupants { get; set; }

    [MaxLength(20)]
    public string Type { get; set; } = "dorm";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}