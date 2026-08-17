using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.News;

public class Article : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(320)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Summary { get; set; }

    [Required]
    [MaxLength(50000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AuthorId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string AuthorName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? FeaturedImageUrl { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "draft";

    public DateTime? PublishedAt { get; set; }

    public int ViewCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int ShareCount { get; set; } = 0;

    public int ReadingTimeMinutes { get; set; } = 0;

    [MaxLength(500)]
    public string? Tags { get; set; }

    [MaxLength(200)]
    public string? MetaTitle { get; set; }

    [MaxLength(500)]
    public string? MetaDescription { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}