using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Hotel;

namespace VSRSystemsBackend.Application.Hotel.Interfaces;

public interface IGuestRepository : IRepository<Guest>
{
    Task<Guest?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<Guest?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Guest>> GetVipGuestsAsync(CancellationToken cancellationToken = default);
}

public interface IRoomRepository : IRepository<Room>
{
    Task<IReadOnlyList<Room>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Room>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Room>> GetVacantRoomsAsync(CancellationToken cancellationToken = default);
}

public interface IReservationRepository : IRepository<Reservation>
{
    Task<IReadOnlyList<Reservation>> GetByGuestIdAsync(string guestId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetByRoomNumberAsync(string roomNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetByDateRangeAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default);
    Task<Reservation?> GetByConfirmationAsync(string confirmation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetCurrentReservationsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetArrivalsAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetDeparturesAsync(DateTime date, CancellationToken cancellationToken = default);
}

public interface IHousekeepingRepository : IRepository<HousekeepingTask>
{
    Task<IReadOnlyList<HousekeepingTask>> GetByAssigneeAsync(string assignee, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<HousekeepingTask>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<HousekeepingTask>> GetByRoomNumberAsync(string roomNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<HousekeepingTask>> GetScheduledForDateAsync(DateTime date, CancellationToken cancellationToken = default);
}