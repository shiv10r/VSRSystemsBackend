using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class Dispatch : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DispatchNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Transporter { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Courier { get; set; } = string.Empty;

    [MaxLength(100)]
    public string TrackingNumber { get; set; } = string.Empty;

    public DateTime DispatchDate { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string VehicleNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Driver { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "ready"; // ready, dispatched, completed

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey(nameof(OrderId))]
    public virtual SalesOrder SalesOrder { get; set; } = null!;

    [ForeignKey(nameof(PackageId))]
    public virtual Package Package { get; set; } = null!;
}