using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IReservationRoomServiceRepository : IBaseRepository<ReservationRoomServices>
{
    Task<ReservationRoomServices?> Delete(Guid reservationRoomId, Guid hotelServicId, CancellationToken cancellationToken);
}
