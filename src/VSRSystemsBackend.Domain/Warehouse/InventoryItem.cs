using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class InventoryItem : AuditableEntity<string>
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

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty; // bag, pcs, can, coil, box, etc.

    public int Qty { get; set; } = 0; // on hand
    public int Reserved { get; set; } = 0;
    public int Damaged { get; set; } = 0;
    public int Quarantine { get; set; } = 0;
    public int InTransit { get; set; } = 0;

    public int ReorderLevel { get; set; } = 0;
    public int MinStock { get; set; } = 0;
    public int MaxStock { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; } = 0; // purchase price

    [Column(TypeName = "decimal(18,2)")]
    public decimal SellingPrice { get; set; } = 0;

    [MaxLength(20)]
    public string Hsn { get; set; } = string.Empty;

    public int GstPct { get; set; } = 18;

    [MaxLength(100)]
    public string? Barcode { get; set; }

    [MaxLength(50)]
    public string? Weight { get; set; }

    [MaxLength(100)]
    public string? Dimensions { get; set; }

    [MaxLength(50)]
    public string Location { get; set; } = string.Empty; // default bin code

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public bool TrackBatch { get; set; } = false;
    public bool TrackSerial { get; set; } = false;
    public bool TrackExpiry { get; set; } = false;

    // Navigation properties
    [ForeignKey(nameof(WarehouseId))]
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
    public virtual ICollection<GrnLine> GrnLines { get; set; } = new List<GrnLine>();
    public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();
    public virtual ICollection<StockTransferLine> TransferLines { get; set; } = new List<StockTransferLine>();
    public virtual ICollection<PickLine> PickLines { get; set; } = new List<PickLine>();
    public virtual ICollection<ReturnLine> ReturnLines { get; set; } = new List<ReturnLine>();
    public virtual ICollection<StockCountLine> StockCountLines { get; set; } = new List<StockCountLine>();
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    // Computed properties (not mapped to DB)
    [NotMapped]
    public int AvailableQty => Qty - Reserved;

    [NotMapped]
    public string StockStatus => Qty <= 0 ? "out_of_stock" : (Qty <= ReorderLevel ? "low_stock" : "in_stock");
}