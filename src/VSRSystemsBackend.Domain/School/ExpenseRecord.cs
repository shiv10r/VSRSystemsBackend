using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class ExpenseRecord : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [Required]
    [MaxLength(200)]
    public string PaidTo { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string Method { get; set; } = "cash";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}