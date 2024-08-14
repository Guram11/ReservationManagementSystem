using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(DataContext context) : base(context)
    {
    }
}
