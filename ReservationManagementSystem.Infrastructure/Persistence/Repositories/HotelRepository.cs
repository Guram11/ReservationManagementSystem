using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
{
    private readonly DataContext _context;

    public HotelRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsHotelInUseAsync(Guid hotelId)
    {
        var isRoomTypeInUse = await _context.RoomTypes
            .AnyAsync(rt => rt.HotelId == hotelId);

        var isRateInUse = await _context.Rates
            .AnyAsync(rt => rt.HotelId == hotelId);

        var isHotelServiceInUse = await _context.HotelServices
            .AnyAsync(rt => rt.HotelId == hotelId);

        var isReservationInUse = await _context.Reservations
            .AnyAsync(r => r.HotelId == hotelId);

        return isRoomTypeInUse || isReservationInUse || isRateInUse || isHotelServiceInUse;
    }
}
