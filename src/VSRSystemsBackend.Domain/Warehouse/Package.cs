using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class Package : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    public virtual ICollection<PackageItem> Items { get; set; } = new List<PackageItem>();

    [MaxLength(50)]
    public string TotalWeight { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Dimensions { get; set; } = string.Empty;

    public int PackageCount { get; set; } = 0;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // pending, packing, packed, ready

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(OrderId))]
    public virtual SalesOrder SalesOrder { get; set; } = null!;

    public virtual ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();
}

public class PackageItem : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;

    // Navigation properties
    [ForeignKey(nameof(PackageId))]
    public virtual Package Package { get; set; } = null!;
}