using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.News.DTOs;

public class ArticleDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? FeaturedImageUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int ShareCount { get; set; }
    public int ReadingTimeMinutes { get; set; }
    public string? Tags { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateArticleDto
{
    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

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

    [MaxLength(500)]
    public string? Summary { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "draft";

    [MaxLength(500)]
    public string? Tags { get; set; }

    [MaxLength(200)]
    public string? MetaTitle { get; set; }

    [MaxLength(500)]
    public string? MetaDescription { get; set; }
}

public class UpdateArticleDto
{
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(50000)]
    public string Content { get; set; } = string.Empty;

    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string AuthorId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string AuthorName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? FeaturedImageUrl { get; set; }

    [MaxLength(500)]
    public string? Summary { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Tags { get; set; }

    [MaxLength(200)]
    public string? MetaTitle { get; set; }

    [MaxLength(500)]
    public string? MetaDescription { get; set; }
}

public class NewsCategoryDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateCategoryDto
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(120)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class BookmarkDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string ArticleId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateBookmarkDto
{
    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ArticleId { get; set; } = string.Empty;
}