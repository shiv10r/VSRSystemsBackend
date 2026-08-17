using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class PurchaseOrder : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PoNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string SupplierId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string SupplierName { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public DateTime ExpectedDelivery { get; set; }

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "draft"; // draft, submitted, approved, partial, received, closed, cancelled

    public virtual ICollection<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; } = 0;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(SupplierId))]
    public virtual Supplier Supplier { get; set; } = null!;

    [ForeignKey(nameof(WarehouseId))]
    public virtual Warehouse Warehouse { get; set; } = null!;

    public virtual ICollection<GrnRecord> GrnRecords { get; set; } = new List<GrnRecord>();
}

public class PurchaseOrderLine : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string PurchaseOrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0;

    // Navigation properties
    [ForeignKey(nameof(PurchaseOrderId))]
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}