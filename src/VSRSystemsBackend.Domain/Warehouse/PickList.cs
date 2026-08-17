using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class PickList : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PickNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // pending, picking, picked

    public virtual ICollection<PickLine> Items { get; set; } = new List<PickLine>();

    // Navigation properties
    [ForeignKey(nameof(OrderId))]
    public virtual SalesOrder SalesOrder { get; set; } = null!;
}

public class PickLine : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string PickListId { get; set; } = string.Empty;

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

    public int RequiredQty { get; set; } = 0;
    public int PickedQty { get; set; } = 0;

    // Navigation properties
    [ForeignKey(nameof(PickListId))]
    public virtual PickList PickList { get; set; } = null!;

    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}