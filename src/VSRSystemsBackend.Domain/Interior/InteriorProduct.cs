using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Interior;

public class InteriorProduct : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Category { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0;

    [MaxLength(100)]
    public string? Material { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }

    [MaxLength(50)]
    public string? Width { get; set; }

    [MaxLength(50)]
    public string? Depth { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}