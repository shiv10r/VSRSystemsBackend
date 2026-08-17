using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Bank;

public class Deposit : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public decimal MaturityAmount { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime StartDate { get; set; }
    public DateTime MaturityDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}