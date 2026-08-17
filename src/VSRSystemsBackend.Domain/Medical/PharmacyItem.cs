using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Medical;

public class PharmacyItem : AuditableEntity<string>
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

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    [MaxLength(500)]
    public string? Composition { get; set; }

    public int StockQuantity { get; set; } = 0;
    public int ReorderLevel { get; set; } = 10;
    public decimal UnitPrice { get; set; }
    public decimal SellingPrice { get; set; }

    [MaxLength(100)]
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}