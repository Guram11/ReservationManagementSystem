using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IHotelRepository : IBaseRepository<Hotel>
{
    Task<bool> IsHotelInUseAsync(Guid hotelId);
}
