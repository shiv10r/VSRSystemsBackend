using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class ReturnRecord : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ReturnNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = "customer"; // customer, supplier

    [Required]
    [MaxLength(200)]
    public string PartyName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OriginalRef { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ReturnLine> Items { get; set; } = new List<ReturnLine>();

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "requested"; // requested, received, inspected, completed

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;
}

public class ReturnLine : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ReturnRecordId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Condition { get; set; } = "good"; // good, damaged

    [Required]
    [MaxLength(30)]
    public string Action { get; set; } = "restock"; // restock, quarantine, return_to_supplier

    // Navigation properties
    [ForeignKey(nameof(ReturnRecordId))]
    public virtual ReturnRecord ReturnRecord { get; set; } = null!;

    [ForeignKey(nameof(ItemId))]
    public virtual InventoryItem Item { get; set; } = null!;
}