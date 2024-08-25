using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IReservationRoomRepository : IBaseRepository<ReservationRoom>
{
    Task<ReservationRoom?> Delete(Guid reservationId, Guid roomId);
    Task<ReservationRoom?> GetReservationRoomWithTimeline(Guid id);
    Task<ReservationRoom?> GetReservationRoomByReservationId(Guid reservationId);
    Task SaveChangesAsync();
}
