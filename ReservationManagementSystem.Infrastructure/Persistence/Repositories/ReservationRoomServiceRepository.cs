using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRoomServiceRepository : BaseRepository<ReservationRoomServices>, IReservationRoomServiceRepository
{
    private readonly DataContext _context;

    public ReservationRoomServiceRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ReservationRoomServices?> Delete(Guid reservationRoomId, Guid hotelServiceId, CancellationToken cancellationToken)
    {
        var reservationRoomService = await _context.ReservationRoomServices
                .FirstOrDefaultAsync(rt => rt.ReservationRoomId == reservationRoomId && rt.HotelServiceId == hotelServiceId, cancellationToken);

        if (reservationRoomService == null)
        {
            return null;
        }

        _context.ReservationRoomServices.Remove(reservationRoomService);
        await _context.SaveChangesAsync(cancellationToken);

        return reservationRoomService;
    }
}
