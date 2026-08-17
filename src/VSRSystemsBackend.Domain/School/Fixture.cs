using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class Fixture : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TeamId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string TeamName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Opponent { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    [Required]
    [MaxLength(200)]
    public string Venue { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Result { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}