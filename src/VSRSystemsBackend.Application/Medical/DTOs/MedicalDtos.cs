using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Medical.DTOs;

public class PatientDto
{
    public string Id { get; set; } = string.Empty;
    public string PatientNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string? BloodGroup { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalHistory { get; set; }
    public string? Allergies { get; set; }
    public string? InsuranceProvider { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePatientDto
{
    [Required]
    [MaxLength(50)]
    public string PatientNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    [Required]
    [MaxLength(10)]
    public string Gender { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? BloodGroup { get; set; }

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalHistory { get; set; }
    public string? Allergies { get; set; }
    public string? InsuranceProvider { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    [MaxLength(20)]
    public string Status { get; set; } = "active";
}

public class UpdatePatientDto
{
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(10)]
    public string Gender { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? BloodGroup { get; set; }

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalHistory { get; set; }
    public string? Allergies { get; set; }
    public string? InsuranceProvider { get; set; }
    public string? InsurancePolicyNumber { get; set; }
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class DoctorDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Qualification { get; set; }
    public int ExperienceYears { get; set; }
    public decimal ConsultationFee { get; set; }
    public string? AvailableDays { get; set; }
    public TimeSpan? AvailableTimeFrom { get; set; }
    public TimeSpan? AvailableTimeTo { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDoctorDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Qualification { get; set; }
    public int ExperienceYears { get; set; }
    public decimal ConsultationFee { get; set; }
    [MaxLength(200)]
    public string? AvailableDays { get; set; }
    public TimeSpan? AvailableTimeFrom { get; set; }
    public TimeSpan? AvailableTimeTo { get; set; }
    [MaxLength(20)]
    public string Status { get; set; } = "active";
}

public class UpdateDoctorDto
{
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;

    [MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Qualification { get; set; }
    public int ExperienceYears { get; set; }
    public decimal ConsultationFee { get; set; }
    [MaxLength(200)]
    public string? AvailableDays { get; set; }
    public TimeSpan? AvailableTimeFrom { get; set; }
    public TimeSpan? AvailableTimeTo { get; set; }
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class AppointmentDto
{
    public string Id { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string DoctorId { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public int DurationMinutes { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public decimal Fee { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateAppointmentDto
{
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
}

public class UpdateAppointmentDto
{
    [MaxLength(50)]
    public string PatientId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string DoctorId { get; set; } = string.Empty;

    public DateTime? AppointmentDate { get; set; }
    public TimeSpan? AppointmentTime { get; set; }

    public int DurationMinutes { get; set; }

    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public decimal Fee { get; set; }
    [MaxLength(20)]
    public string PaymentStatus { get; set; } = string.Empty;
}

public class MedicalRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string DoctorId { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string? AppointmentId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateMedicalRecordDto
{
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
}

public class UpdateMedicalRecordDto
{
    [MaxLength(2000)]
    public string Diagnosis { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Symptoms { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Treatment { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Notes { get; set; }
}

public class PrescriptionDto
{
    public string Id { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string DoctorId { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string? AppointmentId { get; set; }
    public string Medicines { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public int ValidDays { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePrescriptionDto
{
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
}

public class UpdatePrescriptionDto
{
    [MaxLength(5000)]
    public string Medicines { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Instructions { get; set; }

    public int ValidDays { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class PharmacyItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Manufacturer { get; set; }
    public string? Composition { get; set; }
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePharmacyItemDto
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

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    [MaxLength(500)]
    public string? Composition { get; set; }

    public int StockQuantity { get; set; } = 0;
    public int ReorderLevel { get; set; } = 10;
    public decimal UnitPrice { get; set; }
    public decimal SellingPrice { get; set; }
    [MaxLength(100)]
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    [MaxLength(20)]
    public string Status { get; set; } = "active";
}

public class UpdatePharmacyItemDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    [MaxLength(500)]
    public string? Composition { get; set; }

    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SellingPrice { get; set; }
    [MaxLength(100)]
    public string? BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class LabTestDto
{
    public string Id { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string DoctorId { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public string TestType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Result { get; set; }
    public string? ReferenceRange { get; set; }
    public decimal Cost { get; set; }
    public DateTime OrderedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateLabTestDto
{
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
}

public class UpdateLabTestDto
{
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string? Result { get; set; }

    [MaxLength(500)]
    public string? ReferenceRange { get; set; }

    public decimal Cost { get; set; }
}

public class MedicalBillingDto
{
    public string Id { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public string Items { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public DateTime BillingDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateMedicalBillingDto
{
    [Required]
    [MaxLength(50)]
    public string PatientId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Items { get; set; } = string.Empty;

    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    [MaxLength(30)]
    public string? PaymentMethod { get; set; }

    public DateTime? DueDate { get; set; }
}

public class UpdateMedicalBillingDto
{
    [MaxLength(5000)]
    public string Items { get; set; } = string.Empty;

    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? PaymentMethod { get; set; }

    public DateTime? DueDate { get; set; }
}