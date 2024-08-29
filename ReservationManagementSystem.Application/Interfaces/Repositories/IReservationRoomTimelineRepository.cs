using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IReservationRoomTimelineRepository : IBaseRepository<ReservationRoomTimeline>
{
    Task<List<ReservationRoomTimeline>> GetReservationRoomTimelinesByReservationRoomId(Guid id, CancellationToken cancellationToken);
}
