using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Travel;

public class Lead : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Destination { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? TripPreferences { get; set; }

    [MaxLength(500)]
    public string? BudgetRange { get; set; }

    public string PreferredTravelDate { get; set; } = string.Empty;

    public string Status { get; set; } = "new";

    public string Source { get; set; } = "website";

    public DateTime? AssignedTo { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public int Priority { get; set; } = 1;

    public int ViewCount { get; set; } = 0;

    public bool IsConverted { get; set; } = false;

    public string? ConvertedBookingId { get; set; }

    public DateTime? ConvertedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}