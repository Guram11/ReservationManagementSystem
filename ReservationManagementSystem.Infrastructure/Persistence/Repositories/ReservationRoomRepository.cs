using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRoomRepository : BaseRepository<ReservationRoom>, IReservationRoomRepository
{
    private readonly DataContext _context;

    public ReservationRoomRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ReservationRoom?> GetReservationRoomWithTimeline(Guid id, CancellationToken cancellationToken)
    {
        var reservationRoom = await _context.ReservationRooms
          .Include(rt => rt.ReservationRoomTimelines)
          .FirstOrDefaultAsync(rt => rt.Id == id, cancellationToken);

        if (reservationRoom == null)
        {
            return null;
        }

        return reservationRoom;
    }

    public async Task<ReservationRoom?> GetReservationRoomByReservationId(Guid reservationId, CancellationToken cancellationToken)
    {
        var reservationRoom = await _context.ReservationRooms
                .FirstOrDefaultAsync(rt => rt.ReservationId == reservationId, cancellationToken);

        if (reservationRoom == null)
        {
            return null;
        }

        return reservationRoom;
    }

    public async Task<bool> IsReservationRoomInUseAsync(Guid id)
    {
        var isReservationRoomTimelineInUse = await _context.ReservationRoomTimelines
           .AnyAsync(r => r.ReservationRoomId == id);

        var isReservationPaymentInUse = await _context.ReservationRoomPayments
          .AnyAsync(r => r.ReservationRoomId == id);

        var isReservationRoomServicesInUse = await _context.ReservationRoomServices
          .AnyAsync(r => r.ReservationRoomId == id);

        var isGuestInUse = await _context.Guests
            .AnyAsync(r => r.ReservationRoomId == id);


        return isReservationRoomTimelineInUse || isGuestInUse || isReservationPaymentInUse || isReservationRoomServicesInUse;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

