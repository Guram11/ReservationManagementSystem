using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RateTimelineRepository : BaseRepository<RateTimeline>, IRateTimelineRepository
{
    public RateTimelineRepository(DataContext context) : base(context)
    {
    }
}
