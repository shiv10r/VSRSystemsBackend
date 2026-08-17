using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class SchoolPurchaseOrder : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string VendorId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string VendorName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Items { get; set; } = string.Empty;

    public decimal Total { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "draft";

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}