using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Bank;

public class Card : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(19)]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime ExpiryDate { get; set; }

    [MaxLength(4)]
    public string Cvv { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CardHolderName { get; set; } = string.Empty;

    public decimal Limit { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}