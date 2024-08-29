using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class AvailibilityRepository : BaseRepository<AvailabilityTimeline>, IAvailibilityTimelineRepository
{
    private readonly DataContext _context;

    public AvailibilityRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AvailabilityTimeline>> GetAvailabilityByDateRange(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var startDateWithoutTime = startDate.Date;
        var endDateWithoutTime = endDate.Date.AddDays(1);

        return await _context.AvailabilityTimelines
            .Where(at => at.Date >= startDateWithoutTime && at.Date < endDateWithoutTime)
            .ToListAsync(cancellationToken);
    }
}

