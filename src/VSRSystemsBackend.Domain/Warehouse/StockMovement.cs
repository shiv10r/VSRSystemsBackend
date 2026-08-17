using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class StockMovement : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Type { get; set; } = string.Empty; // GRN, adjustment, transfer_out, transfer_in, pick, return, stock_count, dispatch

    public int Qty { get; set; } = 0; // signed (+ in / - out)

    [MaxLength(200)]
    public string From { get; set; } = string.Empty;

    [MaxLength(200)]
    public string To { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(50)]
    public string RefNumber { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}

public class StockAdjustment : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    public int OldQty { get; set; } = 0;
    public int NewQty { get; set; } = 0;
    public int Difference { get; set; } = 0;

    [MaxLength(200)]
    public string Reason { get; set; } = string.Empty; // Damaged | Lost | Found | Counting error | Other

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}