using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class Warehouse : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(100)]
    public string ContactPerson { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<LocationBin> Locations { get; set; } = new List<LocationBin>();
    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    public virtual ICollection<StockTransfer> TransfersFrom { get; set; } = new List<StockTransfer>();
    public virtual ICollection<StockTransfer> TransfersTo { get; set; } = new List<StockTransfer>();
    public virtual ICollection<StockCount> StockCounts { get; set; } = new List<StockCount>();
    public virtual ICollection<ProjectRecord> Projects { get; set; } = new List<ProjectRecord>();
}