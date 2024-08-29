using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRoomTimelineRepository : BaseRepository<ReservationRoomTimeline>, IReservationRoomTimelineRepository
{
    private readonly DataContext _context;

    public ReservationRoomTimelineRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ReservationRoomTimeline>> GetReservationRoomTimelinesByReservationRoomId( Guid id, CancellationToken cancellationToken)
    {
        return await _context.ReservationRoomTimelines
            .Where(rrt => rrt.ReservationRoomId == id)
            .ToListAsync(cancellationToken);
    }
}
