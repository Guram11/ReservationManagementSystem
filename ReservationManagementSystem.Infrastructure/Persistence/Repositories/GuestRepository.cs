using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Persistence.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class GuestRepository : BaseRepository<Guest>, IGuestRepository
{
    public GuestRepository(DataContext context) : base(context)
    {
    }
}
