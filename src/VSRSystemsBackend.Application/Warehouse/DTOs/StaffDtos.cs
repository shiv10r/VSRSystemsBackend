using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class StaffMemberDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? LastAttendance { get; set; }
    public DateTime? LastAttendanceDate { get; set; }
    public decimal? DailyRate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateStaffMemberDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [MaxLength(20)]
    public string? LastAttendance { get; set; }

    public DateTime? LastAttendanceDate { get; set; }

    public decimal? DailyRate { get; set; }
}

public class UpdateStaffMemberDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [MaxLength(20)]
    public string? LastAttendance { get; set; }

    public DateTime? LastAttendanceDate { get; set; }

    public decimal? DailyRate { get; set; }
}