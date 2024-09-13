using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    private readonly DataContext _context;

    public RoomRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsRoomInUseAsync(Guid id)
    {
        var isReservationRoomInUse = await _context.ReservationRooms
           .AnyAsync(r => r.RoomId == id);

        return isReservationRoomInUse;
    }
}