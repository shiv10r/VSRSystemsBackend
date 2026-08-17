using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Commerce;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Sku).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.Property(p => p.CategoryId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.BrandId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.CompareAtPrice).HasColumnType("decimal(18,2)");
        builder.Property(p => p.CostPrice).HasColumnType("decimal(18,2)");
        builder.Property(p => p.StockQuantity).HasDefaultValue(0);
        builder.Property(p => p.LowStockThreshold).HasDefaultValue(10);
        builder.Property(p => p.TrackInventory).HasDefaultValue(true);
        builder.Property(p => p.Weight).HasColumnType("decimal(10,2)");
        builder.Property(p => p.Dimensions).HasMaxLength(100);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(p => p.IsFeatured).HasDefaultValue(false);
        builder.Property(p => p.Tags).HasMaxLength(500);
        builder.Property(p => p.MetaTitle).HasMaxLength(200);
        builder.Property(p => p.MetaDescription).HasMaxLength(500);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Sku).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.BrandId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.IsFeatured);
    }
}

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(120).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.ParentId).HasMaxLength(50);
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
        builder.Property(c => c.DisplayOrder).HasDefaultValue(0);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.ParentId);
        builder.HasIndex(c => c.IsActive);
    }
}

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("brands");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
        builder.Property(b => b.Slug).HasMaxLength(120).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(500);
        builder.Property(b => b.LogoUrl).HasMaxLength(500);
        builder.Property(b => b.WebsiteUrl).HasMaxLength(500);
        builder.Property(b => b.IsActive).HasDefaultValue(true);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(b => b.IsActive);
    }
}

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.CustomerId).HasMaxLength(50);
        builder.Property(c => c.SessionId).HasMaxLength(100);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.CustomerId);
        builder.HasIndex(c => c.SessionId);
    }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("cart_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(i => i.CartId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.ProductId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.Quantity).HasDefaultValue(1);
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(i => i.UpdatedAt);
    }
}

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("wishlists");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(w => w.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(w => w.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(w => w.UpdatedAt);
        builder.Property(w => w.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(w => w.CustomerId).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
{
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        builder.ToTable("wishlist_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(i => i.WishlistId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.ProductId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(o => o.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(o => o.CustomerEmail).HasMaxLength(200).IsRequired();
        builder.Property(o => o.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(o => o.CustomerPhone).HasMaxLength(20);
        builder.Property(o => o.ShippingAddress).HasMaxLength(1000).IsRequired();
        builder.Property(o => o.BillingAddress).HasMaxLength(1000);
        builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(o => o.TaxAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(o => o.ShippingAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(o => o.DiscountAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(o => o.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(o => o.PaymentStatus).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(o => o.PaymentMethod).HasMaxLength(50);
        builder.Property(o => o.TransactionId).HasMaxLength(100);
        builder.Property(o => o.Notes).HasMaxLength(1000);
        builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(o => o.UpdatedAt);
        builder.Property(o => o.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(o => o.OrderNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(o => o.CustomerId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.PaymentStatus);
        builder.HasIndex(o => o.CreatedAt);
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(i => i.OrderId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.ProductId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.ProductName).HasMaxLength(200).IsRequired();
        builder.Property(i => i.ProductSku).HasMaxLength(100).IsRequired();
        builder.Property(i => i.Quantity).HasDefaultValue(1);
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.TotalPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("offers");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(o => o.Code).HasMaxLength(50).IsRequired();
        builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
        builder.Property(o => o.Description).HasMaxLength(1000);
        builder.Property(o => o.Type).HasMaxLength(20).IsRequired();
        builder.Property(o => o.Value).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(o => o.MinOrderAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.MaxDiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.StartDate).IsRequired();
        builder.Property(o => o.EndDate).IsRequired();
        builder.Property(o => o.UsageLimit).HasDefaultValue(0);
        builder.Property(o => o.UsageCount).HasDefaultValue(0);
        builder.Property(o => o.IsActive).HasDefaultValue(true);
        builder.Property(o => o.ApplicableProductIds).HasMaxLength(2000);
        builder.Property(o => o.ApplicableCategoryIds).HasMaxLength(2000);
        builder.Property(o => o.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(o => o.UpdatedAt);
        builder.Property(o => o.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(o => o.Code).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(o => o.IsActive);
        builder.HasIndex(o => o.StartDate);
        builder.HasIndex(o => o.EndDate);
    }
}

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.ProductId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(r => r.Rating).IsRequired();
        builder.Property(r => r.Title).HasMaxLength(200);
        builder.Property(r => r.Comment).HasMaxLength(2000);
        builder.Property(r => r.IsVerifiedPurchase).HasDefaultValue(false);
        builder.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("published");
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.ProductId);
        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.Status);
    }
}