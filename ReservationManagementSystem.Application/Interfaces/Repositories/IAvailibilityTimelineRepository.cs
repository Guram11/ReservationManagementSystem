using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IAvailibilityTimelineRepository : IBaseRepository<AvailabilityTimeline>
{
    Task<IEnumerable<AvailabilityTimeline>> GetAvailabilityByDateRange(DateTime startDate, DateTime endDate);
}
