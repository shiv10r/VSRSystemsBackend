using VSRSystemsBackend.Application.Commerce.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Commerce.Interfaces;

public interface IProductService
{
    Task<Result<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> GetByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> GetByBrandAsync(string brand, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> GetActiveProductsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> GetFeaturedProductsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> GetLowStockProductsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> SearchAsync(string searchTerm, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> UpdateAsync(string id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ICategoryService
{
    Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<Result<CategoryDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CategoryDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CategoryDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<CategoryDto>> UpdateAsync(string id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IBrandService
{
    Task<Result<BrandDto>> CreateAsync(CreateBrandDto dto, CancellationToken cancellationToken = default);
    Task<Result<BrandDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BrandDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BrandDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BrandDto>> UpdateAsync(string id, UpdateBrandDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ICartService
{
    Task<Result<CartDto>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<CartDto>> GetBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default);
    Task<Result<CartDto>> AddItemAsync(string cartId, AddCartItemDto dto, CancellationToken cancellationToken = default);
    Task<Result<CartDto>> UpdateItemAsync(string cartId, string itemId, UpdateCartItemDto dto, CancellationToken cancellationToken = default);
    Task<Result<CartDto>> RemoveItemAsync(string cartId, string itemId, CancellationToken cancellationToken = default);
    Task<Result<CartDto>> ClearAsync(string cartId, CancellationToken cancellationToken = default);
}

public interface IWishlistService
{
    Task<Result<WishlistDto>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<WishlistDto>> AddItemAsync(string wishlistId, string productId, CancellationToken cancellationToken = default);
    Task<Result<WishlistDto>> RemoveItemAsync(string wishlistId, string productId, CancellationToken cancellationToken = default);
    Task<Result<WishlistDto>> ClearAsync(string wishlistId, CancellationToken cancellationToken = default);
}

public interface IOrderService
{
    Task<Result<OrderDto>> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<OrderDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<OrderDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<OrderDto>>> GetByDateRangeAsync(DateTime from, DateTime to, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> UpdateAsync(string id, UpdateOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> ConfirmAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<OrderDto>> CancelAsync(string id, CancellationToken cancellationToken = default);
}

public interface IOfferService
{
    Task<Result<OfferDto>> CreateAsync(CreateOfferDto dto, CancellationToken cancellationToken = default);
    Task<Result<OfferDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<OfferDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<OfferDto>>> GetActiveOffersAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<OfferDto>>> GetByProductIdAsync(string productId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<OfferDto>> UpdateAsync(string id, UpdateOfferDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IReviewService
{
    Task<Result<ReviewDto>> CreateAsync(CreateReviewDto dto, CancellationToken cancellationToken = default);
    Task<Result<ReviewDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReviewDto>>> GetByProductIdAsync(string productId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReviewDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<decimal>> GetAverageRatingAsync(string productId, CancellationToken cancellationToken = default);
    Task<Result<ReviewDto>> UpdateAsync(string id, UpdateReviewDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}