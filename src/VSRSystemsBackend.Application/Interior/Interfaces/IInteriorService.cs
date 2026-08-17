using VSRSystemsBackend.Application.Interior.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Interior.Interfaces;

public interface IInteriorProjectService
{
    Task<Result<InteriorProjectDto>> CreateAsync(CreateInteriorProjectDto dto, CancellationToken cancellationToken = default);
    Task<Result<InteriorProjectDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InteriorProjectDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<InteriorProjectDto>> UpdateAsync(string id, UpdateInteriorProjectDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InteriorProjectDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IInteriorRoomService
{
    Task<Result<InteriorRoomDto>> CreateAsync(CreateInteriorRoomDto dto, CancellationToken cancellationToken = default);
    Task<Result<InteriorRoomDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InteriorRoomDto>>> GetByProjectIdAsync(string projectId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<InteriorRoomDto>> UpdateAsync(string id, UpdateInteriorRoomDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IInteriorDesignService
{
    Task<Result<InteriorDesignDto>> CreateAsync(CreateInteriorDesignDto dto, CancellationToken cancellationToken = default);
    Task<Result<InteriorDesignDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InteriorDesignDto>>> GetByRoomIdAsync(string roomId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InteriorDesignDto>>> GetByProjectIdAsync(string projectId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<InteriorDesignDto>> UpdateAsync(string id, UpdateInteriorDesignDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<InteriorDesignDto>> GenerateDesignAsync(string id, GenerateDesignDto dto, CancellationToken cancellationToken = default);
}

public interface IInteriorProductService
{
    Task<Result<InteriorProductDto>> CreateAsync(CreateInteriorProductDto dto, CancellationToken cancellationToken = default);
    Task<Result<InteriorProductDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InteriorProductDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InteriorProductDto>>> GetByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<InteriorProductDto>> UpdateAsync(string id, UpdateInteriorProductDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}