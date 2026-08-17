using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class LocationBin : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty; // e.g. A01-02

    [MaxLength(20)]
    public string Zone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Rack { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Bin { get; set; } = string.Empty;

    public int Capacity { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    [ForeignKey(nameof(WarehouseId))]
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}