using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Travel;

public class GroupTrip : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string PackageName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int MaxCapacity { get; set; } = 0;
    public int CurrentBookings { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "upcoming";

    [MaxLength(200)]
    public string? TripLeader { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}