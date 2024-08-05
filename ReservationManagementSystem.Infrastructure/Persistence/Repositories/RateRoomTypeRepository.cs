using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RateRoomTypeRepository : BaseRepository<RateRoomType>, IRateRoomTypeRepository
{
    public RateRoomTypeRepository(DataContext context) : base(context)
    {
    }
}
