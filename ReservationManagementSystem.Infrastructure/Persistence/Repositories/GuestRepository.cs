using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class GuestRepository : BaseRepository<Guest>, IGuestRepository
{
    public GuestRepository(DataContext context) : base(context)
    {
    }
}
