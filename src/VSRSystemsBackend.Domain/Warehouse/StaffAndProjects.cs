using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Warehouse;

public class StaffMember : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [MaxLength(20)]
    public string? LastAttendance { get; set; } // present, absent

    public DateTime? LastAttendanceDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DailyRate { get; set; }

    // Navigation properties
    public virtual ICollection<ProjectAttendance> ProjectAttendances { get; set; } = new List<ProjectAttendance>();
}

public class ProjectRecord : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Client { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "planned"; // planned, active, completed

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; } = 0;

    [MaxLength(500)]
    public string? Address { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(50)]
    public string? WarehouseId { get; set; }

    // Navigation properties
    [ForeignKey(nameof(WarehouseId))]
    public virtual Warehouse? Warehouse { get; set; }

    public virtual ICollection<ProjectAttendance> Attendances { get; set; } = new List<ProjectAttendance>();
    public virtual ICollection<ProjectLog> Logs { get; set; } = new List<ProjectLog>();
    public virtual ICollection<ProjectTransaction> Transactions { get; set; } = new List<ProjectTransaction>();
    public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    public virtual ICollection<ProjectParty> Parties { get; set; } = new List<ProjectParty>();
}

public class ProjectAttendance : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string StaffId { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "present"; // present, absent

    // Navigation properties
    [ForeignKey(nameof(ProjectId))]
    public virtual ProjectRecord Project { get; set; } = null!;

    [ForeignKey(nameof(StaffId))]
    public virtual StaffMember Staff { get; set; } = null!;
}

public class ProjectLog : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [ForeignKey(nameof(ProjectId))]
    public virtual ProjectRecord Project { get; set; } = null!;
}

public class ProjectTransaction : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Type { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; } = 0;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ProjectId))]
    public virtual ProjectRecord Project { get; set; } = null!;
}

public class ProjectTask : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    public DateTime? DueDate { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public virtual ProjectRecord Project { get; set; } = null!;
}

public class ProjectParty : BaseEntity<int>
{
    [Key]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public virtual ProjectRecord Project { get; set; } = null!;
}