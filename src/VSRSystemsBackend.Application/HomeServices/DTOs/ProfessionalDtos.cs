using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.HomeServices.DTOs;

// ── Professionals ────────────────────────────────────────────────────────────

public class ProfessionalDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string OnboardingStatus { get; set; } = string.Empty;
    public decimal QualityScore { get; set; }
    public string Tier { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int JobsCompleted { get; set; }
    public double AvgRating { get; set; }
    public List<string> Skills { get; set; } = new();
    public List<string> ServiceAreaNames { get; set; } = new();
}

public class ProfessionalDetailDto : ProfessionalDto
{
    public List<ProfessionalDocumentDto> Documents { get; set; } = new();
    public List<ProfessionalAvailabilityDto> Availabilities { get; set; } = new();
    public List<ProfessionalPerformanceDto> Performances { get; set; } = new();
}

public class ProfessionalDocumentDto
{
    public string Id { get; set; } = string.Empty;
    public string ProfessionalId { get; set; } = string.Empty;
    public string DocType { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }
}

public class ProfessionalAvailabilityDto
{
    public string Id { get; set; } = string.Empty;
    public string ProfessionalId { get; set; } = string.Empty;
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsRecurring { get; set; }
}

public class ProfessionalPerformanceDto
{
    public string Id { get; set; } = string.Empty;
    public string ProfessionalId { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int JobsCompleted { get; set; }
    public int JobsCancelled { get; set; }
    public double AvgRating { get; set; }
    public double OnTimeRate { get; set; }
    public double AcceptanceRate { get; set; }
}

public class RegisterProfessionalDto
{
    [Required]
    [MaxLength(200)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Gender { get; set; } = string.Empty;

    public DateTime? Dob { get; set; }

    [Required]
    [MaxLength(30)]
    [RegularExpression("^[0-9+ -]{10,15}$")]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(300)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public List<string> ServiceIds { get; set; } = new();

    [Required]
    public List<ProfessionalServiceAreaRequest> ServiceAreas { get; set; } = new();
}

public class ProfessionalServiceAreaRequest
{
    [Required]
    [MaxLength(50)]
    public string CityId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ZoneId { get; set; } = string.Empty;
}

public class UpdateProfessionalAvailabilityDto
{
    [Required]
    [Range(0, 6)]
    public int DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; } = new(9, 0, 0);
    public TimeSpan EndTime { get; set; } = new(19, 0, 0);
    public bool IsRecurring { get; set; } = true;
}

public class ReviewProfessionalDocumentDto
{
    [Required]
    [MaxLength(20)]
    [RegularExpression("^(approved|rejected)$")]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? RejectionReason { get; set; }
}

public class UpdateProfessionalStatusDto
{
    [Required]
    [MaxLength(30)]
    [RegularExpression("^(verified|suspended)$")]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Reason { get; set; }
}

public class ProfessionalSearchQueryDto
{
    [MaxLength(50)]
    public string? ServiceId { get; set; }

    [MaxLength(50)]
    public string? CityId { get; set; }

    [MaxLength(50)]
    public string? ZoneId { get; set; }

    [MaxLength(20)]
    public string? OnboardingStatus { get; set; }

    [MaxLength(20)]
    public string? Tier { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ProfessionalListResultDto
{
    public List<ProfessionalDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}