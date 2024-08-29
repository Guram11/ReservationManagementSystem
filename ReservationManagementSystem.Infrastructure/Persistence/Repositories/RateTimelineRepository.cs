using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RateTimelineRepository : BaseRepository<RateTimeline>, IRateTimelineRepository
{
    private readonly DataContext _context;

    public RateTimelineRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RateTimeline>> GetRatesByDateRange(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var startDateWithoutTime = startDate.Date;
        var endDateWithoutTime = endDate.Date.AddDays(1);

        return await _context.RateTimelines
            .Where(at => at.Date >= startDateWithoutTime && at.Date < endDateWithoutTime)
            .ToListAsync(cancellationToken);
    }
}
