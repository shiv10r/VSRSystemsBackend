using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Commerce;

public class Product : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BrandId { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public int StockQuantity { get; set; } = 0;
    public int LowStockThreshold { get; set; } = 10;
    public bool TrackInventory { get; set; } = true;
    public decimal? Weight { get; set; }
    [MaxLength(100)]
    public string? Dimensions { get; set; }
    [MaxLength(20)]
    public string Status { get; set; } = "active";
    public bool IsFeatured { get; set; } = false;
    [MaxLength(500)]
    public string? Tags { get; set; }
    [MaxLength(200)]
    public string? MetaTitle { get; set; }
    [MaxLength(500)]
    public string? MetaDescription { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}