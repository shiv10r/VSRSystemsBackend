using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.News;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.Title).HasMaxLength(300).IsRequired();
        builder.Property(a => a.Slug).HasMaxLength(320).IsRequired();
        builder.Property(a => a.Summary).HasMaxLength(500);
        builder.Property(a => a.Content).HasMaxLength(50000).IsRequired();
        builder.Property(a => a.CategoryId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.AuthorId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.AuthorName).HasMaxLength(200).IsRequired();
        builder.Property(a => a.FeaturedImageUrl).HasMaxLength(500);
        builder.Property(a => a.Status).HasMaxLength(20).HasDefaultValue("draft");
        builder.Property(a => a.PublishedAt);
        builder.Property(a => a.ViewCount).HasDefaultValue(0);
        builder.Property(a => a.LikeCount).HasDefaultValue(0);
        builder.Property(a => a.ShareCount).HasDefaultValue(0);
        builder.Property(a => a.ReadingTimeMinutes).HasDefaultValue(0);
        builder.Property(a => a.Tags).HasMaxLength(500);
        builder.Property(a => a.MetaTitle).HasMaxLength(200);
        builder.Property(a => a.MetaDescription).HasMaxLength(500);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(a => a.CategoryId);
        builder.HasIndex(a => a.AuthorId);
        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => a.PublishedAt);
        builder.HasIndex(a => a.ViewCount);
    }
}

public class NewsCategoryConfiguration : IEntityTypeConfiguration<NewsCategory>
{
    public void Configure(EntityTypeBuilder<NewsCategory> builder)
    {
        builder.ToTable("news_categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(120).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
        builder.Property(c => c.DisplayOrder).HasDefaultValue(0);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.IsActive);
    }
}

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("bookmarks");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.UserId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.ArticleId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => new { b.UserId, b.ArticleId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(b => b.UserId);
    }
}