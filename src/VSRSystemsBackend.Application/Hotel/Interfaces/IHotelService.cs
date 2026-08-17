using VSRSystemsBackend.Application.Hotel.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Hotel.Interfaces;

public interface IGuestService
{
    Task<Result<GuestDto>> CreateAsync(CreateGuestDto dto, CancellationToken cancellationToken = default);
    Task<Result<GuestDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<GuestDto>> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<Result<GuestDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<GuestDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<GuestDto>> UpdateAsync(string id, UpdateGuestDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IRoomService
{
    Task<Result<RoomDto>> CreateAsync(CreateRoomDto dto, CancellationToken cancellationToken = default);
    Task<Result<RoomDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<RoomDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<RoomDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<RoomDto>>> GetByTypeAsync(string type, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<RoomDto>>> GetVacantRoomsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<RoomDto>> UpdateAsync(string id, UpdateRoomDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IReservationService
{
    Task<Result<ReservationDto>> CreateAsync(CreateReservationDto dto, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> GetByConfirmationAsync(string confirmation, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReservationDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReservationDto>>> GetByGuestIdAsync(string guestId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReservationDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReservationDto>>> GetByDateRangeAsync(DateTime checkIn, DateTime checkOut, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReservationDto>>> GetCurrentReservationsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReservationDto>>> GetArrivalsAsync(DateTime date, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReservationDto>>> GetDeparturesAsync(DateTime date, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> UpdateAsync(string id, UpdateReservationDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> CheckInAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> CheckOutAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> CancelAsync(string id, CancellationToken cancellationToken = default);
}

public interface IHousekeepingService
{
    Task<Result<HousekeepingTaskDto>> CreateAsync(CreateHousekeepingTaskDto dto, CancellationToken cancellationToken = default);
    Task<Result<HousekeepingTaskDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<HousekeepingTaskDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<HousekeepingTaskDto>>> GetByAssigneeAsync(string assignee, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<HousekeepingTaskDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<HousekeepingTaskDto>>> GetByRoomNumberAsync(string roomNumber, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<HousekeepingTaskDto>>> GetScheduledForDateAsync(DateTime date, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<HousekeepingTaskDto>> UpdateAsync(string id, UpdateHousekeepingTaskDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<HousekeepingTaskDto>> StartAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<HousekeepingTaskDto>> CompleteAsync(string id, CancellationToken cancellationToken = default);
}