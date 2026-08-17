using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Hotel;

public class Room : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Number { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = "Standard";

    [MaxLength(20)]
    public string Status { get; set; } = "vacant-clean";

    public decimal Rate { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}