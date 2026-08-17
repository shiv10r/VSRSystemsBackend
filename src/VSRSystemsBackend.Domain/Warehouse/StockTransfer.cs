using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class StockTransfer : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TransferNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string FromWarehouseId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ToWarehouseId { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "created"; // created, dispatched, received, completed

    public virtual ICollection<StockTransferLine> Items { get; set; } = new List<StockTransferLine>();

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(FromWarehouseId))]
    public virtual Warehouse FromWarehouse { get; set; } = null!;

    [ForeignKey(nameof(ToWarehouseId))]
    public virtual Warehouse ToWarehouse { get; set; } = null!;
}

public class StockTransferLine : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string StockTransferId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;

    [MaxLength(50)]
    public string? FromBin { get; set; }

    [MaxLength(50)]
    public string? ToBin { get; set; }

    // Navigation properties
    [ForeignKey(nameof(StockTransferId))]
    public virtual StockTransfer StockTransfer { get; set; } = null!;

    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}