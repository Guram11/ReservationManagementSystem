using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;


namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RateRepository : BaseRepository<Rate>, IRateRepository
{
    private readonly DataContext _context;

    public RateRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsRateInUseAsync(Guid id)
    {
        var isRateRoomTypeInUse = await _context.RateRoomTypes
            .AnyAsync(rt => rt.RateId == id);

        var isReservationRoomInUse = await _context.ReservationRooms
            .AnyAsync(r => r.RateId == id);

        var isReservationInUse = await _context.Reservations
            .AnyAsync(r => r.RateId == id);

        var isRateTimelineInUse = await _context.RateTimelines
            .AnyAsync(r => r.RateId == id);

        return isRateRoomTypeInUse || isReservationInUse || isReservationRoomInUse || isRateTimelineInUse;
    }
}
