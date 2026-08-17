using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Medical;

public class Appointment : AuditableEntity<string>
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

    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }

    public int DurationMinutes { get; set; } = 30;

    [MaxLength(30)]
    public string Type { get; set; } = "consultation";

    [MaxLength(20)]
    public string Status { get; set; } = "scheduled";

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public decimal Fee { get; set; }

    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}