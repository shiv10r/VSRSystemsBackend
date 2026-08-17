using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("jobs");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(j => j.Title).HasMaxLength(200).IsRequired();
        builder.Property(j => j.Slug).HasMaxLength(220).IsRequired();
        builder.Property(j => j.CompanyId).HasMaxLength(50).IsRequired();
        builder.Property(j => j.Description).HasMaxLength(5000).IsRequired();
        builder.Property(j => j.Requirements).HasMaxLength(3000);
        builder.Property(j => j.Category).HasMaxLength(50).IsRequired();
        builder.Property(j => j.Type).HasMaxLength(20).HasDefaultValue("full-time");
        builder.Property(j => j.ExperienceLevel).HasMaxLength(30);
        builder.Property(j => j.Location).HasMaxLength(200);
        builder.Property(j => j.IsRemote).HasDefaultValue(false);
        builder.Property(j => j.SalaryMin).HasColumnType("decimal(18,2)");
        builder.Property(j => j.SalaryMax).HasColumnType("decimal(18,2)");
        builder.Property(j => j.SalaryCurrency).HasMaxLength(3).HasDefaultValue("INR");
        builder.Property(j => j.Status).HasMaxLength(20).HasDefaultValue("draft");
        builder.Property(j => j.PublishedAt);
        builder.Property(j => j.ExpiresAt);
        builder.Property(j => j.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(j => j.UpdatedAt);
        builder.Property(j => j.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(j => j.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(j => j.CompanyId);
        builder.HasIndex(j => j.Category);
        builder.HasIndex(j => j.Status);
        builder.HasIndex(j => j.PublishedAt);
    }
}

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(220).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(3000);
        builder.Property(c => c.Website).HasMaxLength(500);
        builder.Property(c => c.LogoUrl).HasMaxLength(500);
        builder.Property(c => c.Size).HasMaxLength(30);
        builder.Property(c => c.Industry).HasMaxLength(100);
        builder.Property(c => c.Location).HasMaxLength(200);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.IsActive);
    }
}

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("job_applications");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.JobId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.CandidateId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.CoverLetter).HasMaxLength(3000);
        builder.Property(a => a.ResumeUrl).HasMaxLength(500);
        builder.Property(a => a.Status).HasMaxLength(20).HasDefaultValue("applied");
        builder.Property(a => a.AppliedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => new { a.JobId, a.CandidateId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(a => a.JobId);
        builder.HasIndex(a => a.CandidateId);
        builder.HasIndex(a => a.Status);
    }
}

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("candidates");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Phone).HasMaxLength(20).IsRequired();
        builder.Property(c => c.ResumeUrl).HasMaxLength(500);
        builder.Property(c => c.LinkedInUrl).HasMaxLength(500);
        builder.Property(c => c.PortfolioUrl).HasMaxLength(500);
        builder.Property(c => c.Skills).HasMaxLength(2000);
        builder.Property(c => c.ExperienceYears).HasDefaultValue(0);
        builder.Property(c => c.CurrentRole).HasMaxLength(100);
        builder.Property(c => c.CurrentCompany).HasMaxLength(200);
        builder.Property(c => c.ExpectedSalary).HasColumnType("decimal(18,2)");
        builder.Property(c => c.NoticePeriodDays).HasDefaultValue(0);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Email).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.Phone).HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.IsActive);
    }
}

public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
{
    public void Configure(EntityTypeBuilder<SavedJob> builder)
    {
        builder.ToTable("saved_jobs");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.CandidateId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.JobId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.SavedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => new { s.CandidateId, s.JobId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.CandidateId);
    }
}

public class ScreeningQuestionConfiguration : IEntityTypeConfiguration<ScreeningQuestion>
{
    public void Configure(EntityTypeBuilder<ScreeningQuestion> builder)
    {
        builder.ToTable("screening_questions");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(q => q.JobId).HasMaxLength(50).IsRequired();
        builder.Property(q => q.Question).HasMaxLength(500).IsRequired();
        builder.Property(q => q.Type).HasMaxLength(20).HasDefaultValue("text");
        builder.Property(q => q.IsRequired).HasDefaultValue(true);
        builder.Property(q => q.Order).HasDefaultValue(0);
        builder.Property(q => q.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(q => q.UpdatedAt);
        builder.Property(q => q.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(q => q.JobId);
    }
}