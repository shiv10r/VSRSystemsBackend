using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class HousePoint : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string HouseId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string HouseName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Event { get; set; } = string.Empty;

    public int Points { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(200)]
    public string AwardedTo { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}