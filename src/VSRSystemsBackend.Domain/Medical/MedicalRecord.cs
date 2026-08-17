using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Medical;

public class MedicalRecord : AuditableEntity<string>
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
    [MaxLength(2000)]
    public string Diagnosis { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Symptoms { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Treatment { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public DateTime RecordDate { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}