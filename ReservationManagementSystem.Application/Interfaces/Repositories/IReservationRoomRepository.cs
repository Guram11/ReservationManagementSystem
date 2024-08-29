using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IReservationRoomRepository : IBaseRepository<ReservationRoom>
{
    Task<ReservationRoom?> Delete(Guid reservationId, Guid roomId, CancellationToken cancellationToken);
    Task<ReservationRoom?> GetReservationRoomWithTimeline(Guid id, CancellationToken cancellationToken);
    Task<ReservationRoom?> GetReservationRoomByReservationId(Guid reservationId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
