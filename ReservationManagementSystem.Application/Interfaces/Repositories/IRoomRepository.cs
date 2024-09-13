using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<bool> IsRoomInUseAsync(Guid id);
}
