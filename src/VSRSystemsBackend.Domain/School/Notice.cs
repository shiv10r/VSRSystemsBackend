using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class Notice : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Body { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Audience { get; set; } = "all";

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public bool Pinned { get; set; } = false;

    [MaxLength(20)]
    public string Status { get; set; } = "draft";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}