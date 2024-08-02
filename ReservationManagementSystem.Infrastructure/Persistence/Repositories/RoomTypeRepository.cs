using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RateTypeRepository : BaseRepository<RoomType>, IRoomTypeRepository
{
    public RateTypeRepository(DataContext context) : base(context)
    {
    }
}
