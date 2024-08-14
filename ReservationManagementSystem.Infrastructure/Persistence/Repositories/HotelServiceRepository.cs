using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class HotelServiceRepository : BaseRepository<HotelService>, IHotelServiceRepository
{
    public HotelServiceRepository(DataContext context) : base(context)
    {
    }
}
