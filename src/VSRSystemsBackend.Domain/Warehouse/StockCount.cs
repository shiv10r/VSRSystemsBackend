using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class StockCount : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CountNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public virtual ICollection<StockCountLine> Items { get; set; } = new List<StockCountLine>();

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "open"; // open, approved

    // Navigation properties
    [ForeignKey(nameof(WarehouseId))]
    public virtual Warehouse Warehouse { get; set; } = null!;
}

public class StockCountLine : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string StockCountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int SystemQty { get; set; } = 0;
    public int PhysicalQty { get; set; } = 0;
    public int Difference { get; set; } = 0;

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(StockCountId))]
    public virtual StockCount StockCount { get; set; } = null!;

    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}