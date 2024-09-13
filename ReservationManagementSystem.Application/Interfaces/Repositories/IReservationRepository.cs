using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<bool> IsReservationInUseAsync(Guid id);
}
