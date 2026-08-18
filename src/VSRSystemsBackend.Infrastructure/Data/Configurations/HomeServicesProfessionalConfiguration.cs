using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
{
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.ToTable("professionals");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.UserId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.DisplayName).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Gender).HasMaxLength(20);
        builder.Property(p => p.OnboardingStatus).HasMaxLength(30).HasDefaultValue("draft");
        builder.Property(p => p.QualityScore).HasColumnType("double precision").HasDefaultValue(0);
        builder.Property(p => p.Tier).HasMaxLength(20).HasDefaultValue("bronze");
        builder.Property(p => p.Phone).HasMaxLength(500);
        builder.Property(p => p.Email).HasMaxLength(300);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.UserId).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.OnboardingStatus);
        builder.HasIndex(p => p.Tier);

        builder.HasMany(p => p.Documents)
            .WithOne(d => d.Professional)
            .HasForeignKey(d => d.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Skills)
            .WithOne(s => s.Professional)
            .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ServiceAreas)
            .WithOne(a => a.Professional)
            .HasForeignKey(a => a.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Availabilities)
            .WithOne(a => a.Professional)
            .HasForeignKey(a => a.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.TimeOffs)
            .WithOne(t => t.Professional)
            .HasForeignKey(t => t.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Performances)
            .WithOne(per => per.Professional)
            .HasForeignKey(per => per.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProfessionalDocumentConfiguration : IEntityTypeConfiguration<ProfessionalDocument>
{
    public void Configure(EntityTypeBuilder<ProfessionalDocument> builder)
    {
        builder.ToTable("professional_documents");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.DocType).HasMaxLength(50).IsRequired();
        builder.Property(d => d.FileUrl).HasMaxLength(1000);
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(d => d.ReviewedBy).HasMaxLength(50);
        builder.Property(d => d.RejectionReason).HasMaxLength(500);
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => new { d.ProfessionalId, d.DocType });
        builder.HasIndex(d => d.Status);
    }
}

public class ProfessionalSkillConfiguration : IEntityTypeConfiguration<ProfessionalSkill>
{
    public void Configure(EntityTypeBuilder<ProfessionalSkill> builder)
    {
        builder.ToTable("professional_skills");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.SkillLevel).HasMaxLength(20).HasDefaultValue("standard");
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => new { s.ProfessionalId, s.ServiceId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.ServiceId);
    }
}

public class ProfessionalServiceAreaConfiguration : IEntityTypeConfiguration<ProfessionalServiceArea>
{
    public void Configure(EntityTypeBuilder<ProfessionalServiceArea> builder)
    {
        builder.ToTable("professional_service_areas");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.CityId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.ZoneId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.IsActive).HasDefaultValue(true);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => new { a.ProfessionalId, a.CityId, a.ZoneId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(a => a.CityId);
        builder.HasIndex(a => a.ZoneId);
    }
}

public class ProfessionalAvailabilityConfiguration : IEntityTypeConfiguration<ProfessionalAvailability>
{
    public void Configure(EntityTypeBuilder<ProfessionalAvailability> builder)
    {
        builder.ToTable("professional_availabilities");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.DayOfWeek).HasDefaultValue(0);
        builder.Property(a => a.StartTime).HasDefaultValue(new TimeSpan(9, 0, 0));
        builder.Property(a => a.EndTime).HasDefaultValue(new TimeSpan(19, 0, 0));
        builder.Property(a => a.IsRecurring).HasDefaultValue(true);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.ProfessionalId);
        builder.HasIndex(a => new { a.DayOfWeek, a.IsRecurring });
    }
}

public class ProfessionalTimeOffConfiguration : IEntityTypeConfiguration<ProfessionalTimeOff>
{
    public void Configure(EntityTypeBuilder<ProfessionalTimeOff> builder)
    {
        builder.ToTable("professional_time_offs");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(t => t.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Reason).HasMaxLength(500);
        builder.Property(t => t.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(t => t.UpdatedAt);
        builder.Property(t => t.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(t => t.ProfessionalId);
        builder.HasIndex(t => t.StartAt);
    }
}

public class ProfessionalPerformanceConfiguration : IEntityTypeConfiguration<ProfessionalPerformance>
{
    public void Configure(EntityTypeBuilder<ProfessionalPerformance> builder)
    {
        builder.ToTable("professional_performances");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.PeriodStart).IsRequired();
        builder.Property(p => p.PeriodEnd).IsRequired();
        builder.Property(p => p.JobsCompleted).HasDefaultValue(0);
        builder.Property(p => p.JobsCancelled).HasDefaultValue(0);
        builder.Property(p => p.AvgRating).HasColumnType("double precision").HasDefaultValue(0);
        builder.Property(p => p.OnTimeRate).HasColumnType("double precision").HasDefaultValue(0);
        builder.Property(p => p.AcceptanceRate).HasColumnType("double precision").HasDefaultValue(0);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => new { p.ProfessionalId, p.PeriodStart, p.PeriodEnd }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}
