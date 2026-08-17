using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.School.DTOs;

public class StudentDto
{
    public string Id { get; set; } = string.Empty;
    public string AdmissionNo { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ClassId { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string GuardianName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? RollNo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateStudentDto
{
    [Required]
    [MaxLength(50)]
    public string AdmissionNo { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string GuardianName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime? Dob { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? RollNo { get; set; }
}

public class UpdateStudentDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string ClassId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string GuardianName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public DateTime? Dob { get; set; }
    public string? Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? RollNo { get; set; }
}

public class SchoolClassDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int StudentCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateSchoolClassDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Section { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Teacher { get; set; } = string.Empty;

    public int Capacity { get; set; }
}

public class UpdateSchoolClassDto
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Section { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Teacher { get; set; } = string.Empty;

    public int Capacity { get; set; }
}

public class StaffDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? LastAttendance { get; set; }
    public DateTime? LastAttendanceDate { get; set; }
    public decimal? DailyRate { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public DateTime? JoinDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateStaffDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public string? LastAttendance { get; set; }
    public DateTime? LastAttendanceDate { get; set; }
    public decimal? DailyRate { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public DateTime? JoinDate { get; set; }
}

public class UpdateStaffDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public string? LastAttendance { get; set; }
    public DateTime? LastAttendanceDate { get; set; }
    public decimal? DailyRate { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public DateTime? JoinDate { get; set; }
}

public class ParentDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public List<string> ChildIds { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateParentDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public List<string> ChildIds { get; set; } = new();
    public string Status { get; set; } = "active";
}

public class UpdateParentDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public List<string> ChildIds { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}

public class SchoolProjectDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Incharge { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public decimal Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateSchoolProjectDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Incharge { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "planned";

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public decimal Budget { get; set; }
}

public class UpdateSchoolProjectDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Incharge { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public decimal Budget { get; set; }
}

public class StockItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Qty { get; set; }
    public string Unit { get; set; } = string.Empty;
    public int ReorderLevel { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateStockItemDto
{
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public int Qty { get; set; }

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    public int ReorderLevel { get; set; }
    public decimal UnitPrice { get; set; }
}

public class UpdateStockItemDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public int Qty { get; set; }

    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    public int ReorderLevel { get; set; }
    public decimal UnitPrice { get; set; }
}

public class AdmissionLeadDto
{
    public string Id { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string GuardianName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Stage { get; set; } = string.Empty;
    public DateTime? FollowUpDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateAdmissionLeadDto
{
    [Required]
    [MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string GuardianName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    [Required]
    [MaxLength(50)]
    public string Grade { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Source { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Stage { get; set; } = "lead";

    public DateTime? FollowUpDate { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAdmissionLeadDto
{
    [MaxLength(200)]
    public string StudentName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string GuardianName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Stage { get; set; } = string.Empty;
    public DateTime? FollowUpDate { get; set; }
    public string? Notes { get; set; }
}

public class AcademicSessionDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateAcademicSessionDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrent { get; set; }
}

public class SubjectDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ClassId { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string TeacherId { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateSubjectDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TeacherId { get; set; } = string.Empty;
}

public class TimetableSlotDto
{
    public string Id { get; set; } = string.Empty;
    public string ClassId { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string Day { get; set; } = string.Empty;
    public int Period { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTimetableSlotDto
{
    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Day { get; set; } = string.Empty;

    public int Period { get; set; }

    [Required]
    [MaxLength(100)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Teacher { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Room { get; set; } = string.Empty;
}

public class FeeRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PaidDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateFeeRecordDto
{
    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "pending";
}

public class ReceiptDto
{
    public string Id { get; set; } = string.Empty;
    public string ReceiptNo { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<string> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateReceiptDto
{
    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public List<string> Items { get; set; } = new();
}

public class PayrollRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string StaffId { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string Month { get; set; } = string.Empty;
    public decimal Basic { get; set; }
    public decimal Hra { get; set; }
    public decimal Allowances { get; set; }
    public decimal Deductions { get; set; }
    public decimal Net { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePayrollRecordDto
{
    [Required]
    [MaxLength(50)]
    public string StaffId { get; set; } = string.Empty;

    public string Month { get; set; } = string.Empty;
    public decimal Basic { get; set; }
    public decimal Hra { get; set; }
    public decimal Allowances { get; set; }
    public decimal Deductions { get; set; }
}