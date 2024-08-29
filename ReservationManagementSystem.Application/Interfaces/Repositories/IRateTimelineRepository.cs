using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IRateTimelineRepository : IBaseRepository<RateTimeline>
{
    Task<IEnumerable<RateTimeline>> GetRatesByDateRange(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
}
