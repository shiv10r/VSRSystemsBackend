using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Jobs;

public class Company : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Website { get; set; }

    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    [MaxLength(30)]
    public string? Size { get; set; }

    [MaxLength(100)]
    public string? Industry { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}