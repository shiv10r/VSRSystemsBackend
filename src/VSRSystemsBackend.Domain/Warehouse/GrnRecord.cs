using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class GrnRecord : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string GrnNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PoId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PoNumber { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public virtual ICollection<GrnLine> Lines { get; set; } = new List<GrnLine>();

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(PoId))]
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}

public class GrnLine : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string GrnRecordId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int OrderedQty { get; set; } = 0;
    public int ReceivedQty { get; set; } = 0;
    public int DamagedQty { get; set; } = 0;
    public int RejectedQty { get; set; } = 0;
    public int AcceptedQty { get; set; } = 0;

    public virtual ICollection<PutawayBin> Putaway { get; set; } = new List<PutawayBin>();

    // Navigation properties
    [ForeignKey(nameof(GrnRecordId))]
    public virtual GrnRecord GrnRecord { get; set; } = null!;

    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}

public class PutawayBin : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    public int GrnLineId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;

    // Navigation properties
    [ForeignKey(nameof(GrnLineId))]
    public virtual GrnLine GrnLine { get; set; } = null!;
}