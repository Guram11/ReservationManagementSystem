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

    public async Task<ReservationRoom?> Delete(Guid reservationId, Guid roomId)
    {
        var reservationRoom = await _context.ReservationRooms
                .FirstOrDefaultAsync(rt => rt.ReservationId == reservationId && rt.RoomId == roomId);

        if (reservationRoom == null)
        {
            return null;
        }

        _context.ReservationRooms.Remove(reservationRoom);
        await _context.SaveChangesAsync();

        return reservationRoom;
    }
}

