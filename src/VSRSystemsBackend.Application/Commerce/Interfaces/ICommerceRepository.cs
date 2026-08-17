using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Commerce;

namespace VSRSystemsBackend.Application.Commerce.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyList<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetByBrandAsync(string brand, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetFeaturedProductsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetLowStockProductsAsync(CancellationToken cancellationToken = default);
}

public interface ICategoryRepository : IRepository<Category>
{
    Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public interface IBrandRepository : IRepository<Brand>
{
    Task<IReadOnlyList<Brand>> GetActiveBrandsAsync(CancellationToken cancellationToken = default);
    Task<Brand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Cart?> GetBySessionIdAsync(string sessionId, CancellationToken cancellationToken = default);
}

public interface IWishlistRepository : IRepository<Wishlist>
{
    Task<Wishlist?> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
}

public interface IOrderRepository : IRepository<Order>
{
    Task<IReadOnlyList<Order>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}

public interface IOfferRepository : IRepository<Offer>
{
    Task<IReadOnlyList<Offer>> GetActiveOffersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Offer>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Offer>> GetValidOffersAsync(DateTime date, CancellationToken cancellationToken = default);
}

public interface IReviewRepository : IRepository<Review>
{
    Task<IReadOnlyList<Review>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<decimal> GetAverageRatingAsync(string productId, CancellationToken cancellationToken = default);
}