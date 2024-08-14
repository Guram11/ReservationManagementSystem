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

    public async Task<IEnumerable<RateTimeline>> GetRatesByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.RateTimelines
            .Where(rt => rt.Date >= startDate && rt.Date <= endDate)
            .ToListAsync();
    }
}
