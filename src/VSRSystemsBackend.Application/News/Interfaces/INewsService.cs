using VSRSystemsBackend.Application.News.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.News.Interfaces;

public interface IArticleService
{
    Task<Result<ArticleDto>> CreateAsync(CreateArticleDto dto, CancellationToken cancellationToken = default);
    Task<Result<ArticleDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ArticleDto>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ArticleDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ArticleDto>>> GetByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ArticleDto>>> GetByAuthorAsync(string author, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ArticleDto>>> GetPublishedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ArticleDto>>> GetTrendingAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ArticleDto>>> GetLatestAsync(int limit = 10, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ArticleDto>>> SearchAsync(string searchTerm, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<ArticleDto>> UpdateAsync(string id, UpdateArticleDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ArticleDto>> PublishAsync(string id, CancellationToken cancellationToken = default);
}

public interface ICategoryService
{
    Task<Result<NewsCategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<Result<NewsCategoryDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<NewsCategoryDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<NewsCategoryDto>>> GetActiveAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<NewsCategoryDto>> UpdateAsync(string id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IBookmarkService
{
    Task<Result<BookmarkDto>> CreateAsync(CreateBookmarkDto dto, CancellationToken cancellationToken = default);
    Task<Result<BookmarkDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BookmarkDto>>> GetByUserIdAsync(string userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}