using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Travel;

public class TravelPackage : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DestinationId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public int DurationDays { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountedPrice { get; set; }

    [MaxLength(3000)]
    public string? Inclusions { get; set; }

    [MaxLength(3000)]
    public string? Exclusions { get; set; }

    [MaxLength(10000)]
    public string? Itinerary { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public int MaxGroupSize { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}