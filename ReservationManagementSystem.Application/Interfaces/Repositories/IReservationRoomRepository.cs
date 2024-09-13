using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IReservationRoomRepository : IBaseRepository<ReservationRoom>
{
    Task<ReservationRoom?> GetReservationRoomWithTimeline(Guid id, CancellationToken cancellationToken);
    Task<ReservationRoom?> GetReservationRoomByReservationId(Guid reservationId, CancellationToken cancellationToken);
    Task<bool> IsReservationRoomInUseAsync(Guid id);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
