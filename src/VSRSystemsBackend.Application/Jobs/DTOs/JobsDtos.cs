using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Jobs.DTOs;

public class JobDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ExperienceLevel { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsRemote { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string SalaryCurrency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyInitials { get; set; } = string.Empty;
    public string? Industry { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string Country { get; set; } = "India";
    public string ExperienceText { get; set; } = string.Empty;
    public int MinExperience { get; set; }
    public int MaxExperience { get; set; }
    public string SalaryText { get; set; } = string.Empty;
    public bool SalaryVisible { get; set; }
    public string WorkMode { get; set; } = "On-site";
    public string EmploymentType { get; set; } = "Full-time";
    public string Summary { get; set; } = string.Empty;
    public string SkillsJson { get; set; } = "[]";
    public string ResponsibilitiesJson { get; set; } = "[]";
    public string BenefitsJson { get; set; } = "[]";
    public string ApplicationMode { get; set; } = "EasyApply";
    public string? ExternalApplyUrl { get; set; }
    public string? OriginalSourceUrl { get; set; }
    public string? SourceType { get; set; }
    public bool IsAggregated { get; set; }
    public bool Featured { get; set; }
    public bool Verified { get; set; }
    public string? ExternalJobId { get; set; }
    public string? PrimaryJobSourceId { get; set; }
    public string? PostedAtSource { get; set; }
    public DateTime? LastSeenAtSource { get; set; }
    public string CanonicalFingerprint { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateJobDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CompanyId { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Requirements { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "full-time";

    [MaxLength(30)]
    public string ExperienceLevel { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    public bool IsRemote { get; set; } = false;

    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    [MaxLength(3)]
    public string SalaryCurrency { get; set; } = "INR";

    public DateTime? ExpiresAt { get; set; }
}

public class UpdateJobDto
{
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(50)]
    public string CompanyId { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Requirements { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(30)]
    public string ExperienceLevel { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    public bool IsRemote { get; set; }

    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    [MaxLength(3)]
    public string SalaryCurrency { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public DateTime? ExpiresAt { get; set; }
}

public class CompanyDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public string? Size { get; set; }
    public string? Industry { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCompanyDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Website { get; set; }

    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    [MaxLength(30)]
    public string? Size { get; set; }

    [MaxLength(100)]
    public string? Industry { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }
}

public class UpdateCompanyDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Website { get; set; }

    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    [MaxLength(30)]
    public string? Size { get; set; }

    [MaxLength(100)]
    public string? Industry { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public bool IsActive { get; set; }
}

public class JobApplicationDto
{
    public string Id { get; set; } = string.Empty;
    public string JobId { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string CandidateId { get; set; } = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public string CoverLetter { get; set; } = string.Empty;
    public string? ResumeUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateJobApplicationDto
{
    [Required]
    [MaxLength(50)]
    public string JobId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CandidateId { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string CoverLetter { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ResumeUrl { get; set; }
}

public class UpdateJobApplicationDto
{
    [MaxLength(3000)]
    public string CoverLetter { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ResumeUrl { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class CandidateDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? ResumeUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? Skills { get; set; }
    public int ExperienceYears { get; set; }
    public string? CurrentRole { get; set; }
    public string? CurrentCompany { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public int NoticePeriodDays { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCandidateDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ResumeUrl { get; set; }

    [MaxLength(500)]
    public string? LinkedInUrl { get; set; }

    [MaxLength(500)]
    public string? PortfolioUrl { get; set; }

    [MaxLength(2000)]
    public string? Skills { get; set; }

    public int ExperienceYears { get; set; }

    [MaxLength(100)]
    public string? CurrentRole { get; set; }

    [MaxLength(200)]
    public string? CurrentCompany { get; set; }

    public decimal? ExpectedSalary { get; set; }

    public int NoticePeriodDays { get; set; }
}

public class UpdateCandidateDto
{
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ResumeUrl { get; set; }

    [MaxLength(500)]
    public string? LinkedInUrl { get; set; }

    [MaxLength(500)]
    public string? PortfolioUrl { get; set; }

    [MaxLength(2000)]
    public string? Skills { get; set; }

    public int ExperienceYears { get; set; }

    [MaxLength(100)]
    public string? CurrentRole { get; set; }

    [MaxLength(200)]
    public string? CurrentCompany { get; set; }

    public decimal? ExpectedSalary { get; set; }

    public int NoticePeriodDays { get; set; }

    public bool IsActive { get; set; }
}

public class SavedJobDto
{
    public string Id { get; set; } = string.Empty;
    public string CandidateId { get; set; } = string.Empty;
    public string JobId { get; set; } = string.Empty;
    public DateTime SavedAt { get; set; }
}

public class CreateSavedJobDto
{
    [Required]
    [MaxLength(50)]
    public string CandidateId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string JobId { get; set; } = string.Empty;
}

public class ScreeningQuestionDto
{
    public string Id { get; set; } = string.Empty;
    public string JobId { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateScreeningQuestionDto
{
    [Required]
    [MaxLength(50)]
    public string JobId { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "text";

    public bool IsRequired { get; set; } = true;

    public int Order { get; set; }
}

public class UpdateScreeningQuestionDto
{
    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public bool IsRequired { get; set; }

    public int Order { get; set; }
}