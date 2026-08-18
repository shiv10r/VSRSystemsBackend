using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.HomeServices.DTOs;

// ── Reviews (§76-§78) ────────────────────────────────────────────────────────

public class ReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string BookingNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string ProfessionalId { get; set; } = string.Empty;
    public string ProfessionalName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int? Quality { get; set; }
    public int? Professionalism { get; set; }
    public int? Punctuality { get; set; }
    public int? Cleanliness { get; set; }
    public int? Communication { get; set; }
    public int? Value { get; set; }
    public List<ReviewMediaDto> Media { get; set; } = new();
}

public class ReviewMediaDto
{
    public string Id { get; set; } = string.Empty;
    public string ReviewId { get; set; } = string.Empty;
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
}

public class CreateReviewDto
{
    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    [MaxLength(3000)]
    public string Comment { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new();

    [Range(1, 5)]
    public int? Quality { get; set; }

    [Range(1, 5)]
    public int? Professionalism { get; set; }

    [Range(1, 5)]
    public int? Punctuality { get; set; }

    [Range(1, 5)]
    public int? Cleanliness { get; set; }

    [Range(1, 5)]
    public int? Communication { get; set; }

    [Range(1, 5)]
    public int? Value { get; set; }

    public List<string> MediaUrls { get; set; } = new();
}

public class ReviewQueryDto
{
    [MaxLength(50)]
    public string? ProfessionalId { get; set; }

    [MaxLength(50)]
    public string? ServiceId { get; set; }

    [Range(1, 5)]
    public int? MinRating { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ReviewListResultDto
{
    public List<ReviewDto> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

// ── Support (§80-§85) ────────────────────────────────────────────────────────

public class SupportTicketDto
{
    public string Id { get; set; } = string.Empty;
    public string TicketNumber { get; set; } = string.Empty;
    public string RaisedBy { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? BookingId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
    public string? Resolution { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSupportTicketDto
{
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? BookingId { get; set; }

    [MaxLength(20)]
    [RegularExpression("^(low|medium|high|critical)$")]
    public string Priority { get; set; } = "medium";
}

public class UpdateSupportTicketDto
{
    [MaxLength(20)]
    [RegularExpression("^(open|in_progress|waiting_customer|waiting_professional|escalated|resolved|closed)$")]
    public string Status { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? AssignedTo { get; set; }

    [MaxLength(1000)]
    public string? Resolution { get; set; }
}

public class DisputeDto
{
    public string Id { get; set; } = string.Empty;
    public string? TicketId { get; set; }
    public string BookingId { get; set; } = string.Empty;
    public string BookingNumber { get; set; } = string.Empty;
    public string RaisedBy { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Resolution { get; set; }
    public string? ResolvedBy { get; set; }
    public DateTime? ResolvedAt { get; set; }
}

public class CreateDisputeDto
{
    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? TicketId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Details { get; set; } = string.Empty;
}

public class ResolveDisputeDto
{
    [Required]
    [MaxLength(30)]
    [RegularExpression("^(resolved|rejected)$")]
    public string Status { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Resolution { get; set; } = string.Empty;
}

public class NotificationDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = "{}";
    public DateTime SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
}

public class SendNotificationDto
{
    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(10)]
    [RegularExpression("^(push|sms|email|in_app)$")]
    public string Channel { get; set; } = "in_app";

    [Required]
    [MaxLength(100)]
    public string Template { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string PayloadJson { get; set; } = "{}";
}