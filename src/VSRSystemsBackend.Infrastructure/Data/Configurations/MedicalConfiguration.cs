using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Medical;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.PatientNumber).HasMaxLength(50).IsRequired();
        builder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(p => p.LastName).HasMaxLength(100).IsRequired();
        builder.Property(p => p.DateOfBirth).IsRequired();
        builder.Property(p => p.Gender).HasMaxLength(10).IsRequired();
        builder.Property(p => p.BloodGroup).HasMaxLength(10);
        builder.Property(p => p.Phone).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(200);
        builder.Property(p => p.Address).HasMaxLength(500);
        builder.Property(p => p.EmergencyContactName).HasMaxLength(200);
        builder.Property(p => p.EmergencyContactPhone).HasMaxLength(20);
        builder.Property(p => p.MedicalHistory).HasMaxLength(5000);
        builder.Property(p => p.Allergies).HasMaxLength(2000);
        builder.Property(p => p.InsuranceProvider).HasMaxLength(200);
        builder.Property(p => p.InsurancePolicyNumber).HasMaxLength(100);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.PatientNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.Phone);
        builder.HasIndex(p => p.Email);
    }
}

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("doctors");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(d => d.LastName).HasMaxLength(100).IsRequired();
        builder.Property(d => d.Specialization).HasMaxLength(100).IsRequired();
        builder.Property(d => d.LicenseNumber).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Phone).HasMaxLength(20).IsRequired();
        builder.Property(d => d.Email).HasMaxLength(200);
        builder.Property(d => d.Qualification).HasMaxLength(500);
        builder.Property(d => d.ExperienceYears).HasDefaultValue(0);
        builder.Property(d => d.ConsultationFee).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(d => d.AvailableDays).HasMaxLength(200);
        builder.Property(d => d.AvailableTimeFrom);
        builder.Property(d => d.AvailableTimeTo);
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => d.LicenseNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(d => d.Specialization);
        builder.HasIndex(d => d.Status);
    }
}

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.PatientId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.DoctorId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.AppointmentDate).IsRequired();
        builder.Property(a => a.AppointmentTime).IsRequired();
        builder.Property(a => a.DurationMinutes).HasDefaultValue(30);
        builder.Property(a => a.Type).HasMaxLength(30).HasDefaultValue("consultation");
        builder.Property(a => a.Status).HasMaxLength(20).HasDefaultValue("scheduled");
        builder.Property(a => a.Notes).HasMaxLength(2000);
        builder.Property(a => a.Fee).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(a => a.PaymentStatus).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.PatientId);
        builder.HasIndex(a => a.DoctorId);
        builder.HasIndex(a => a.AppointmentDate);
        builder.HasIndex(a => a.Status);
    }
}

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.ToTable("medical_records");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(m => m.PatientId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.DoctorId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.AppointmentId).HasMaxLength(50);
        builder.Property(m => m.Diagnosis).HasMaxLength(2000);
        builder.Property(m => m.Symptoms).HasMaxLength(2000);
        builder.Property(m => m.Treatment).HasMaxLength(3000);
        builder.Property(m => m.Notes).HasMaxLength(2000);
        builder.Property(m => m.RecordDate).HasDefaultValueSql("NOW()");
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(m => m.PatientId);
        builder.HasIndex(m => m.DoctorId);
        builder.HasIndex(m => m.RecordDate);
    }
}

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("prescriptions");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.PatientId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.DoctorId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.AppointmentId).HasMaxLength(50);
        builder.Property(p => p.Medicines).HasMaxLength(5000).IsRequired();
        builder.Property(p => p.Instructions).HasMaxLength(2000);
        builder.Property(p => p.ValidDays).HasDefaultValue(30);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.PatientId);
        builder.HasIndex(p => p.DoctorId);
        builder.HasIndex(p => p.Status);
    }
}

public class PharmacyItemConfiguration : IEntityTypeConfiguration<PharmacyItem>
{
    public void Configure(EntityTypeBuilder<PharmacyItem> builder)
    {
        builder.ToTable("pharmacy_items");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Sku).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Category).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Manufacturer).HasMaxLength(200);
        builder.Property(p => p.Composition).HasMaxLength(500);
        builder.Property(p => p.StockQuantity).HasDefaultValue(0);
        builder.Property(p => p.ReorderLevel).HasDefaultValue(10);
        builder.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.SellingPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.BatchNumber).HasMaxLength(100);
        builder.Property(p => p.ExpiryDate);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Sku).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.Category);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.ExpiryDate);
    }
}

public class LabTestConfiguration : IEntityTypeConfiguration<LabTest>
{
    public void Configure(EntityTypeBuilder<LabTest> builder)
    {
        builder.ToTable("lab_tests");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(l => l.PatientId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.DoctorId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.TestName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.TestType).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Status).HasMaxLength(20).HasDefaultValue("ordered");
        builder.Property(l => l.Result).HasMaxLength(5000);
        builder.Property(l => l.ReferenceRange).HasMaxLength(500);
        builder.Property(l => l.Cost).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(l => l.OrderedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.CompletedAt);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt);
        builder.Property(l => l.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(l => l.PatientId);
        builder.HasIndex(l => l.DoctorId);
        builder.HasIndex(l => l.Status);
    }
}

public class MedicalBillingConfiguration : IEntityTypeConfiguration<MedicalBilling>
{
    public void Configure(EntityTypeBuilder<MedicalBilling> builder)
    {
        builder.ToTable("medical_billings");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.PatientId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.InvoiceNumber).HasMaxLength(50).IsRequired();
        builder.Property(b => b.Items).HasMaxLength(5000).IsRequired();
        builder.Property(b => b.SubTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.TaxAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.DiscountAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.PaidAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.BalanceAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(b => b.PaymentMethod).HasMaxLength(30);
        builder.Property(b => b.BillingDate).HasDefaultValueSql("NOW()");
        builder.Property(b => b.DueDate);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.InvoiceNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(b => b.PatientId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.DueDate);
    }
}