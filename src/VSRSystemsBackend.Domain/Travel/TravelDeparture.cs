using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Travel;

public class TravelDeparture : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string DepartureCity { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    public DateTime DepartureDate { get; set; }

    public int AvailableSeats { get; set; }

    public int TotalSeats { get; set; }

    [Required]
    public decimal Price { get; set; }

    public decimal? DiscountedPrice { get; set; }

    [MaxLength(3000)]
    public string? ImageUrl { get; set; }

    public string Status { get; set; } = "active";

    public bool IsAvailable => Status == "active" && AvailableSeats > 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}