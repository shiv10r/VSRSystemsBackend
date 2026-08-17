using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Medical;

public class Prescription : AuditableEntity<string>
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

    [MaxLength(50)]
    public string? AppointmentId { get; set; }

    [Required]
    [MaxLength(5000)]
    public string Medicines { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Instructions { get; set; }

    public int ValidDays { get; set; } = 30;

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}