using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Interior;

public class InteriorRoom : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string RoomType { get; set; } = string.Empty;

    public int Length { get; set; } = 0;
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;

    public decimal Budget { get; set; } = 0;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}