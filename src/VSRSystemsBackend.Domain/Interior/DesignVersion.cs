using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Interior;

public class DesignVersion : BaseEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DesignId { get; set; } = string.Empty;

    public int Version { get; set; } = 0;

    [Required]
    [MaxLength(20)]
    public string Style { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Color { get; set; } = string.Empty;

    public decimal Budget { get; set; } = 0;

    [MaxLength(2000)]
    public string Prompt { get; set; } = string.Empty;

    public List<string> ProductIds { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}