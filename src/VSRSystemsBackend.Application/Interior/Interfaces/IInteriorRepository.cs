using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Interior;

namespace VSRSystemsBackend.Application.Interior.Interfaces;

public interface IInteriorProjectRepository : IRepository<InteriorProject>
{
    Task<IReadOnlyList<InteriorProject>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IInteriorRoomRepository : IRepository<InteriorRoom>
{
    Task<IReadOnlyList<InteriorRoom>> GetByProjectIdAsync(string projectId, CancellationToken cancellationToken = default);
}

public interface IInteriorDesignRepository : IRepository<InteriorDesign>
{
    Task<IReadOnlyList<InteriorDesign>> GetByRoomIdAsync(string roomId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InteriorDesign>> GetByProjectIdAsync(string projectId, CancellationToken cancellationToken = default);
}

public interface IInteriorProductRepository : IRepository<InteriorProduct>
{
    Task<IReadOnlyList<InteriorProduct>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
}