using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRoomTimelineRepository : BaseRepository<ReservationRoomTimeline>, IReservationRoomTimelineRepository
{
    public ReservationRoomTimelineRepository(DataContext context) : base(context)
    {
    }
}
