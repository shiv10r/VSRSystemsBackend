using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Medical;

public class LabTest : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PatientId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DoctorId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string TestName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TestType { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "ordered";

    [MaxLength(5000)]
    public string? Result { get; set; }

    [MaxLength(500)]
    public string? ReferenceRange { get; set; }

    public decimal Cost { get; set; }

    public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}