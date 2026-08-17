using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Interior;

public class InteriorDesign : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RoomId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Style { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Color { get; set; } = string.Empty;

    public decimal Budget { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "generating";

    public bool Favorite { get; set; } = false;
    public bool Saved { get; set; } = false;

    public int CurrentVersion { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}