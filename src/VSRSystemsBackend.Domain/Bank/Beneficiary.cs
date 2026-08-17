using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Bank;

public class Beneficiary : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
    public string IfscCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string BankName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Branch { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "IMPS";

    public bool IsVerified { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}