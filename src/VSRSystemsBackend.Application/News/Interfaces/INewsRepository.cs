using VSRSystemsBackend.Core.Interfaces;
using DomainNews = VSRSystemsBackend.Domain.News;

namespace VSRSystemsBackend.Application.News.Interfaces;

public interface IArticleRepository : IRepository<DomainNews.Article>
{
    Task<IReadOnlyList<DomainNews.Article>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainNews.Article>> GetByAuthorAsync(string author, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainNews.Article>> GetPublishedArticlesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainNews.Article>> GetTrendingArticlesAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainNews.Article>> GetLatestArticlesAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<DomainNews.Article?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainNews.Article>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

public interface ICategoryRepository : IRepository<DomainNews.NewsCategory>
{
    Task<IReadOnlyList<DomainNews.NewsCategory>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
}

public interface IBookmarkRepository : IRepository<DomainNews.Bookmark>
{
    Task<IReadOnlyList<DomainNews.Bookmark>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<DomainNews.Bookmark?> GetByUserAndArticleAsync(string userId, string articleId, CancellationToken cancellationToken = default);
}