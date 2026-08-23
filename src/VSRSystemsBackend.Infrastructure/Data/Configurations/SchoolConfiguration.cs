using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.School;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.AdmissionNo).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.ClassId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ClassName).HasMaxLength(100).IsRequired();
        builder.Property(s => s.GuardianName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Phone).HasMaxLength(20).IsRequired();
        builder.Property(s => s.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(s => s.Dob);
        builder.Property(s => s.Gender).HasMaxLength(10);
        builder.Property(s => s.BloodGroup).HasMaxLength(10);
        builder.Property(s => s.Email).HasMaxLength(200);
        builder.Property(s => s.Address).HasMaxLength(500);
        builder.Property(s => s.RollNo).HasMaxLength(20);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.AdmissionNo).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.ClassId);
        builder.HasIndex(s => s.Status);
    }
}

public class SchoolClassConfiguration : IEntityTypeConfiguration<SchoolClass>
{
    public void Configure(EntityTypeBuilder<SchoolClass> builder)
    {
        builder.ToTable("school_classes");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Section).HasMaxLength(10).IsRequired();
        builder.Property(c => c.Teacher).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Capacity).HasDefaultValue(0);
        builder.Property(c => c.StudentCount).HasDefaultValue(0);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Name);
    }
}

public class SchoolStaffMemberConfiguration : IEntityTypeConfiguration<SchoolStaffMember>
{
    public void Configure(EntityTypeBuilder<SchoolStaffMember> builder)
    {
        builder.ToTable("school_staff_members");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Role).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Phone).HasMaxLength(20).IsRequired();
        builder.Property(s => s.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(s => s.LastAttendance).HasMaxLength(20);
        builder.Property(s => s.LastAttendanceDate);
        builder.Property(s => s.DailyRate).HasColumnType("decimal(18,2)");
        builder.Property(s => s.Email).HasMaxLength(200);
        builder.Property(s => s.Department).HasMaxLength(100);
        builder.Property(s => s.JoinDate);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.Department);
        builder.HasIndex(s => s.Status);
    }
}

public class ParentRecordConfiguration : IEntityTypeConfiguration<ParentRecord>
{
    public void Configure(EntityTypeBuilder<ParentRecord> builder)
    {
        builder.ToTable("parent_records");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Phone).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(200);
        builder.Property(p => p.Occupation).HasMaxLength(100);
        builder.Property(p => p.Address).HasMaxLength(500);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.Property(p => p.ChildIds)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (left, right) => left != null && right != null && left.SequenceEqual(right),
                value => value.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
                value => value.ToList()));

        builder.HasIndex(p => p.Phone).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class SchoolProjectConfiguration : IEntityTypeConfiguration<SchoolProject>
{
    public void Configure(EntityTypeBuilder<SchoolProject> builder)
    {
        builder.ToTable("school_projects");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Incharge).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("planned");
        builder.Property(p => p.StartDate).HasDefaultValueSql("NOW()");
        builder.Property(p => p.Budget).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);
    }
}

public class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("school_stock_items");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.Sku).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Category).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Qty).HasDefaultValue(0);
        builder.Property(s => s.Unit).HasMaxLength(20).IsRequired();
        builder.Property(s => s.ReorderLevel).HasDefaultValue(0);
        builder.Property(s => s.UnitPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.Sku).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class AdmissionLeadConfiguration : IEntityTypeConfiguration<AdmissionLead>
{
    public void Configure(EntityTypeBuilder<AdmissionLead> builder)
    {
        builder.ToTable("admission_leads");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.StudentName).HasMaxLength(200).IsRequired();
        builder.Property(a => a.GuardianName).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Phone).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Email).HasMaxLength(200);
        builder.Property(a => a.Grade).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Source).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Stage).HasMaxLength(20).HasDefaultValue("lead");
        builder.Property(a => a.FollowUpDate);
        builder.Property(a => a.Notes).HasMaxLength(1000);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.Phone);
        builder.HasIndex(a => a.Stage);
        builder.HasIndex(a => a.FollowUpDate);
    }
}

public class AcademicSessionConfiguration : IEntityTypeConfiguration<AcademicSession>
{
    public void Configure(EntityTypeBuilder<AcademicSession> builder)
    {
        builder.ToTable("academic_sessions");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.Name).HasMaxLength(50).IsRequired();
        builder.Property(a => a.StartDate).IsRequired();
        builder.Property(a => a.EndDate).IsRequired();
        builder.Property(a => a.IsCurrent).HasDefaultValue(false);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.IsCurrent);
    }
}

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Code).HasMaxLength(20).IsRequired();
        builder.Property(s => s.ClassId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ClassName).HasMaxLength(100).IsRequired();
        builder.Property(s => s.TeacherId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.TeacherName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.ClassId);
        builder.HasIndex(s => s.TeacherId);
    }
}

public class TimetableSlotConfiguration : IEntityTypeConfiguration<TimetableSlot>
{
    public void Configure(EntityTypeBuilder<TimetableSlot> builder)
    {
        builder.ToTable("timetable_slots");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(t => t.ClassId).HasMaxLength(50).IsRequired();
        builder.Property(t => t.ClassName).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Day).HasMaxLength(10).IsRequired();
        builder.Property(t => t.Period).IsRequired();
        builder.Property(t => t.Subject).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Teacher).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Room).HasMaxLength(50).IsRequired();
        builder.Property(t => t.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(t => t.UpdatedAt);
        builder.Property(t => t.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(t => t.ClassId);
        builder.HasIndex(t => t.Day);
        builder.HasIndex(t => t.Teacher);
    }
}

public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
{
    public void Configure(EntityTypeBuilder<Homework> builder)
    {
        builder.ToTable("homework");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(h => h.ClassId).HasMaxLength(50).IsRequired();
        builder.Property(h => h.ClassName).HasMaxLength(100).IsRequired();
        builder.Property(h => h.Subject).HasMaxLength(100).IsRequired();
        builder.Property(h => h.Title).HasMaxLength(200).IsRequired();
        builder.Property(h => h.Description).HasMaxLength(1000);
        builder.Property(h => h.DueDate).IsRequired();
        builder.Property(h => h.Status).HasMaxLength(20).HasDefaultValue("draft");
        builder.Property(h => h.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(h => h.UpdatedAt);
        builder.Property(h => h.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(h => h.ClassId);
        builder.HasIndex(h => h.DueDate);
        builder.HasIndex(h => h.Status);
    }
}

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Subject).HasMaxLength(100).IsRequired();
        builder.Property(c => c.ClassId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.ClassName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.Status).HasMaxLength(20).HasDefaultValue("draft");
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.ClassId);
        builder.HasIndex(c => c.Status);
    }
}

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("lessons");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(l => l.CourseId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Title).HasMaxLength(200).IsRequired();
        builder.Property(l => l.ContentType).HasMaxLength(20).IsRequired();
        builder.Property(l => l.Content).HasMaxLength(5000);
        builder.Property(l => l.DurationMin);
        builder.Property(l => l.Order).IsRequired();
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt);
        builder.Property(l => l.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(l => l.CourseId);
        builder.HasIndex(l => l.Order);
    }
}

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("attendance_records");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.StudentId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.StudentName).HasMaxLength(200).IsRequired();
        builder.Property(a => a.ClassName).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Date).IsRequired();
        builder.Property(a => a.Status).HasMaxLength(20).IsRequired();
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.StudentId);
        builder.HasIndex(a => a.Date);
        builder.HasIndex(a => new { a.StudentId, a.Date }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("leave_requests");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(l => l.PersonType).HasMaxLength(10).IsRequired();
        builder.Property(l => l.PersonId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.PersonName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Kind).HasMaxLength(50).IsRequired();
        builder.Property(l => l.DateFrom).IsRequired();
        builder.Property(l => l.DateTo).IsRequired();
        builder.Property(l => l.Reason).HasMaxLength(500);
        builder.Property(l => l.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt);
        builder.Property(l => l.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(l => l.PersonId);
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.DateFrom);
    }
}

// Add more configurations for remaining school entities...
