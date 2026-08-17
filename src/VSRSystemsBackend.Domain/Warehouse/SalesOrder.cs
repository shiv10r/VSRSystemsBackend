using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class SalesOrder : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "created"; // created, confirmed, reserved, picking, packed, dispatched, completed, cancelled

    public virtual ICollection<SalesOrderLine> Lines { get; set; } = new List<SalesOrderLine>();

    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrandTotal { get; set; } = 0;

    [MaxLength(500)]
    public string DeliveryAddress { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(WarehouseId))]
    public virtual Warehouse Warehouse { get; set; } = null!;

    public virtual ICollection<PickList> PickLists { get; set; } = new List<PickList>();
    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
    public virtual ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();
}

public class SalesOrderLine : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string SalesOrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    public int TaxPct { get; set; } = 18;

    public int DiscountPct { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; } = 0;

    // Navigation properties
    [ForeignKey(nameof(SalesOrderId))]
    public virtual SalesOrder SalesOrder { get; set; } = null!;

    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}