using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class AvailibilityRepository : BaseRepository<AvailabilityTimeline>, IAvailibilityTimelineRepository
{
    public AvailibilityRepository(DataContext context) : base(context)
    {
    }
}

