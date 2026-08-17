using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.School;

public class LibraryIssue : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string BookTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string MemberType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string MemberId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string MemberName { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}