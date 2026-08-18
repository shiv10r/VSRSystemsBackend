using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class Professional : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    public DateTime? Dob { get; set; }

    [MaxLength(30)]
    public string OnboardingStatus { get; set; } = "draft"; // draft/submitted/verified/rejected/suspended

    public decimal QualityScore { get; set; } = 0;

    [MaxLength(20)]
    public string Tier { get; set; } = "bronze"; // bronze/silver/gold/platinum

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Phone { get; set; }

    [MaxLength(300)]
    public string? Email { get; set; }

    // Navigation
    public virtual ICollection<ProfessionalDocument> Documents { get; set; } = new List<ProfessionalDocument>();
    public virtual ICollection<ProfessionalSkill> Skills { get; set; } = new List<ProfessionalSkill>();
    public virtual ICollection<ProfessionalServiceArea> ServiceAreas { get; set; } = new List<ProfessionalServiceArea>();
    public virtual ICollection<ProfessionalAvailability> Availabilities { get; set; } = new List<ProfessionalAvailability>();
    public virtual ICollection<ProfessionalTimeOff> TimeOffs { get; set; } = new List<ProfessionalTimeOff>();
    public virtual ICollection<ProfessionalPerformance> Performances { get; set; } = new List<ProfessionalPerformance>();
}

public class ProfessionalDocument : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DocType { get; set; } = string.Empty; // id_proof/address_proof/police_verification/certification

    [MaxLength(1000)]
    public string FileUrl { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // pending/approved/rejected

    [MaxLength(50)]
    public string? ReviewedBy { get; set; }

    public DateTime? ReviewedAt { get; set; }

    [MaxLength(500)]
    public string? RejectionReason { get; set; }

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}

public class ProfessionalSkill : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [MaxLength(20)]
    public string SkillLevel { get; set; } = "standard"; // trainee/standard/expert

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}

public class ProfessionalServiceArea : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CityId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ZoneId { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;

    [ForeignKey(nameof(CityId))]
    public virtual City City { get; set; } = null!;

    [ForeignKey(nameof(ZoneId))]
    public virtual Zone Zone { get; set; } = null!;
}

public class ProfessionalAvailability : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    public int DayOfWeek { get; set; } = 0; // 0=Sunday .. 6=Saturday

    public TimeSpan StartTime { get; set; } = new(9, 0, 0);
    public TimeSpan EndTime { get; set; } = new(19, 0, 0);

    public bool IsRecurring { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}

public class ProfessionalTimeOff : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}

public class ProfessionalPerformance : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    public int JobsCompleted { get; set; } = 0;
    public int JobsCancelled { get; set; } = 0;
    public double AvgRating { get; set; } = 0;
    public double OnTimeRate { get; set; } = 0;
    public double AcceptanceRate { get; set; } = 0;

    // Navigation
    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;
}
