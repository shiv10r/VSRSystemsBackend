using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Commerce.DTOs;

public class ProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string BrandId { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public int StockQuantity { get; set; }
    public int LowStockThreshold { get; set; }
    public bool TrackInventory { get; set; }
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public string? Tags { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateProductDto
{
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BrandId { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public int StockQuantity { get; set; } = 0;
    public int LowStockThreshold { get; set; } = 10;
    public bool TrackInventory { get; set; } = true;
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public string Status { get; set; } = "active";
    public bool IsFeatured { get; set; } = false;
    public string? Tags { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
}

public class UpdateProductDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string BrandId { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public int StockQuantity { get; set; }
    public int LowStockThreshold { get; set; }
    public bool TrackInventory { get; set; }
    public decimal? Weight { get; set; }
    public string? Dimensions { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public string? Tags { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
}

public class CategoryDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ParentId { get; set; }
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

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ParentId { get; set; }

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

    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ParentId { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class BrandDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBrandDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    [MaxLength(500)]
    public string? WebsiteUrl { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdateBrandDto
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(120)]
    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    [MaxLength(500)]
    public string? WebsiteUrl { get; set; }

    public bool IsActive { get; set; }
}

public class CartDto
{
    public string Id { get; set; } = string.Empty;
    public string? CustomerId { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CartItemDto
{
    public string Id { get; set; } = string.Empty;
    public string CartId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AddCartItemDto
{
    [Required]
    [MaxLength(50)]
    public string ProductId { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;
}

public class UpdateCartItemDto
{
    public int Quantity { get; set; }
}

public class WishlistDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public List<WishlistItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class WishlistItemDto
{
    public string Id { get; set; } = string.Empty;
    public string WishlistId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class OrderDto
{
    public string Id { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string? BillingAddress { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class OrderItemDto
{
    public string Id { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateOrderDto
{
    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? CustomerPhone { get; set; }

    [Required]
    [MaxLength(1000)]
    public string ShippingAddress { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? BillingAddress { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public class CreateOrderItemDto
{
    [Required]
    [MaxLength(50)]
    public string ProductId { get; set; } = string.Empty;

    public int Quantity { get; set; } = 1;
}

public class UpdateOrderDto
{
    [MaxLength(1000)]
    public string ShippingAddress { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? BillingAddress { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
}

public class OfferDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public int UsageCount { get; set; }
    public bool IsActive { get; set; }
    public string? ApplicableProductIds { get; set; }
    public string? ApplicableCategoryIds { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateOfferDto
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public decimal Value { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }

    public string? ApplicableProductIds { get; set; }
    public string? ApplicableCategoryIds { get; set; }
}

public class UpdateOfferDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public decimal Value { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public decimal? MaxDiscountAmount { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }

    public string? ApplicableProductIds { get; set; }
    public string? ApplicableCategoryIds { get; set; }
}

public class ReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public bool IsVerifiedPurchase { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateReviewDto
{
    [Required]
    [MaxLength(50)]
    public string ProductId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public bool IsVerifiedPurchase { get; set; }
}

public class UpdateReviewDto
{
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string Status { get; set; } = string.Empty;
}