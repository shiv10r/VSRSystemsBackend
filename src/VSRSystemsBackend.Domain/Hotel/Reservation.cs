using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Hotel;

public class Reservation : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Confirmation { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string GuestId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public int Adults { get; set; } = 1;
    public int Children { get; set; } = 0;

    [MaxLength(20)]
    public string Source { get; set; } = "Direct";

    [MaxLength(20)]
    public string Status { get; set; } = "confirmed";

    public decimal Balance { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}