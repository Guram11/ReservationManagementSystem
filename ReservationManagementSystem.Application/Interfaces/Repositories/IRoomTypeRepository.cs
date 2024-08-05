using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IRoomTypeRepository : IBaseRepository<RoomType>
{
    Task<RoomType?> GetRoomTypeWithAvailabilityAsync(Guid roomTypeId);
    Task SaveChangesAsync();
}